package task

import (
	"bufio"
	"bytes"
	"fmt"
	"io"
	"net"
	"time"

	"../util"
)

//TCPClientTask tcp客户端
type TCPClientTask struct {
	//IP ip
	IP string
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
	if t.IP, ok = data["Ip"].(string); !ok {
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
	data["Ip"] = t.IP
	data["Port"] = t.Port
	data["Content"] = t.Content
	data["Timeout"] = t.Timeout
	return data
}

//Run 执行任务
func (t *TCPClientTask) Run(session *Session) error {
	conn, err := net.DialTimeout("tcp", t.IP+":"+t.Port, time.Millisecond*time.Duration(t.Timeout))
	if err != nil {
		return err
	}
	defer conn.Close()
	if session.IsCancel() {
		return ErrCANCEL
	}
	_, err = conn.Write([]byte(t.Content + "\n"))
	if err != nil {
		return err
	}
	reader := bufio.NewReader(conn)
	for {
		if session.IsCancel() {
			return ErrCANCEL
		}
		data, err := reader.ReadBytes('\n')
		if err != nil {
			if err == io.EOF { //请求结束
				return nil
			}
			return err
		}
		data, suc := analysis(data)
		session.Write(data)
		if !suc {
			return fmt.Errorf("%s", data)
		}
	}
}

//解析返回数据的成功与失败
func analysis(msg []byte) ([]byte, bool) {
	index := bytes.Index(msg, []byte("|"))
	if index <= 0 {
		return msg, false
	}
	if bytes.Compare(msg[:index], []byte("0")) == 0 {
		return msg[index+1:], true
	}
	return msg[index+1:], false
}

func init() {
	util.RegisterType((*TCPClientTask)(nil))
}
