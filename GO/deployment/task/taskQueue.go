package task

import (
	"fmt"
	"encoding/json"
	"log"
	"context"
	"io/ioutil"
)

//任务队列
type TaskQueue struct {
	TaskList []Task
	ctx context.Context
	cancel bool
	cancelFunc context.CancelFunc
}

const (
	CANCEL string = "cancel"
)

func (this *TaskQueue) Run(ctx context.Context) (string, error) {
	for _, task := range this.TaskList {
		if this.cancel {
			return CANCEL, nil
		}
		res, err := task.Run(ctx)
		if err != nil {
			log.Printf("Task:%s Run fail err:%v;\n", task.Type, err)
			return res, err
		}
	}
	return "", nil
}

func (this *TaskQueue) MarshalJSON() ([]byte, error) {
	return json.Marshal(this.TaskList)
}

func (this *TaskQueue) UnmarshalJSON(data []byte) error {
	return json.Unmarshal(data, &this.TaskList)
}

func (this *TaskQueue) Load(filePath string) error {
	content, err := ioutil.ReadFile(filePath)
	if err != nil {
		log.Fatalf("TaskQueue load fail; path:%s; err:%v", filePath, err)
		return err
	}
	err = this.UnmarshalJSON(content)
	if err != nil {
		log.Fatalf("TaskQueue resolve fail; err:%v", err)
		return err
	}
	return nil
}

//取消任务执行
func (this *TaskQueue) Canel() {
	this.cancel = true
	if this.cancelFunc != nil {
		this.cancelFunc()
	}
}

//执行任务
func (this *TaskQueue) Start() error {
	if this.ctx == nil {
		this.ctx, this.cancelFunc = context.WithCancel(context.Background())
	}
	result, err := this.Run(this.ctx)
	if err != nil {
		return err
	} else {
		fmt.Println(result)
		return nil
	}
}

