package test

import (
	"strconv"
	"sync"
	"fmt"
	"log"
	"net/http"
)

var (
	count int
	mu sync.RWMutex
)

func WebServer(){
	http.HandleFunc("/", func(resp http.ResponseWriter, req *http.Request){
		mu.Lock()
		count++
		mu.Unlock()
		fmt.Fprintf(resp, "URL.path=%q\n", req.URL.Path)
	});
	http.HandleFunc("/count", func(resp http.ResponseWriter, req *http.Request){
		mu.RLock()
		fmt.Fprintf(resp, "count=%d\n", count)
		mu.RUnlock()
	});
	http.HandleFunc("/info", info)
	http.HandleFunc("/img", func(resp http.ResponseWriter, req *http.Request){
		if err := req.ParseForm(); err != nil{
			log.Print(err);
		}
		cycle,err := strconv.Atoi(req.Form.Get("cycle"))
		if err != nil {
			fmt.Fprintf(resp, "cycle type err")
			return
		}
		Lissajous(resp, cycle)
	})
	log.Fatal(http.ListenAndServe("127.0.0.1:8080", nil))
}

func info(resp http.ResponseWriter, req *http.Request){
	fmt.Fprintf(resp, "Method:%s,url:%s,proto:%s\n", req.Method, req.URL, req.Proto)
	for k,v := range req.Header {
		fmt.Fprintf(resp, "header:[%q] = %q\n", k, v)
	}

	fmt.Fprintf(resp, "Host=%q\n", req.Host)
	fmt.Fprintf(resp, "RemoteAddr=%q\n", req.RemoteAddr)
	if err := req.ParseForm(); err != nil{
		log.Print(err);
	}
	for k ,v := range req.Form{
		fmt.Fprintf(resp, "Form:[%q] = %q\n", k, v)
	}
}