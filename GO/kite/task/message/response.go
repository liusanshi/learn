package message

import (
	"io"
)

//Response 响应结构
type Response struct {
	IMessage
}

//SetMessage 设置消息
func (r *Response) SetMessage(msg IMessage) {
	r.IMessage = msg
}

//Response 写入数据
func (r *Response) Write(w io.Writer) (int, error) {
	i, err := r.WriteTo(w)
	return int(i), err
}

//Message 获取响应body
func (r *Response) Message() IMessage {
	return r.IMessage
}

//ParseForm 设置body
func (r *Response) ParseForm(read io.Reader) (int64, error) {
	r.IMessage = &Message{}
	return r.IMessage.ReadFrom(read)
}
