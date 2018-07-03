package task

import (
	"log"
	"encoding/json"
	"context"
)

//任务执行器
type ITask interface {
	Run(ctx context.Context) (string, error)
}

type Task struct {
	Type string
	Task ITask
}

func (this *Task) MarshalJSON() ([]byte, error) {
	dic := make(map[string]string)
	dic["type"] = this.Type
	task, err := json.Marshal(this.Task)
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
	err = json.Unmarshal([]byte(dic["task"]), &this.Task)
	if err != nil {
		log.Printf("Task - iTask UnmarshalJSON err:%v\n", err)
		return err
	}
	return nil
}

func (this *Task) Run(ctx context.Context) (string, error) {
	return this.Task.Run(ctx)
}