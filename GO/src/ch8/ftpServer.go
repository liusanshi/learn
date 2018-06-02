package ch8

import (
	"bufio"
	"bytes"
	"context"
	"fmt"
	"io/ioutil"
	"log"
	"net"
	"reflect"
	"strconv"
	"sync"
)

type Ftp struct {
	port     int
	path     string
	listener net.Listener
}

type User struct {
	Name   string
	Passwd string
	conn   net.Conn
	closed bool
	ctx    *Ftp
}

var cache = struct {
	data map[string]reflect.Value
	sync.RWMutex
}{
	data: make(map[string]reflect.Value),
}

func (this *Ftp) Start() {
	listen, err := net.Listen("tcp", "127.0.0.1:"+strconv.Itoa(this.port))
	if err != nil {
		log.Print(err)
		return
	}
	this.listener = listen
	ctx := context.Background()
	for {
		conn, err := listen.Accept()
		if err != nil {
			log.Print(err)
			continue
		}
		fmt.Printf("conn:%#v\n", conn)
		user := &User{
			conn: conn,
			ctx:  this,
		}
		go user.process(ctx)
	}
}

func (this *Ftp) Close() {
	this.listener.Close()
}

func (this *User) process(ctx context.Context) {
	conn := this.conn
	if _, err := this.Write([]byte("220 FTP Server v1.0")); err != nil {
		return
	}
	input := bufio.NewScanner(conn)
	for input.Scan() {
		if this.closed {
			log.Println("closing...")
			return
		}
		strMethod := input.Text()
		log.Printf("remoting ip:%v connecting; cmd:%s;\n", conn.RemoteAddr(), strMethod)
		cache.RLock()
		method, ok := cache.data[strMethod]
		cache.RUnlock()
		if !ok {
			method, ok = cache.data[strMethod]
			if !ok {
				method = reflect.ValueOf(this).MethodByName(strMethod)
				cache.Lock()
				cache.data[strMethod] = method
				cache.Unlock()
			}
		}
		if method.IsValid() {
			method.Call([]reflect.Value{})
		} else {
			if _, err := this.Write([]byte("方法[" + strMethod + "]未找到")); err != nil {
				return
			}
		}
	}
}

func (this *User) Write(cmd []byte) (int, error) {
	length, err := this.conn.Write(append(cmd, '\n', '\n'))
	if err != nil {
		log.Print(err)
	}
	return length, err
}

func (this *User) List() {
	writer := this.conn
	list, err := ioutil.ReadDir(this.ctx.path)
	if err != nil {
		fmt.Fprintf(writer, "server err:%v\n\n", err)
		return
	}
	var resp = bytes.NewBuffer(make([]byte, 0, 100))
	for _, dir := range list {
		if dir.IsDir() {
			_, err = fmt.Fprintf(resp, "DIR: Name:%s Size:%d\n", dir.Name(), dir.Size())
		} else {
			_, err = fmt.Fprintf(resp, "FILE:Name:%s Size:%d\n", dir.Name(), dir.Size())
		}
		if err != nil {
			log.Print(err)
			return
		}
	}
	log.Printf("%s\n", resp)
	this.Write(resp.Bytes())
}

func (this *User) Quit() {
	this.Write([]byte("Bye"))
	log.Println("Quit")
	this.closed = true
	err := this.conn.Close()
	if err != nil {
		log.Println(err)
	}
}

func StartFtpServer(port int, path string) *Ftp {
	ftp := &Ftp{
		port: port,
		path: path,
	}
	ftp.Start()
	return ftp
}
