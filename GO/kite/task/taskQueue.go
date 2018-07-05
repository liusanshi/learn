package task

import (
	"context"
	"encoding/json"
	"fmt"
	"io"
	"io/ioutil"
	"log"
	"os"
)

// TaskList 是一个Task任务的列表
type TaskList []Task

// Init 根据数据初始化
func (l *TaskList) Init(data []interface{}) error {
	*l = make([]Task, len(data))
	for i, item := range data {
		if task, ok := item.(map[string]interface{}); ok {
			nt := Task{}
			err := nt.Init(task)
			if err != nil {
				log.Printf("TaskList - subTask; err:%v\n", err)
				return err
			}
			(*l)[i] = nt
		} else {
			return fmt.Errorf("TaskList TaskList subTask type error")
		}
	}
	return nil
}

//ToArray 将列表数据序列化为[]interface{}
func (l *TaskList) ToArray() []interface{} {
	data := make([]interface{}, 0, len(*l))
	for _, item := range *l {
		data = append(data, item.ToMap())
	}
	return data
}

// MarshalJSON 序列化接口
func (l *TaskList) MarshalJSON() ([]byte, error) {
	return json.Marshal(*l)
}

// UnmarshalJSON 反序列化
func (l *TaskList) UnmarshalJSON(data []byte) error {
	return json.Unmarshal(data, l)
}

//TaskQueue 任务队列
type TaskQueue struct {
	TaskList
	ctx        context.Context
	cancel     bool
	cancelFunc context.CancelFunc
}

const (
	// CANCEL 取消操作的常量
	CANCEL = "cancel"
)

//Run 执行任务
func (q *TaskQueue) Run(ctx context.Context) (string, error) {
	for _, task := range q.TaskList {
		if q.cancel {
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

//MarshalJSON 序列化
func (q *TaskQueue) MarshalJSON() ([]byte, error) {
	data := q.ToArray()
	return json.Marshal(data)
	// return json.Marshal(q.TaskList)
}

//UnmarshalJSON 反序列化
func (q *TaskQueue) UnmarshalJSON(data []byte) error {
	array := []interface{}{}
	err := json.Unmarshal(data, &array)
	if err != nil {
		return err
	}
	return q.Init(array)
	// return json.Unmarshal(data, &q.TaskList)
}

//Load 获取任务
func (q *TaskQueue) Load(filePath string) error {
	content, err := ioutil.ReadFile(filePath)
	if err != nil {
		log.Fatalf("TaskQueue load fail; path:%s; err:%v", filePath, err)
		return err
	}
	err = q.UnmarshalJSON(content)
	if err != nil {
		log.Fatalf("TaskQueue resolve fail; err:%v", err)
		return err
	}
	return nil
}

//Save 保存任务
func (q *TaskQueue) Save(filePath string) error {
	data, err := q.MarshalJSON()
	if err != nil {
		log.Fatalf("TaskQueue MarshalJSON fail; err:%v", err)
		return err
	}
	return ioutil.WriteFile(filePath, data, os.ModePerm)
}

//Canel 取消任务执行
func (q *TaskQueue) Canel() {
	q.cancel = true
	if q.cancelFunc != nil {
		q.cancelFunc()
	}
}

// Start 执行任务
func (q *TaskQueue) Start(writer io.Writer) error {
	if q.ctx == nil {
		q.ctx, q.cancelFunc = context.WithCancel(context.Background())
	}
	result, err := q.Run(q.ctx)
	if err != nil {
		return err
	}
	fmt.Println(result) //输出打印到日志
	if writer != nil {
		writer.Write([]byte(result)) //将数据输出到客户端
	}
	return nil
}
