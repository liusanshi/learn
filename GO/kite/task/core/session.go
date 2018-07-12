package core

import (
	"context"
	"fmt"
	"io"

	"../message"
)

// Session 会话
type Session struct {
	ID    string
	Ctx   context.Context
	data  map[string]interface{}
	BMan  *BranchManager
	write io.Writer
}

const (
	//CmdKey 指令的key
	CmdKey = "CMD"
	// CurrentBranchKey 当前指令的key
	CurrentBranchKey = "CURRENTBRANCH"
	//WorkSpace 工作空间
	WorkSpace = "WorkSpace"
)

// GetVal 获取数据
func (c *Session) GetVal(key string) (interface{}, bool) {
	data, ok := c.data[key]
	return data, ok
}

// SetVal 设置值
func (c *Session) SetVal(key string, val interface{}) {
	c.data[key] = val
}

// GetStrVal 获取string值
func (c *Session) GetStrVal(key string) string {
	data, ok := c.data[key]
	if !ok {
		return ""
	}
	if cmd, ok := data.(string); ok {
		return cmd
	}
	return ""
}

//GetWorkSpace 获取工作区
func (c *Session) GetWorkSpace() string {
	return c.GetStrVal(WorkSpace)
}

// SetWorkSpace 设置工作区
func (c *Session) SetWorkSpace(ws string) {
	c.data[CmdKey] = ws
}

//GetCMD 获取指令
func (c *Session) GetCMD() string {
	return c.GetStrVal(CmdKey)
}

// SetCMD 设置指令
func (c *Session) SetCMD(cmd string) {
	c.data[CmdKey] = cmd
}

//GetCurrentBranch 获取操作的分支
func (c *Session) GetCurrentBranch() string {
	return c.GetStrVal(CurrentBranchKey)
}

// SetCurrentBranch 设置操作的分支
func (c *Session) SetCurrentBranch(branch string) {
	c.data[CurrentBranchKey] = branch
}

//GetCurBranchEntity 获取当前分支的实体
func (c *Session) GetCurBranchEntity() (*Branch, bool) {
	b := c.GetCurrentBranch()
	if len(b) > 0 {
		return c.BMan.GetBranch(b)
	}
	return nil, false
}

// IsCancel 会话是否取消
func (c *Session) IsCancel() bool {
	select {
	case <-c.Ctx.Done():
		return true
	default:
		return false
	}
}

//Copy 复制一个子会话
func (c *Session) Copy(w io.Writer, keys ...string) *Session {
	s := &Session{
		ID:    c.ID + "/id",
		data:  make(map[string]interface{}),
		Ctx:   c.Ctx,
		BMan:  c.BMan,
		write: w,
	}
	for _, k := range keys {
		if val, ok := c.GetVal(k); ok {
			s.SetVal(k, val)
		}
	}
	return s
}

//Write 实现io.Writer接口
func (c *Session) Write(p []byte) (n int, err error) {
	return c.write.Write(p)
}

//Printf 格式化输出
func (c *Session) Printf(suc bool, typ message.Type, format string, a ...interface{}) (n int, err error) {
	msg := message.NewMessage(suc, typ, fmt.Sprintf(format, a...))
	return c.Write(msg.Bytes())
}

//NewSession 创建一个会话
func NewSession(ctx context.Context, id string, w io.Writer, bm *BranchManager) *Session {
	return &Session{
		ID:    id,
		data:  make(map[string]interface{}),
		Ctx:   ctx,
		write: w,
		BMan:  bm,
	}
}
