package task

import (
	"bufio"
	"context"
	"fmt"
	"io"
	"net"
	"time"

	"../util"
)

//TCPClientTask tcp客户端
type TCPClientTask struct {
	//Ip ip
	Ip string
	//Port 端口
	Port string
	//Timeout 超时时间
	Timeout int
	//Content 发送内容
	Content string
}

//Init 数据初始化
func (t *TCPClientTask) Init(data map[string]interface{}) error {
	var ok bool
	if t.Ip, ok = data["Ip"].(string); !ok {
		return fmt.Errorf("TCPClientTask Ip type error")
	}
	if t.Port, ok = data["Port"].(string); !ok {
		return fmt.Errorf("TCPClientTask Port type error")
	}
	if t.Content, ok = data["Content"].(string); !ok {
		return fmt.Errorf("TCPClientTask Content type error")
	}
	if timeout, ok := data["Timeout"].(float64); ok {
		t.Timeout = int(timeout)
	} else {
		return fmt.Errorf("TCPClientTask Content type error")
	}
	return nil
}

//ToMap 转换为map
func (t *TCPClientTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Ip"] = t.Ip
	data["Port"] = t.Port
	data["Content"] = t.Content
	data["Timeout"] = t.Timeout
	return data
}

//Run 执行任务
func (t *TCPClientTask) Run(ctx context.Context, w io.Writer) error {
	conn, err := net.DialTimeout("tcp", t.Ip+":"+t.Port, time.Millisecond*time.Duration(t.Timeout))
	if err != nil {
		return err
	}
	defer conn.Close()
	if isEnd(ctx) {
		return CANCEL
	}
	_, err = conn.Write([]byte(t.Content + "\n"))
	if err != nil {
		return err
	}
	reader := bufio.NewReader(conn)
	if isEnd(ctx) {
		return CANCEL
	}
	data, err := reader.ReadBytes('\n')
	if err != nil {
		return err
	}
	w.Write(data)
	return nil
}

func init() {
	util.RegisterType((*TCPClientTask)(nil))
}
