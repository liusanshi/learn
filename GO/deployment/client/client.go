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

func TestLoadTask(cmd string){
	path := util.GetCurrentPath() + "/task.json"
	if !util.FileExists(path) {
		_, err := os.Create(path)
		if err != nil {
			log.Fatalf("配置文件创建失败：%v", err)
		}
	}
	taskQueue := task.TaskQueue{}
	err := taskQueue.Load(path)
	if err != nil {
		fmt.Println(err)
	}
	temp,_ := taskQueue.MarshalJSON()
	fmt.Printf("%s\n", temp)
}

func TestSaveTask(cmd string){
	taskQueue := task.TaskQueue{
		TaskList: []task.Task{
			task.Task{ 
				Type: "CurlTask", 
				Task: &task.CurlTask{ 
					Url:"http://www.qq.com",
					Method: task.POST,
					Head: map[string]string{"content-type": "html/text", "a":"1"},
				},
			},
			task.Task{ 
				Type: "ShellTask", 
				Task: &task.ShellTask{ 
					Cmd: "/usr/bin/bash", 
					Args: []string{ "echo hi", "echo hi-1" }, 
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