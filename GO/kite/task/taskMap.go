package task

import (
	"context"
	"fmt"
	"io"
	"log"
)

//Map 任务字典
type Map map[string]List

// Init 根据数据初始化
func (m *Map) Init(data map[string]interface{}) error {
	*m = NewMap()
	for i, item := range data {
		if task, ok := item.([]interface{}); ok {
			nt := List{}
			err := nt.Init(task)
			if err != nil {
				log.Printf("Map - subTask; err:%v\n", err)
				return err
			}
			(*m)[i] = nt
		} else {
			return fmt.Errorf("Map List subTask type error")
		}
	}
	return nil
}

//ToMap 将列表数据序列化为[]interface{}
func (m *Map) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	for key, val := range *m {
		data[key] = val.ToArray()
	}
	return data
}

//Run 任务运行
func (m *Map) Run(ctx context.Context, write io.Writer) error {
	for _, val := range *m {
		if isEnd(ctx) {
			return ErrCANCEL
		}
		if err := val.Run(ctx, write); err != nil {
			return err
		}
	}
	return nil
}

// NewMap 创建一个Map
func NewMap() Map {
	return Map(make(map[string]List))
}
