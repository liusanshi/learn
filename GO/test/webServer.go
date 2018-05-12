package test

import (
	"strconv"
	"sync"
	"fmt"
	"log"
	"net/http"
	"html/template"
)

var (
	count int
	mu sync.RWMutex
)

func WebServer(){
	http.HandleFunc("/test", func(resp http.ResponseWriter, req *http.Request){
		fmt.Fprintf(resp, "hello world\n")
	})
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
	http.HandleFunc("/issues", IssuesInfo)
	http.HandleFunc("/escape", HTMLEscape)
	http.HandleFunc("/img", func(resp http.ResponseWriter, req *http.Request){
		if err := req.ParseForm(); err != nil{
			log.Print(err);
		}
		// cycle,err := strconv.Atoi(req.Form.Get("cycle"))
		strcycle := req.Form.Get("cycle")
		if strcycle == "" {
			strcycle = "5"
		}
		cycle,err := strconv.ParseFloat(strcycle, 64)
		if err != nil {
			fmt.Fprintf(resp, "cycle type err")
			return
		}
		Lissajous(resp, cycle)
	})
	http.HandleFunc("/surface", func(resp http.ResponseWriter, req *http.Request){
		resp.Header().Add("Content-Type", "text/html; charset=UTF-8"); //指示输出为html格式不是文本
		Surface(resp)
	})
	log.Fatal(http.ListenAndServe("127.0.0.1:8888", nil))
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

func IssuesInfo(resp http.ResponseWriter, req *http.Request){
	issuessList := template.Must(template.New("issues").Parse(`
		<h1>{{.TotalCount}}</h1>
		<table>
		<tr style="text-align:left;">
			<th>#</th>
			<th>State</th>
			<th>User</th>
			<th>Title</th>
		</tr>
		{{range .Items}}
			<tr>
				<td><a href="{{.HTMLURL}}" />{{.Number}}</td>
				<td>{{.State}}</td>
				<td><a href="{{.User.HTMLURL}}">{{.User.Login}}</td>
				<td><a href="{{.HTMLURL}}" />{{.Title}}</td>
			</tr>
		{{end}}
		<table>
		`))

		result, err := SearchIssues([]string{"repo:golang/go", "is:open", "json", "decoder"})
		if err != nil {
			fmt.Fprintf(resp, "search issues fail: %s\n", err)
			return
		}
		issuessList.Execute(resp, result)
}

func HTMLEscape(resp http.ResponseWriter, req *http.Request){
	var data struct {
		A string
		B template.HTML
	}
	data.A = "<b>Hello!</b>"
	data.B = "<b>Hello!</b>"
	template.Must(template.New("escape").Parse(`<p>A:{{.A}}</p><p>B:{{.B}}</p>`)).
		Execute(resp, data)
}