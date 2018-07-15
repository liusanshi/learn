package core

import (
	"log"
)

//条件接口
type IConditions interface {
	ITask
	GetResult() bool //获取条件执行的结果
}

// IfElse 条件任务  分支任务
type IfElse struct {
	Cond     IConditions //条件
	Body     List        //完成之后的执行body
	ElseTask Task        //与之匹配的else
}

//Init 初始化任务
func (i *IfElse) Init(data map[string]interface{}) error {
	if val, ok := data["Cond"].(map[string]interface{}); ok {
		nt, err := TaskWithMap(val)
		if err != nil {
			log.Printf("IfElse - Cond; err:%v\n", err)
			return err
		}
		if i.Cond, ok = nt.Task.(IConditions); !ok {
			log.Printf("IfElse - Cond; type err: cant convert (%T) to (%T)\n", nt.Task, IConditions(nil))
		}
	}
	if val, ok := data["ElseTask"].(map[string]interface{}); ok {
		nt, err := TaskWithMap(val)
		if err != nil {
			log.Printf("IfElse - ElseTask; err:%v\n", err)
			return err
		}
		i.ElseTask = *nt
	}
	if val, ok := data["Body"].([]interface{}); ok {
		nt, err := TaskWithList(val)
		if err != nil {
			log.Printf("IfElse - Body; err:%v\n", err)
			return err
		}
		i.Body = *nt
	}
	return nil

}

// ToMap 数据转换为map
func (i *IfElse) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Cond"] = i.Cond.ToMap()
	data["Body"] = i.Body.ToArray()
	data["ElseTask"] = i.ElseTask.ToMap()
	return data
}

//Run 任务运行
func (i *IfElse) Run(session *Session) error {
	err := i.Cond.Run(session)
	if err != nil {
		return err
	}
	if i.Cond.GetResult() {
		return i.Body.Run(session)
	}
	return i.ElseTask.Run(session)
}
