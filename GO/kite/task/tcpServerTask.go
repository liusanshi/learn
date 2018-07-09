package task

import (
	"bufio"
	"context"
	"fmt"
	"io"
	"log"
	"net"
	"strings"

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
	args, err := reader.ReadBytes('\n')
	defer conn.Close()
	if err != nil && err != io.EOF {
		log.Print(err)
		return
	}
	if len(args) <= 2 {
		fmt.Fprintf(conn, "param is empty\n")
		return
	}
	args = args[:len(args)-1]
	params := strings.Split(string(args), " ")
	if task, ok := t.TaskDict[params[0]]; ok {
		tctx := ctx.Value(TaskCONTEXTKEY).(*Context)
		tctx.SetVal("cmd", params[0])
		if len(params) > 1 {
			tctx.SetVal(BranchCtxKey, params[1])
		}
		err := task.Run(ctx, conn)
		if err != nil {
			log.Print(err)
			fmt.Fprintf(conn, "method：%s; execute fail:%v\n", args, err)
			return
		}
		//执行成功
		fmt.Fprintf(conn, "method：%s; execute success\n", params[0])
	} else {
		fmt.Fprintf(conn, "method：%s; not fount\n", params[0])
	}
}

func init() {
	util.RegisterType((*TCPServerTask)(nil))
}
