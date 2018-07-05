package task

import (
	"net"
	"context"
	"fmt"
	"log"
	"bufio"
	"io"
	"../util"
)

type TcpServerTask struct {
	Port string
	TaskDict map[string]TaskList
}

func (this *TcpServerTask) Init(data map[string]interface{}) (error) {
	var ok bool
	if this.Port, ok = data["Port"].(string); !ok {
		return fmt.Errorf("TcpServerTask Port type error")
	}
	this.TaskDict = make(map[string]TaskList)
	if taskList, ok := data["TaskDict"].(map[string]interface{}); ok {
		for key, val := range taskList {
			if list, ok := val.([]interface{}); ok {
				taskListItem := TaskList{}
				err := taskListItem.Init(list)
				if err != nil {
					log.Printf("TcpServerTask - TaskList; err:%v\n", err)
					return err
				}
				this.TaskDict[key] = taskListItem
			} else {
				return fmt.Errorf("TcpServerTask TaskDict type error")
			}
		}
		return nil
	} else {
		return fmt.Errorf("TcpServerTask Port type error")
	}
}

func (this *TcpServerTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Port"] = this.Port
	dict := make(map[string][]interface{})
	for key, val := range this.TaskDict {
		dict[key] = val.ToArray()
	}
	data["TaskDict"] = dict
	return data
}

//监听端口号，接收请求，然后根据指令执行任务；将任务的结果输出给客户端
func (this *TcpServerTask) Run(ctx context.Context) (string, error){
	listen, err := net.Listen("tcp", "127.0.0.1:" + this.Port)
	if err != nil {
		log.Print(err)
		return "TcpServerTask Listen fail", err
	}
	for {
		if isEnd(ctx) {
			return CANCEL, nil
		}
		conn, err := listen.Accept()
		if err != nil {
			log.Print(err)
			continue
		}
		go this.handleConn(ctx, conn)
	}
}

//处理请求
func (this *TcpServerTask) handleConn(ctx context.Context, conn net.Conn) {
	reader := bufio.NewReader(conn)
	cmd, err := reader.ReadString('\n')
	writer := bufio.NewWriter(conn)
	defer conn.Close()
	if err != nil && err != io.EOF{
		log.Print(err)
		return
	}
	if task, ok := this.TaskDict[cmd]; ok {
		taskQueue := TaskQueue{ TaskList: task, ctx: ctx }
		err := taskQueue.Start(writer)
		if err != nil {
			log.Print(err)
			return
		}
		//执行成功
		writer.Write([]byte(fmt.Sprintf("method：%s execute success\n", cmd)))
	} else {
		writer.Write([]byte(fmt.Sprintf("method：%s not fount\n", cmd)))
	}
}

func init(){
	util.RegisterType((*TcpServerTask)(nil))
}