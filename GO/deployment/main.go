package main

import(
	"flag"
	"./client"
	"./config"
	// "./server"
	"strings"
	"log"
)

func main(){
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
			client.Command(*args)
		default :
			log.Println("方法名称错误")
	}
}