package task

import (
	"context"
	"io"
	"time"

	"../util"
)

//InitTask 创建分支的任务
type InitTask struct{}

func init() {
	util.RegisterType((*InitTask)(nil))
}

//Init 数据初始化
func (c *InitTask) Init(data map[string]interface{}) error {
	return nil
}

//ToMap 数据转换为map
func (c *InitTask) ToMap() map[string]interface{} {
	return make(map[string]interface{})
}

//Run 创建分支
func (c *InitTask) Run(ctx context.Context, write io.Writer) error {
	tctx := ctx.Value(TaskCONTEXTKEY).(*Context)
	branch, _ := tctx.GetVal(BranchCtxKey)
	strBranch := branch.(string)
	tctx.List = append(tctx.List, &Branch{
		Name:    strBranch,
		Version: 1,
		Time:    time.Now().Format("2006-01-02 15:04:05"),
		Path:    "",
	})
	return tctx.Save()
}
