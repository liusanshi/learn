package task

import (
	"context"
	"fmt"
	"io"
	"log"
)

// List 是一个Task任务的列表
type List []Task

// Init 根据数据初始化
func (l *List) Init(data []interface{}) error {
	*l = make([]Task, len(data))
	for i, item := range data {
		if task, ok := item.(map[string]interface{}); ok {
			nt := Task{}
			err := nt.Init(task)
			if err != nil {
				log.Printf("List - subTask; err:%v\n", err)
				return err
			}
			(*l)[i] = nt
		} else {
			return fmt.Errorf("List List subTask type error")
		}
	}
	return nil
}

//Run 任务运行
func (l *List) Run(ctx context.Context, write io.Writer) error {
	for _, item := range *l {
		if isEnd(ctx) {
			return ErrCANCEL
		}
		if err := item.Run(ctx, write); err != nil {
			return err
		}
	}
	return nil
}

//ToArray 将列表数据序列化为[]interface{}
func (l *List) ToArray() []interface{} {
	data := make([]interface{}, 0, len(*l))
	for _, item := range *l {
		data = append(data, item.ToMap())
	}
	return data
}
