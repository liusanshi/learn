package main

import (
	"flag"
	"log"
	"strings"

	"./client"
	"./config"
	"./server"
)

func main() {
	method := flag.String("func", "", "方法名称与路径")
	fpath := flag.String("path", "", "配置文件路径")
	cmd := flag.String("cmd", "", "指令")
	work := flag.String("workspace", "", "工作区")
	args := flag.String("args", "", "参数")

	flag.Parse()

	switch *method {
	case "config-list":
		config.List(*args, *fpath)
	case "config-set":
		params := strings.Split(*args, " ")
		if len(params) < 2 {
			log.Fatalf("参数格式错误")
		}
		config.Set(params[0], params[1], "")
	case "client":
		client.Client(*fpath, *cmd)
	case "server":
		server.Sev(*fpath, *work)
	default:
		log.Println("方法名称错误")
	}
}
