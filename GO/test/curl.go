package test

import (
	"time"
	"strings"
	"io"
	"net/http"
	"io/ioutil"
	"fmt"
	"os"
)

func Curl(){
	for _, url := range os.Args[1:] {
		if !strings.HasPrefix(url, "http://") {
			url = "http://" + url
		}
		resp, err := http.Get(url)
		if err != nil {
			fmt.Printf("fetch: %v, err: %v", url, err)
			os.Exit(1)
		}
		//b, err := ioutil.ReadAll(resp.Body)
		_, err = io.Copy(os.Stdout, resp.Body)
		resp.Body.Close()

		fmt.Printf("status:%d", resp.StatusCode)

		if err != nil {
			fmt.Printf("fetch: %v, read err: %v", url, err)
			os.Exit(1)
		}
		// fmt.Printf("html: %s", b)
	}
}

func FetchAllSeq(urls []string){
	start := time.Now()
	chList := make([]chan string, len(urls))
	for i, url := range urls {
		chList[i] = make(chan string)
		go fetch(url, chList[i])
	}
	for i,_ := range urls {
		fmt.Println(<-chList[i])
	}
	fmt.Printf("FetchAllSeq: 总耗时：%.7fs\n", time.Since(start).Seconds())
}

func FetchAll(urls []string){
	start := time.Now()
	ch := make(chan string)
	for _, url := range urls {
		go fetch(url, ch)
	}
	for range urls{
		fmt.Println(<-ch)
	}
	fmt.Printf("FetchAll: 总耗时：%.7fs\n", time.Since(start).Seconds())
}

func fetch(url string, res chan <- string){
	start := time.Now()
	if !strings.HasPrefix(url, "http://"){
		url = "http://" + url
	}
	resp, err := http.Get(url)
	if err != nil {
		res <- fmt.Sprintf("url:%s,err:%v", url, err)
		return
	}
	nbytes, err := io.Copy(ioutil.Discard, resp.Body)
	resp.Body.Close()
	if err != nil {
		res <- fmt.Sprintf("url:%s,read err:%v", url, err)
		return
	}
	secs := time.Since(start).Seconds()
	res <- fmt.Sprintf("time:%.7fs, resp:%7d, url:%s", secs, nbytes, url)
	// close(res)
}