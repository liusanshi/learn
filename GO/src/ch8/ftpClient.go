package ch8

import (
	"bufio"
	"bytes"
	"fmt"
	"io"
	"log"
	"net"
	"os"
	"time"
)

func StartFtpClient(addr string) {
	conn, err := net.Dial("tcp", addr)
	if err != nil {
		log.Println(err)
		return
	}
	_, err = conn.Write([]byte("hi~\n"))
	if err != nil {
		log.Println(err)
		return
	}
	defer conn.Close()
	conn.Write([]byte("HI~\n"))
	go mustCopy(os.Stdout, conn)
	mustCopy(conn, os.Stdin)
}

func mustCopybuff(dis io.Writer, src io.Reader) {
	buf := make([]byte, 1024)
	count, err := io.CopyBuffer(dis, src, buf)
	if count > 0 {
		fmt.Println("end...")
	}
	if err != nil {
		log.Fatal(err)
	}
}

func readConn(conn net.Conn) {
	reader := bufio.NewReader(conn)
	conn.SetReadDeadline(time.Now().Add(1 * time.Second)) //1s超时
	for {
		log.Println("收到:")
		var result = make([]byte, 256)
		_, err := reader.Read(result)
		index := bytes.IndexAny(result, "\n\n")

		if index >= 0 {
			result = result[:index+1]
		}
		log.Printf("%s\n", result)
		// if index >= 0 || num == 0 {
		// 	break
		// }
		if err != nil {
			log.Println(err)
			break
		}
	}
}

func compSlice(arr1, arr2 []byte) bool {
	if arr1 == nil && arr2 == nil {
		return true
	} else if arr1 == nil || arr2 == nil {
		return false
	} else if len(arr1) != len(arr2) {
		return false
	}
	for i, b := range arr2 {
		if b != arr1[i] {
			return false
		}
	}
	return true
}
