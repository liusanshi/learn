package ch8

import (
	"io"
	"log"
	"net"
	"os"
)

func EchoClient() {
	conn, err := net.Dial("tcp", "127.0.0.1:8080")
	if err != nil {
		log.Fatal(err)
	}
	done := make(chan struct{})
	go func() {
		io.Copy(os.Stdout, conn)
		log.Println("done")
		close(done)
	}()
	go func() {
		<-done
		log.Println("closing...")
		conn.(*net.TCPConn).CloseWrite()
	}()
	mustCopy(conn, os.Stdin)
}
