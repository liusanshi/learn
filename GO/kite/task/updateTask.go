package task

import (
	"fmt"
	"time"

	"../util"
	"./core"
)

//UpdateTask 更新分支的任务
type UpdateTask struct{}

func init() {
	util.RegisterType((*UpdateTask)(nil))
}

//Init 数据初始化
func (c *UpdateTask) Init(data map[string]interface{}) error {
	return nil
}

//ToMap 数据转换为map
func (c *UpdateTask) ToMap() map[string]interface{} {
	return make(map[string]interface{})
}

//Run 更新分支
func (c *UpdateTask) Run(session *core.Session) error {
	defer session.BMan.Unlock() //解锁
	b, ok := session.GetCurBranchEntity()
	if !ok {
		return fmt.Errorf("branch:%s not exist", session.Branch)
	}
	b.Version++
	b.Time = time.Now().Format("2006-01-02 15:04:05")
	return session.BMan.Save()
}
