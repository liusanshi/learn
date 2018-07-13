package message

import (
	"bufio"
	"bytes"
	"fmt"
	"io"
	"strconv"
)

//IMessage 消息结构体
type IMessage interface {
	io.Closer
	io.WriterTo
	io.ReaderFrom
	GetVal(string) (interface{}, bool)
	SetVal(string, interface{})
	GetStrVal(key string) string
}

//内部数据结构
type innerData struct {
	data map[string]interface{}
}

// GetVal 获取数据
func (c *innerData) GetVal(key string) (interface{}, bool) {
	data, ok := c.data[key]
	return data, ok
}

// SetVal 设置值
func (c *innerData) SetVal(key string, val interface{}) {
	c.data[key] = val
}

// GetStrVal 获取string值
func (c *innerData) GetStrVal(key string) string {
	data, ok := c.data[key]
	if !ok {
		return ""
	}
	if cmd, ok := data.(string); ok {
		return cmd
	}
	return ""
}

//Type 消息类型
type Type int

const (
	// SystemMessage 系统消息
	SystemMessage = Type(0)
	// BusinessMessage 业务消息
	BusinessMessage = Type(1)
)

//curMsgID 当前的消息id
var curMsgID = 0

//Message 消息对象
type Message struct {
	ID      int
	Success bool
	Type    Type
	Content string
	innerData
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

//Close 关闭
func (m *Message) Close() error {
	return nil
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
	nr := bufio.NewReader(r)
	str, err := nr.ReadBytes('\n')
	if err != nil {
		return 0, err
	}
	return int64(len(str)), m.Init(bytes.TrimSpace(str))
}

//WriteTo 写入数据
func (m *Message) WriteTo(w io.Writer) (int64, error) {
	n, err := w.Write(m.Bytes())
	return int64(n), err
}

//NewMessage 创建一个消息
func NewMessage(suc bool, typ Type, msg string) *Message {
	curMsgID++
	return &Message{
		Success: suc,
		Type:    typ,
		ID:      curMsgID,
		Content: msg,
	}
}
