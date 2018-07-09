package task

import (
	"context"
	"io"

	"../util"
)

//DeleteTask 删除分支的任务
type DeleteTask struct{}

func init() {
	util.RegisterType((*DeleteTask)(nil))
}

//Init 数据初始化
func (c *DeleteTask) Init(data map[string]interface{}) error {
	return nil
}

//ToMap 数据转换为map
func (c *DeleteTask) ToMap() map[string]interface{} {
	return make(map[string]interface{})
}

//Run 删除分支
func (c *DeleteTask) Run(ctx context.Context, write io.Writer) error {
	tctx := ctx.Value(TaskCONTEXTKEY).(*Context)
	branch, _ := tctx.GetVal(BranchCtxKey)
	strBranch := branch.(string)
	tctx.DelBranch(strBranch)
	return tctx.Save()
}
