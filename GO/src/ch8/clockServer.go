package ch8

import (
	"io"
	"log"
	"net"
	"time"
)

func ClockServer() {
	listen, err := net.Listen("tcp", "127.0.0.1:8000")
	if err != nil {
		log.Print(err)
	}

	for {
		conn, err := listen.Accept()
		if err != nil {
			log.Print(err)
			continue
		}
		go handleConn(conn)
	}
}

func handleConn(conn net.Conn) {
	defer conn.Close()
	for {
		t := time.Now().Format("2006-01-02 15:04:05\n")
		_, err := io.WriteString(conn, t)
		if err != nil {
			log.Print(err)
			break
		}
		time.Sleep(1 * time.Second)
	}
}
