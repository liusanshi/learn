package ch8

import (
	"bufio"
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
	if _, err := this.Write([]byte("220 FTP Server v1.0\r\n")); err != nil {
		return
	}
	for {
		if this.closed {
			log.Println("closing...")
			return
		}
		// conn.SetReadDeadline(time.Now().Add(100 * time.Millisecond)) //100ms超时时间
		bstr, _, err := bufio.NewReader(conn).ReadLine()
		if err != nil {
			log.Print(err)
			return
		}
		strMethod := string(bstr)
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
			if _, err := this.Write([]byte("方法未找到\r\n")); err != nil {
				return
			}
		}
	}
}

func (this *User) Write(cmd []byte) (int, error) {
	length, err := this.conn.Write(cmd)
	if err != nil {
		log.Print(err)
	}
	return length, err
}

func (this *User) List() {
	writer := this.conn
	list, err := ioutil.ReadDir(this.ctx.path)
	if err != nil {
		fmt.Fprintf(writer, "server err:%v\r\n", err)
		return
	}

	for _, dir := range list {
		if dir.IsDir() {
			log.Printf("DIR: Name:%s Size:%d\n", dir.Name(), dir.Size())
			_, err = fmt.Fprintf(writer, "DIR: Name:%s Size:%d\r\n", dir.Name(), dir.Size())
		} else {
			log.Printf("DIR: Name:%s Size:%d\n", dir.Name(), dir.Size())
			_, err = fmt.Fprintf(writer, "FILE:Name:%s Size:%d\r\n", dir.Name(), dir.Size())
		}
		if err != nil {
			log.Print(err)
			return
		}
	}
}

func (this *User) Quit() {
	_, _ = fmt.Fprintf(this.conn, "Bye\r\n")
	this.closed = true
	this.conn.Close()
}

func StartFtpServer(port int, path string) *Ftp {
	ftp := &Ftp{
		port: port,
		path: path,
	}
	ftp.Start()
	return ftp
}
