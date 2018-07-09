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

	"../util"
)

var (
	// ErrCANCEL 取消操作的常量
	ErrCANCEL = errors.New("cancel task")
)

//ITask 任务执行器
type ITask interface {
	Run(ctx context.Context, writer io.Writer) error
	IConertToObject
	// IConertToArray
	IInit
}

//IConertToObject 转换
type IConertToObject interface {
	ToMap() map[string]interface{}
}

//IConertToArray 初始化数据的接口
type IConertToArray interface {
	ToArray() []interface{}
}

//IInit 初始化数据的接口
type IInit interface {
	Init(map[string]interface{}) error
}

//Task 任务
type Task struct {
	Type string
	Task ITask
}

const (
	//TypeKey 类型的key
	TypeKey = "__type__"
)

//Init 初始化任务
func (t *Task) Init(data map[string]interface{}) error {
	var ok bool
	if t.Type, ok = data[TypeKey].(string); !ok {
		return fmt.Errorf("Task Type type error")
	}
	if temp, ok := util.NewStructPtr(t.Type); ok {
		t.Task, ok = temp.(ITask)
		if !ok {
			log.Printf("Task - iTask UnmarshalJSON; type err:%s, need ITask;\n", t.Type)
			return fmt.Errorf("Task - iTask UnmarshalJSON; type err:%s, need ITask;", t.Type)
		}
	} else {
		log.Printf("Task - iTask NewStructPtr; type err:%s;\n", t.Type)
		return fmt.Errorf("Task - iTask NewStructPtr; type err:%s;", t.Type)
	}
	err := t.Task.Init(data)
	if err != nil {
		log.Printf("Task - iTask UnmarshalJSON; err:%v\n", err)
		return err
	}
	return nil
}

// ToMap 数据转换为map
func (t *Task) ToMap() map[string]interface{} {
	data := t.Task.ToMap()
	data[TypeKey] = t.Type
	return data
}

// MarshalJSON 数据序列化
func (t *Task) MarshalJSON() ([]byte, error) {
	dic := t.ToMap()
	data, err := json.Marshal(dic)
	if err != nil {
		log.Printf("Task MarshalJSON err:%v\n", err)
		return nil, err
	}
	return data, nil
}

//UnmarshalJSON 数据反序列化
func (t *Task) UnmarshalJSON(data []byte) error {
	dic := make(map[string]interface{})
	err := json.Unmarshal(data, &dic)
	if err != nil {
		log.Printf("Task UnmarshalJSON err:%v\n", err)
		return err
	}
	err = t.Init(dic)
	if err != nil {
		log.Printf("Task - init; origin:%s; err:%v\n", data, err)
		return err
	}
	return nil
}

//Run 任务运行
func (t *Task) Run(ctx context.Context, write io.Writer) error {
	return t.Task.Run(ctx, write)
}

//isEnd 判断是否结束
func isEnd(ctx context.Context) bool {
	select {
	case <-ctx.Done():
		return true
	default:
		return false
	}
}

//Load 加载数据
func Load(filePath string, unser json.Unmarshaler) error {
	content, err := ioutil.ReadFile(filePath)
	if err != nil {
		log.Fatalf("load fail; path:%s; err:%v", filePath, err)
		return err
	}
	err = (unser).UnmarshalJSON(content)
	if err != nil {
		log.Fatalf("resolve fail; err:%v; orign:%s", err, content)
		return err
	}
	return nil
}

//Save 保存任务
func Save(filePath string, ser json.Marshaler) error {
	data, err := (ser).MarshalJSON()
	if err != nil {
		log.Fatalf("MarshalJSON fail; err:%v", err)
		return err
	}
	return ioutil.WriteFile(filePath, data, os.ModePerm)
}
