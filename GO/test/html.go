package test


import (
	"strings"
	"net/http"
	// "text/template"
	"fmt"
	"golang.org/x/net/html"
)

func TestHtML(url string){
	resp, err := http.Get(url)
	if err != nil {
		fmt.Printf("get url:%s fail: %s\n", url, err)
		return
	}
	defer resp.Body.Close()
	if resp.StatusCode != http.StatusOK {
		fmt.Printf("get url err code: %d\n", resp.StatusCode)
		return
	}

	node, err := html.Parse(resp.Body)
	if err != nil {
		fmt.Printf("html parse fail: %s\n", err)
		return
	}

	for _, link := range visit(nil, node) {
		fmt.Println(link)
	}
}

func visit(links []string, n *html.Node) []string {
	if n.Type == html.ElementNode && n.Data == "a" {
		for _, a := range n.Attr {
			if a.Key == "href" {
				links = append(links, a.Val)
			}
		}
	}
	for c := n.FirstChild; c != nil; c = c.NextSibling {
		links = visit(links, c)
	}
	return links
}

func CountWordAndImage(url string)(words, images int, err error){
	resp, err := http.Get(url)
	if err != nil {
		fmt.Printf("get url:%s fail: %s\n", url, err)
		return
	}
	defer resp.Body.Close()
	if resp.StatusCode != http.StatusOK {
		fmt.Printf("get url err code: %d\n", resp.StatusCode)
		return
	}

	node, err := html.Parse(resp.Body)
	if err != nil {
		fmt.Printf("html parse fail: %s\n", err)
		return
	}

	words, images = countWordAndImage(node)
	return
}

func countWordAndImage(node *html.Node)(words, images int){
	if node != nil {
		if node.Type == html.ElementNode {
			if node.Data == "img" {
				images++
			} else if strings.Contains(",p,div,span,td,li,ld,b,label,", strings.ToLower(node.Data)) {
				words++
			}
		}

		for c := node.FirstChild; c != nil; c = c.NextSibling {
			w, i := countWordAndImage(c)
			words += w
			images += i
		}
	}
	return
}
