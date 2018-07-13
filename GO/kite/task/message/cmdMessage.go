package message

import (
	"bufio"
	"bytes"
	"fmt"
	"io"
)

//CmdMessage 请求结构体
type CmdMessage struct {
	//Cmd 指令
	Cmd string
	//Arg 参数
	Arg string
	innerData
}

//String 将数据转换为字符串
func (req *CmdMessage) String() string {
	return fmt.Sprintf("%s %s", req.Cmd, req.Arg)
}

//Close 关闭
func (req *CmdMessage) Close() error {
	return nil
}

//ReadFrom 读取数据
func (req *CmdMessage) ReadFrom(r io.Reader) (int64, error) {
	nr := bufio.NewReader(r)
	head, err := nr.ReadBytes('\n')
	if err != nil {
		if err != io.EOF {
			return 0, err
		}
		if len(head) == 0 {
			return 0, io.EOF
		}
	}
	head = bytes.TrimSpace(head)
	index := bytes.IndexByte(head, ' ')
	if index > -1 {
		req.Cmd = string(head[:index])
		req.Arg = string(head[index+1:])
	} else {
		req.Cmd = string(head)
	}
	return int64(len(head)), nil
}

//WriteTo 写入数据
func (req *CmdMessage) WriteTo(w io.Writer) (int64, error) {
	var (
		n   int
		err error
	)
	if len(req.Arg) > 0 {
		n, err = io.WriteString(w, fmt.Sprintf("%s %s\n", req.Cmd, req.Arg))
	} else {
		n, err = io.WriteString(w, fmt.Sprintf("%s\n", req.Cmd))
	}
	return int64(n), err
}

//NewCmdMessage 创建一个消息
func NewCmdMessage(cmd, arg string) *CmdMessage {
	return &CmdMessage{
		Cmd: cmd,
		Arg: arg,
	}
}
