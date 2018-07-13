package message

import (
	"bufio"
	"bytes"
	"io"
)

//Request 请求体
type Request struct {
	IMessage
	cmd string
	arg string
}

//SetMessage 设置body
func (r *Request) SetMessage(msg IMessage) {
	r.IMessage = msg
}

//Send 写入数据
func (r *Request) Send(w io.Writer) (int, error) {
	i, err := r.WriteTo(w)
	return int(i), err
}

//Message 获取响应body
func (r *Request) Message() IMessage {
	return r.IMessage
}

//ParseFormCmd 设置body
func (r *Request) ParseFormCmd(read io.Reader) (int64, error) {
	cmd := &CmdMessage{}
	n, err := cmd.ReadFrom(read)
	r.cmd = cmd.Cmd
	r.arg = cmd.Arg
	r.IMessage = cmd
	return n, err
}

//ParseFormFile 设置body
func (r *Request) ParseFormFile(read io.Reader) (int64, error) {
	r.IMessage = &FileMessage{}
	n, err := r.IMessage.ReadFrom(read)
	r.cmd = "ReceiveFile" //接收文件
	return n, err
}

//ParseForm 设置body
func (r *Request) ParseForm(read io.Reader) (int64, error) {
	nr := bufio.NewReader(read)
	b, err := nr.ReadByte()
	if err != nil {
		return 0, err
	}
	if b == '\n' {
		return 0, io.EOF
	}
	nnr := io.MultiReader(bytes.NewReader([]byte{b}), nr)
	if b >= '0' && b <= '9' { //上传的开始是数字
		return r.ParseFormFile(nnr)
	}
	return r.ParseFormCmd(nnr)
}

//Cmd 获取指令
func (r *Request) Cmd() string {
	return r.cmd
}

//Arg 获取参数
func (r *Request) Arg() string {
	return r.arg
}
