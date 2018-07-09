package task

import (
	"bufio"
	"context"
	"fmt"
	"io"
	"log"
	"net"

	"../util"
)

//TCPServerTask tcp服务的任务
type TCPServerTask struct {
	Port     string
	TaskDict Map
}

//Init 数据的初始化
func (t *TCPServerTask) Init(data map[string]interface{}) error {
	var ok bool
	if t.Port, ok = data["Port"].(string); !ok {
		return fmt.Errorf("TCPServerTask Port type error")
	}
	t.TaskDict = NewMap()
	if dict, ok := data["TaskDict"].(map[string]interface{}); ok {
		return t.TaskDict.Init(dict)
	}
	return fmt.Errorf("TCPServerTask Port type error")
}

//ToMap 数据转换为map
func (t *TCPServerTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Port"] = t.Port
	data["TaskDict"] = t.TaskDict.ToMap()
	return data
}

//Run 监听端口号，接收请求，然后根据指令执行任务；将任务的结果输出给客户端
func (t *TCPServerTask) Run(ctx context.Context, w io.Writer) error {
	listen, err := net.Listen("tcp", "127.0.0.1:"+t.Port)
	if err != nil {
		log.Print(err)
		return err
	}
	for {
		if isEnd(ctx) {
			return ErrCANCEL
		}
		conn, err := listen.Accept()
		if err != nil {
			log.Print(err)
			continue
		}
		go t.handleConn(ctx, conn)
	}
}

//handleConn 处理请求
func (t *TCPServerTask) handleConn(ctx context.Context, conn net.Conn) {
	reader := bufio.NewReader(conn)
	cmd, err := reader.ReadBytes('\n')
	cmd = cmd[:len(cmd)-1]
	defer conn.Close()
	if err != nil && err != io.EOF {
		log.Print(err)
		return
	}
	if task, ok := t.TaskDict[string(cmd)]; ok {
		err := task.Run(ctx, conn)
		if err != nil {
			log.Print(err)
			fmt.Fprintf(conn, "method：%s; execute fail:%v\n", cmd, err)
			return
		}
		//执行成功
		fmt.Fprintf(conn, "method：%s; execute success\n", cmd)
	} else {
		fmt.Fprintf(conn, "method：%s; not fount\n", cmd)
	}
}

func init() {
	util.RegisterType((*TCPServerTask)(nil))
}
