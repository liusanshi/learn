package task

import (
	"os"
	"fmt"
	"encoding/json"
	"log"
	"context"
	"io/ioutil"
	"io"
)

type TaskList []Task

func (this *TaskList) Init(data []interface{}) (error) {
	*this = make([]Task, len(data))
	for i, item := range data {
		if task, ok := item.(map[string]interface{}); ok {
			nt := Task{}
			err := nt.Init(task)
			if err != nil {
				log.Printf("TcpServerTask - subTask; err:%v\n", err)
				return err
			}
			(*this)[i] = nt
		} else {
			return fmt.Errorf("TcpServerTask TaskList subTask type error")
		}
	}
	return nil
}

func (this *TaskList) ToArray() []interface{} {
	data := make([]interface{}, 0, len(*this))
	for _, item := range *this {
		data = append(data, item.ToMap())
	}
	return data
}

func (this *TaskList) MarshalJSON() ([]byte, error) {
	return json.Marshal(*this)
}

func (this *TaskList) UnmarshalJSON(data []byte) error {
	return json.Unmarshal(data, this)
}

//任务队列
type TaskQueue struct {
	TaskList
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
	data := this.ToArray()
	return json.Marshal(data)
	// return json.Marshal(this.TaskList)
}

func (this *TaskQueue) UnmarshalJSON(data []byte) error {
	array := []interface{}{}
	err := json.Unmarshal(data, &array)
	if err != nil {
		return err
	} else {
		return this.Init(array)
	}
	// return json.Unmarshal(data, &this.TaskList)
}

//获取任务
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

//保存任务
func (this *TaskQueue) Save(filePath string) error {
	data, err := this.MarshalJSON()
	if err != nil {
		log.Fatalf("TaskQueue MarshalJSON fail; err:%v", err)
		return err
	}
	return ioutil.WriteFile(filePath, data, os.ModePerm)
}

//取消任务执行
func (this *TaskQueue) Canel() {
	this.cancel = true
	if this.cancelFunc != nil {
		this.cancelFunc()
	}
}

//执行任务
func (this *TaskQueue) Start(writer io.Writer) error {
	if this.ctx == nil {
		this.ctx, this.cancelFunc = context.WithCancel(context.Background())
	}
	result, err := this.Run(this.ctx)
	if err != nil {
		return err
	} else {
		fmt.Println(result) //输出打印到日志
		writer.Write([]byte(result)) //将数据输出到客户端
		return nil
	}
}

