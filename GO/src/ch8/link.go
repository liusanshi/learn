package ch8

import (
	"fmt"
	"net/http"

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
