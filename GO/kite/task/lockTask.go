package task

import (
	"fmt"

	"../util"
)

//LockTask 删除分支的任务
type LockTask struct{}

func init() {
	util.RegisterType((*LockTask)(nil))
}

//Init 数据初始化
func (c *LockTask) Init(data map[string]interface{}) error {
	return nil
}

//ToMap 数据转换为map
func (c *LockTask) ToMap() map[string]interface{} {
	return make(map[string]interface{})
}

//Run 获得锁
func (c *LockTask) Run(session *Session) error {
	if !session.BMan.TryLock() {
		return fmt.Errorf("获取锁失败，请稍后重试~")
	}
	return nil
}