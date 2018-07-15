package core

import (
	"log"

	"../../util"
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
	ElseTask List        //与之匹配的else
}

func init() {
	util.RegisterType((*IfElse)(nil))
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
	if val, ok := data["ElseTask"].([]interface{}); ok {
		nt, err := TaskWithList(val)
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
	data["ElseTask"] = i.ElseTask.ToArray()
	return data
}

//Run 任务运行
func (i *IfElse) Run(session *Session) error {
	err := i.Cond.Run(session)
	if err != nil {
		return err
	}
	if i.Cond.GetResult() {
		if i.Body != nil && len(i.Body) > 0 {
			return i.Body.Run(session)
		}
		return nil
	}
	if i.ElseTask != nil && len(i.ElseTask) > 0 {
		return i.ElseTask.Run(session)
	}
	return nil
}
