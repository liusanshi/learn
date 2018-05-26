package ch8

import (
	"bufio"
	"log"
	"net"
	"os"
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
	for {
		cmd, _, err := bufio.NewReader(os.Stdin).ReadLine()
		if err != nil {
			log.Println(err)
			return
		}
		cmd = append(cmd, '\n')
		log.Printf("send msg:%s\n", cmd)
		_, err = conn.Write(cmd)
		if err != nil {
			log.Println(err)
			return
		}
		log.Println("result:")
		for {
			result, hasMore, err := bufio.NewReader(conn).ReadLine()
			if err != nil {
				log.Println(err)
				break
			}
			log.Printf("%s\n", result)
			log.Printf("%v\n", hasMore)
			if !hasMore {
				break
			}
		}
		if err != nil {
			log.Println(err)
			return
		}
		log.Println("完成一次交互")
	}

}
