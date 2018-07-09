package task

import (
	"context"
	"fmt"
	"io"

	"../util"
)

//ListTask 显示所有分支的list
type ListTask struct{}

//Init 数据初始化
func (c *ListTask) Init(data map[string]interface{}) error {
	return nil
}

//ToMap 数据转换为map
func (c *ListTask) ToMap() map[string]interface{} {
	return make(map[string]interface{})
}

//Run 执行任务
func (c *ListTask) Run(ctx context.Context, write io.Writer) error {
	tctx, ok := ctx.Value(TaskCONTEXTKEY).(*Context)
	if !ok {
		return fmt.Errorf("ListTask type err")
	}
	for _, b := range tctx.List {
		fmt.Fprintf(write, "%s\t%d\t%s\n", b.Name, b.Version, b.Time)
	}
	return nil
}

func init() {
	util.RegisterType((*ListTask)(nil))
}
