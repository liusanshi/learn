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
	args := flag.String("args", "", "参数")

	flag.Parse()

	switch *method {
	case "config-list":
		config.List(*args)
	case "config-set":
		params := strings.Split(*args, " ")
		if len(params) < 2 {
			log.Fatalf("参数格式错误")
		}
		config.Set(params[0], params[1])
	case "client":
		params := strings.Split(*args, " ")
		if len(params) < 2 {
			log.Fatalf("参数格式错误")
		}
		client.Client(params[0], params[1])
	case "server":
		server.Sev(*args)
	default:
		log.Println("方法名称错误")
	}
}
