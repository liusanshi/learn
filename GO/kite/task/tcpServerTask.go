package task

import (
	"bufio"
	"fmt"
	"io"
	"log"
	"net"
	"strings"

	"../util"
	"./core"
	"./message"
)

//TCPServerTask tcp服务的任务
type TCPServerTask struct {
	Port     string
	TaskDict core.Map
}

//Init 数据的初始化
func (t *TCPServerTask) Init(data map[string]interface{}) error {
	var ok bool
	if t.Port, ok = data["Port"].(string); !ok {
		return fmt.Errorf("TCPServerTask Port type error")
	}
	t.TaskDict = core.NewMap()
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
func (t *TCPServerTask) Run(session *core.Session) error {
	listen, err := net.Listen("tcp", "127.0.0.1:"+t.Port)
	if err != nil {
		log.Print(err)
		return err
	}
	for {
		if session.IsCancel() {
			return core.ErrCANCEL
		}
		conn, err := listen.Accept()
		if err != nil {
			log.Print(err)
			continue
		}
		go t.handleConn(session.Copy(conn, core.WorkSpace) /* 复制一个新的会话 */, conn)
	}
}

//handleConn 处理请求
func (t *TCPServerTask) handleConn(session *core.Session, conn net.Conn) {
	reader := bufio.NewReader(conn)
	args, err := reader.ReadBytes('\n')
	defer conn.Close()
	if err != nil && err != io.EOF {
		log.Print(err)
		return
	}
	if len(args) <= 2 {
		session.Printf(false, message.SystemMessage, "param is empty\n")
		return
	}
	args = args[:len(args)-1]
	params := strings.Split(string(args), " ")
	if task, ok := t.TaskDict[params[0]]; ok {
		session.SetCMD(params[0])
		if len(params) > 1 {
			session.SetCurrentBranch(params[1])
		}
		err := task.Run(session)
		if err != nil {
			log.Print(err)
			session.Printf(false, message.SystemMessage, "method：%s; execute fail:%v\n", args, err)
			return
		}
		log.Printf("method：%s; execute success\n", params[0])
		//执行成功
		session.Printf(true, message.SystemMessage, "method：%s; execute success\n", params[0])
	} else {
		log.Printf("method：%s; not fount\n", params[0])
		session.Printf(false, message.SystemMessage, "method：%s; not fount\n", params[0])
	}
}

func init() {
	util.RegisterType((*TCPServerTask)(nil))
}
