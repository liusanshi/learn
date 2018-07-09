package task

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"io/ioutil"
	"log"
	"os"
)

//TaskQueue 任务队列
type TaskQueue struct {
	List
	ctx        context.Context
	cancel     bool
	cancelFunc context.CancelFunc
}

var (
	// CANCEL 取消操作的常量
	CANCEL = errors.New("cancel task")
)

//Run 执行任务
func (q *TaskQueue) Run(ctx context.Context, w io.Writer) error {
	return q.List.Run(ctx, w)
}

//MarshalJSON 序列化
func (q *TaskQueue) MarshalJSON() ([]byte, error) {
	data := q.ToArray()
	return json.Marshal(data)
	// return json.Marshal(q.List)
}

//UnmarshalJSON 反序列化
func (q *TaskQueue) UnmarshalJSON(data []byte) error {
	array := []interface{}{}
	err := json.Unmarshal(data, &array)
	if err != nil {
		return err
	}
	return q.Init(array)
	// return json.Unmarshal(data, &q.List)
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
func (q *TaskQueue) Start(ctx context.Context, writer io.Writer) error {
	if q.ctx == nil {
		if ctx == nil {
			q.ctx, q.cancelFunc = context.WithCancel(context.Background())
		} else {
			q.ctx, q.cancelFunc = context.WithCancel(ctx)
		}
	}

	if writer == nil {
		writer = os.Stdout //输出到控制台
	}
	err := q.Run(q.ctx, writer)
	if err != nil {
		fmt.Fprintf(writer, "err:%v\n", err)
		return err
	}
	return nil
}
