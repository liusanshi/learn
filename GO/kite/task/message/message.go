package message

import (
	"bufio"
	"bytes"
	"fmt"
	"io"
	"strconv"
)

//MessageType 消息类型
type MessageType int

const (
	// SystemMessage 系统消息
	SystemMessage = MessageType(0)
	// BusinessMessage 业务消息
	BusinessMessage = MessageType(1)
)

//curMsgID 当前的消息id
var curMsgID = 0

//Message 消息对象
type Message struct {
	ID      int
	Success bool
	Type    MessageType
	Content string
}

//String 将数据转换为字符串
func (m *Message) String() string {
	suc := 0
	if m.Success {
		suc = 1
	}
	return fmt.Sprintf("%d,%d,%d|%s", m.ID, suc, m.Type, m.Content)
}

//Bytes 转换为字节数组
func (m *Message) Bytes() []byte {
	return []byte(m.String())
}

// Init 将json数据转换为Message
func (m *Message) Init(msg []byte) error {
	index := bytes.IndexByte(msg, '|')
	if index <= 0 {
		return fmt.Errorf("Meessage formart error")
	}
	list := bytes.Split(msg[:index], []byte{','})
	if len(list) > 3 {
		return fmt.Errorf("Meessage Head formart error")
	}
	id, err := strconv.Atoi(string(list[0]))
	if err != nil {
		return err
	}
	m.ID = id
	if bytes.EqualFold(list[1], []byte{'1'}) {
		m.Success = true
	}
	if bytes.EqualFold(list[2], []byte{'1'}) {
		m.Type = BusinessMessage
	}
	m.Content = string(msg[index+1:])
	return nil
}

//ReadFrom 读取数据
func (m *Message) ReadFrom(r io.Reader) (int64, error) {
	nr := bufio.NewReaderSize(r, 1024)
	str, err := nr.ReadBytes('\n')
	if err != nil {
		return 0, err
	}
	return int64(len(str)), m.Init(str)
}

//WriteTo 写入数据
func (m *Message) WriteTo(w io.Writer) (int64, error) {
	nw := bufio.NewWriter(w)
	n, err := nw.Write(m.Bytes())
	return int64(n), err
}

//NewMessage 初始化一个消息
func NewMessage(suc bool, typ MessageType, msg string) *Message {
	curMsgID++
	return &Message{
		ID:      curMsgID,
		Success: suc,
		Type:    typ,
		Content: msg,
	}
}

//AnalysisMessage 解析一个消息
func AnalysisMessage(msg []byte) *Message {
	message := &Message{}
	message.Init(msg)
	return message
}
