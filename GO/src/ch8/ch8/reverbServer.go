package ch8

import (
	"bufio"
	"fmt"
	"log"
	"net"
	"strings"
	"time"
)

func EchoServer() {
	listen, err := net.Listen("tcp", "127.0.0.1:8080")
	if err != nil {
		log.Print(err)
		return
	}
	conn, err := listen.Accept()
	if err != nil {
		log.Print(err)
		return
	}
	handlerConn(conn)
}

func handlerConn(conn net.Conn) {
	input := bufio.NewScanner(conn)
	for input.Scan() {
		go echo(conn, input.Text(), 1*time.Second)
	}
	conn.Close()
}

func echo(conn net.Conn, shout string, delay time.Duration) {
	fmt.Fprintln(conn, "\t", strings.ToUpper(shout))
	time.Sleep(delay)
	fmt.Fprintln(conn, "\t", shout)
	time.Sleep(delay)
	fmt.Fprintln(conn, "\t", strings.ToLower(shout))
}
