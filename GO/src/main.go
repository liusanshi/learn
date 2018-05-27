package main

import (
	"flag"
	"fmt"

	"github.com/myproject/src/ch8"
)

func main() {
	// ch8.Spinner()
	method := flag.String("func", "", "方法名称与路径")
	args := flag.String("args", "", "参数")

	flag.Parse()

	fmt.Println(*method)
	fmt.Println(*args)

	switch *method {
	case "ch8.Spinner":
		ch8.Spinner()
	case "ch8.ClockServer", "server":
		ch8.ClockServer()
	case "ch8.ClockClient", "client":
		ch8.ClockClient()
	case "ch8.ftpServer", "ftpServer":
		ch8.StartFtpServer(8000, "E:\\git\\learn\\GO")
	case "ch8.ftpClient", "ftpClient":
		ch8.StartFtpClient("127.0.0.1:8000")
	case "pipeline":
		ch8.PipeLine()
	case "parall":
		ch8.Parall()
	default:
		println("method not found")
	}
}
