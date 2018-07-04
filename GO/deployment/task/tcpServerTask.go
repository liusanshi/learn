package task

import (
	// "net"
	"context"
	"fmt"
	"log"
	"../util"
)

type TcpServerTask struct {
	Port string
	TaskDict map[string]TaskList
}

func (this *TcpServerTask) Init(data map[string]interface{}) (error) {
	var ok bool
	if this.Port, ok = data["Port"].(string); !ok {
		return fmt.Errorf("TcpServerTask Port type error")
	}
	this.TaskDict = make(map[string]TaskList)
	if taskList, ok := data["TaskDict"].(map[string]interface{}); ok {
		for key, val := range taskList {
			if list, ok := val.([]interface{}); ok {
				taskListItem := TaskList{}
				err := taskListItem.Init(list)
				if err != nil {
					log.Printf("TcpServerTask - TaskList; err:%v\n", err)
						return err
				}
				this.TaskDict[key] = taskListItem
			} else {
				return fmt.Errorf("TcpServerTask TaskDict type error")
			}
		}
		return nil
	} else {
		return fmt.Errorf("TcpServerTask Port type error")
	}
}

func (this *TcpServerTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Port"] = this.Port
	dict := make(map[string][]interface{})
	for key, val := range this.TaskDict {
		dict[key] = val.ToArray()
	}
	data["TaskDict"] = dict
	return data
}

func (this *TcpServerTask) Run(ctx context.Context) (string, error){
	return "", nil
}

func init(){
	util.RegisterType((*TcpServerTask)(nil))
}