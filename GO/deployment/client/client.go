package client

import (
	"fmt"
	"../task"
	"../util"
	"os"
	"log"
)

func Command(cmd string){

}

func TestSaveTask(cmd string){
	taskQueue := task.TaskQueue{
		TaskList: []task.Task{
			task.Task{ 
				Type: "CurlTask", 
				Task: &task.CurlTask{ 
					Url:"http://www.qq.com",
				},
			},
			task.Task{ 
				Type: "ShellTask", 
				Task: &task.ShellTask{ 
					Cmd: "/usr/bin/bash", 
					Args: []string{ "echo hi" }, 
					},
			},
		},
	}
	path := util.GetCurrentPath() + "/task.json"
	if !util.FileExists(path) {
		_, err := os.Create(path)
		if err != nil {
			log.Fatalf("配置文件创建失败：%v", err)
		}
	}
	err := taskQueue.Save(path)
	if err != nil {
		fmt.Println(err)
	}
}