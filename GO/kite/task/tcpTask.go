package task

import (
	"bufio"
	"time"
	"net"
	"context"
	"fmt"
	"../util"
)

type TcpTask struct {
	Ip string
	Port string
	Timeout int
	Content string
}

func (this *TcpTask) Init(data map[string]interface{}) (error) {
	var ok bool
	if this.Ip, ok = data["Ip"].(string); !ok {
		return fmt.Errorf("TcpTask Ip type error")
	}
	if this.Port, ok = data["Port"].(string); !ok {
		return fmt.Errorf("TcpTask Port type error")
	}
	if this.Content, ok = data["Content"].(string); !ok {
		return fmt.Errorf("TcpTask Content type error")
	}
 	if timeout, ok := data["Timeout"].(float64); ok {
		this.Timeout = int(timeout)
	} else {
		return fmt.Errorf("TcpTask Content type error")
	}
	return nil
}

func (this *TcpTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Ip"] = this.Ip
	data["Port"] = this.Port
	data["Content"] = this.Content
	data["Timeout"] = this.Timeout
	return data
}

func (this *TcpTask) Run(ctx context.Context) (string, error){
	conn, err := net.DialTimeout("tcp", this.Ip + ":" + this.Port, time.Millisecond * time.Duration(this.Timeout))
	if err != nil {
		return "", err
	}
	defer conn.Close()
	if isEnd(ctx) {
		return CANCEL, nil
	}
	_, err = conn.Write([]byte(this.Content + "\n"))
	if err != nil {
		return "", err
	}
	reader := bufio.NewReader(conn)
	if isEnd(ctx) {
		return CANCEL, nil
	}
	return reader.ReadString('\n')
}

func init(){
	util.RegisterType((*TcpTask)(nil))
}