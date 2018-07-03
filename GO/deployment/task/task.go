package task

import (
	"log"
	"encoding/json"
	"context"
)

//任务执行器
type IRun interface {
	Run(ctx context.Context) (string, error)
}

//任务接口
type ITask interface {
	IRun
	json.Marshaler
	json.Unmarshaler
}

type Task struct {
	Type string
	Task ITask
}

func (this *Task) MarshalJSON() ([]byte, error) {
	dic := make(map[string]string)
	dic["type"] = this.Type
	task, err := this.Task.MarshalJSON()
	if err != nil {
		log.Printf("Task subTask MarshalJSON err:%v\n", err)
		return nil, err
	}
	dic["task"] = string(task)
	data, err := json.Marshal(dic)
	if err != nil {
		log.Printf("Task MarshalJSON err:%v\n", err)
		return nil, err
	}
	return data, nil
}

func (this *Task) UnmarshalJSON(data []byte) error {
	dic := make(map[string]string)
	err := json.Unmarshal(data, &dic)
	if err != nil {
		log.Printf("Task UnmarshalJSON err:%v\n", err)
		return err
	}
	this.Type = dic["type"]
	err = this.Task.UnmarshalJSON([]byte(dic["task"]))
	if err != nil {
		log.Printf("Task - iTask UnmarshalJSON err:%v\n", err)
		return err
	}
	return nil
}

func (this *Task) Run(ctx context.Context) (string, error) {
	return this.Task.Run(ctx)
}