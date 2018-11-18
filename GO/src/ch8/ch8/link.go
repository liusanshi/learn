package ch8

import (
	"bytes"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"net/url"
	"os"
	"path"
	"sync"
	"time"

	"golang.org/x/net/html"
	// "golang.org/x/net/html"
)

type Element html.Node

//查找节点的属性
func (this *Element) getAttr(key string) (string, bool) {
	for _, a := range this.Attr {
		if a.Key != key {
			continue
		}
		return a.Val, true
	}
	return "", false
}

//文档类型
const (
	HTML = iota
	TEXT
	JS
	CSS
	IMG
)

type linkInfo struct {
	content  []byte
	linkType int
	URL      *url.URL
	level    int //层级
}

var urlCache = struct {
	m map[string]bool
	sync.RWMutex
}{
	m: make(map[string]bool),
}

//保存文件
func (this *linkInfo) save(filepath string) error {
	if len(this.content) <= 0 {
		return nil
	}
	host := this.URL.Hostname()
	filepath += "/" + host
	filename := this.URL.Path
	if filename == "/" || filename == "" {
		filename = "/index.html"
	}
	filepath += filename
	distdir := path.Dir(filepath)
	_, err := os.Stat(distdir)
	if err != nil {
		if os.IsNotExist(err) {
			err = os.MkdirAll(distdir, os.ModePerm)
			if err != nil {
				return err
			}
		} else {
			return err
		}
	}
	if filepath[len(filepath)-1:] == "/" {
		filepath += "index.htm"
	}
	fmt.Printf("begin save file:%s\n", filepath)
	return ioutil.WriteFile(filepath, this.content, os.ModePerm)
}

//获取url
func (this *linkInfo) getUrl(url string) string {
	if this.URL != nil {
		url = this.URL.String()
	}
	return url
}

//下载文件
func (this *linkInfo) download(url string) error {
	url = this.getUrl(url)
	if len(url) <= 0 {
		return nil
	}
	fmt.Printf("begin download level:%d, type:%d, url:%s\n", this.level, this.linkType, url)
	client := &http.Client{
		Timeout: 1 * time.Second,
	}
	resp, err := client.Get(url)
	if err != nil {
		return err
	}
	if resp.StatusCode != http.StatusOK {
		resp.Body.Close()
		return fmt.Errorf("getting %s: %s", url, resp.Status)
	}
	content, err := ioutil.ReadAll(resp.Body)
	resp.Body.Close()
	if err != nil {
		return err
	}
	this.content = content
	this.URL = resp.Request.URL
	return nil
}

func (this *linkInfo) extract() ([]*linkInfo, error) {
	links := make([]*linkInfo, 0, 20)
	visitNode := func(n *html.Node) {
		if n.Type == html.ElementNode {
			ele := (*Element)(n)
			key := ""
			docType := TEXT
			switch n.Data {
			case "link":
				docType = CSS
				key = "href"
			case "a":
				docType = HTML
				key = "href"
			case "script":
				docType = JS
				key = "src"
			case "img":
				docType = IMG
				key = "src"
			}
			val, ok := ele.getAttr(key)
			if ok {
				link, err := this.URL.Parse(val)
				if err == nil && link.Opaque == "" {
					links = append(links, &linkInfo{
						URL:      link,
						linkType: docType,
						level:    this.level + 1,
					})
				}
			}
		}
	}
	doc, err := html.Parse(bytes.NewReader(this.content))
	if err != nil {
		return nil, err
	}
	forEachNode(doc, visitNode, nil)
	return links, nil
}

func (this *linkInfo) Crawler(url, path string) error {
	url = this.getUrl(url)
	if _, ok := urlCache.m[url]; ok {
		return nil
	}
	urlCache.Lock()
	if _, ok := urlCache.m[url]; ok {
		urlCache.Unlock()
		return nil
	}
	urlCache.m[url] = true
	urlCache.Unlock()
	err := this.download(url)
	if err != nil {
		fmt.Println(err)
		return err
	}
	err = this.save(path)
	if err != nil {
		fmt.Println(err)
		return err
	}
	if this.linkType != HTML || this.level >= 10 {
		return nil
	}
	list, err := this.extract()
	if err != nil {
		return err
	}
	for _, l := range list {
		err = l.Crawler("", path)
		if err != nil {
			log.Println(err)
			continue
		}
	}
	return nil
}

func LinkInfoTest(url string) error {
	link := linkInfo{
		linkType: HTML,
	}
	return link.Crawler(url, "e:/download")
}

func Crawler(url string) {
	breadthFirst(func(url string) []string {
		links, err := Extract(url)
		if err != nil {
			fmt.Println(err)
			return []string{}
		}
		for _, link := range links {
			fmt.Println(link)
		}
		return links
	}, []string{url})
}

func Extract(url string) ([]string, error) {
	resp, err := http.Get(url)
	if err != nil {
		return nil, err
	}
	if resp.StatusCode != http.StatusOK {
		resp.Body.Close()
		return nil, fmt.Errorf("getting %s: %s", url, resp.Status)
	}
	doc, err := html.Parse(resp.Body)
	resp.Body.Close()
	links := []string{}
	visitNode := func(n *html.Node) {
		if n.Type == html.ElementNode {
			ele := (*Element)(n)
			key := ""
			switch n.Data {
			case "a":
			case "link":
				key = "href"
			case "script":
			case "img":
				key = "src"
			}
			val, ok := ele.getAttr(key)
			if ok {
				link, err := resp.Request.URL.Parse(val)
				if err == nil && link.Opaque == "" {
					links = append(links, link.String())
				}
			}
		}
	}
	forEachNode(doc, visitNode, nil)
	return links, nil
}

func forEachNode(doc *html.Node, pre, post func(*html.Node)) {
	if pre != nil {
		pre(doc)
	}
	for c := doc.FirstChild; c != nil; c = c.NextSibling {
		forEachNode(c, pre, post)
	}
	if post != nil {
		post(doc)
	}
}

func breadthFirst(f func(item string) []string, worklist []string) {
	seen := make(map[string]bool)
	for len(worklist) > 0 {
		items := worklist
		worklist = nil
		for _, item := range items {
			if !seen[item] {
				seen[item] = true
				worklist = append(worklist, f(item)...)
			}
		}
	}
}
