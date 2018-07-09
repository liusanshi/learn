package unit

import (
	"fmt"
	"os"
	"testing"

	"../task"
	"../util"
)

//测试任务的序列化
func TestSaveTask(t *testing.T) {
	taskQueue := task.List{
		task.Task{
			Type: "CurlTask",
			Task: &task.CurlTask{
				URL:    "http://www.qq.com",
				Method: task.POST,
				Head:   map[string]string{"content-type": "html/text", "a": "1"},
			},
		},
		task.Task{
			Type: "ShellTask",
			Task: &task.ShellTask{
				Cmd:  "/usr/bin/bash",
				Args: []string{"echo hi", "echo hi-1"},
			},
		},
		task.Task{
			Type: "TCPServerTask",
			Task: &task.TCPServerTask{
				Port: "80",
				TaskDict: map[string]task.List{
					"list": {
						task.Task{
							Type: "CurlTask",
							Task: &task.CurlTask{
								URL:    "http://www.qq.com",
								Method: task.POST,
								Head:   map[string]string{"content-type": "html/text", "a": "1"},
							},
						},
						task.Task{
							Type: "ShellTask",
							Task: &task.ShellTask{
								Cmd:  "/usr/bin/bash",
								Args: []string{"echo hi", "echo hi-1"},
							},
						},
					},
					"modify": {
						task.Task{
							Type: "ShellTask",
							Task: &task.ShellTask{
								Cmd:  "/usr/bin/bash",
								Args: []string{"echo hi", "echo hi-1"},
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
	err := task.Save("E:\\git\\learn\\GO\\kite\\task.json", &taskQueue)
	if err != nil {
		t.Log(err)
	}
}

//测试任务的反序列化
func TestLoadTask(t *testing.T) {
	// path := util.GetCurrentPath() + "/task.json"
	// if !util.FileExists(path) {
	// 	_, err := os.Create(path)
	// 	if err != nil {
	// 		t.Logf("配置文件创建失败：%v", err)
	// 		return
	// 	}
	// }
	taskQueue := task.NewList()
	err := task.Load("E:\\git\\learn\\GO\\kite\\task.json", &taskQueue)
	if err != nil {
		t.Log(err)
	}
	temp, _ := taskQueue.MarshalJSON()
	fmt.Printf("%s\n", temp)
	t.Logf("%s\n", temp)
}

func TestLoadClientTask(t *testing.T) {
	taskQueue := task.NewMap()
	err := task.Load("E:\\git\\learn\\GO\\kite\\task_client.json", &taskQueue)
	if err != nil {
		t.Log(err)
	}
	temp, _ := taskQueue.MarshalJSON()
	fmt.Printf("%s\n", temp)
	t.Logf("%s\n", temp)
}
