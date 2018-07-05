package task

import (
	"log"
	"encoding/json"
	"context"
	"../util"
	"fmt"
)

//任务执行器
type ITask interface {
	Run(ctx context.Context) (string, error)
	IConert
}

//转换
type IConert interface {
	Init(map[string]interface{}) error
	ToMap() map[string]interface{}
}

type Task struct {
	Type string
	Task ITask
}

const (
	TypeKey = "__type__" //类型的key
)

func (this *Task) Init(data map[string]interface{}) (error) {
	var ok bool
	if this.Type, ok = data[TypeKey].(string); !ok {
		return fmt.Errorf("Task Type type error")
	}
	if temp, ok := util.NewStructPtr(this.Type); ok {
		this.Task, ok = temp.(ITask)
		if !ok {
			log.Printf("Task - iTask UnmarshalJSON; type err:%s, need ITask;\n", this.Type)
			return fmt.Errorf("Task - iTask UnmarshalJSON; type err:%s, need ITask;\n", this.Type)
		}
	} else {
		log.Printf("Task - iTask NewStructPtr; type err:%s;\n", this.Type)
		return fmt.Errorf("Task - iTask NewStructPtr; type err:%s;\n", this.Type)
	}
	err := this.Task.Init(data)
	if err != nil {
		log.Printf("Task - iTask UnmarshalJSON; err:%v\n", err)
		return err
	}
	return nil
}

func (this *Task) ToMap() map[string]interface{} {
	data := this.Task.ToMap()
	data[TypeKey] = this.Type
	return data
}

func (this *Task) MarshalJSON() ([]byte, error) {
	dic := this.ToMap()
	data, err := json.Marshal(dic)
	if err != nil {
		log.Printf("Task MarshalJSON err:%v\n", err)
		return nil, err
	}
	return data, nil
}

func (this *Task) UnmarshalJSON(data []byte) error {
	dic := make(map[string]interface{})
	err := json.Unmarshal(data, &dic)
	if err != nil {
		log.Printf("Task UnmarshalJSON err:%v\n", err)
		return err
	}
	err = this.Init(dic)
	if err != nil {
		log.Printf("Task - init; origin:%s; err:%v\n", data, err)
		return err
	}
	return nil
}

func (this *Task) Run(ctx context.Context) (string, error) {
	return this.Task.Run(ctx)
}

//判断是否结束
func isEnd(ctx context.Context) bool{
	select {
	case <- ctx.Done():
		return true
	default:
		return false
	}
}