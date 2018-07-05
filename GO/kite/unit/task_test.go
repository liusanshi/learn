package unit

import (
	"testing"
	"os"
	"../util"
	"../task"
)

//测试任务的序列化
func TestSaveTask(t *testing.T){
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
			task.Task{ 
				Type: "TcpServerTask", 
				Task: &task.TcpServerTask{ 
					Port: "80",
					TaskDict: map[string]task.TaskList{
						"list": {
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
						"modify": {
							task.Task{ 
								Type: "ShellTask", 
								Task: &task.ShellTask{ 
									Cmd: "/usr/bin/bash", 
									Args: []string{ "echo hi", "echo hi-1" }, 
									},
							},
						},
					},
				},
			},
		},
	}
	path := util.GetCurrentPath() + "/task.json"
	if !util.FileExists(path) {
		_, err := os.Create(path)
		if err != nil {
			t.Logf("配置文件创建失败：%v", err)
			return
		}
	}
	err := taskQueue.Save("E:\\git\\learn\\GO\\deployment\\task.json")
	if err != nil {
		t.Log(err)
	}
}


//测试任务的反序列化
func TestLoadTask(t *testing.T){
	// path := util.GetCurrentPath() + "/task.json"
	// if !util.FileExists(path) {
	// 	_, err := os.Create(path)
	// 	if err != nil {
	// 		t.Logf("配置文件创建失败：%v", err)
	// 		return
	// 	}
	// }
	taskQueue := task.TaskQueue{}
	err := taskQueue.Load("E:\\git\\learn\\GO\\deployment\\task.json")
	if err != nil {
		t.Log(err)
	}
	temp, _ := taskQueue.MarshalJSON()
	t.Logf("%s\n", temp)
}