package task

import (
	"context"
	"fmt"
	"io"
	"time"

	"../util"
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
func (c *UpdateTask) Run(ctx context.Context, write io.Writer) error {
	tctx := ctx.Value(TaskCONTEXTKEY).(*Context)
	branch, _ := tctx.GetVal(BranchCtxKey)
	strBranch := branch.(string)
	b, ok := tctx.GetBranch(strBranch)
	if !ok {
		return fmt.Errorf("branch not exists")
	}
	b.Version++
	b.Time = time.Now().Format("2006-01-02 15:04:05")
	return tctx.Save()
}
