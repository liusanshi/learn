package ch8

import (
	"io"
	"log"
	"net"
	"os"
)

func ClockClient() {
	conn, err := net.Dial("tcp", "127.0.0.1:8000")
	if err != nil {
		log.Print(err)
	}
	defer conn.Close()
	mustCopy(os.Stdout, conn)
}

func mustCopy(dis io.Writer, src io.Reader) {
	if _, err := io.Copy(dis, src); err != nil {
		log.Fatal(err)
	}
}
