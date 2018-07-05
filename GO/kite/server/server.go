package server

import (
	"fmt"

	"../task"
	"../util"
)

// Sev 服务入口
func Sev(path string) {
	if len(path) == 0 {
		path = util.GetCurrentPath() + "/task.json"
	}
	if !util.FileExists(path) {
		fmt.Printf("配置文件:%s 不存在\n", path)
		return
	}
	taskQueue := task.TaskQueue{}
	err := taskQueue.Load(path)
	if err != nil {
		fmt.Printf("任务加载失败: %v\n", err)
		return
	}
	err = taskQueue.Start(nil)
	if err != nil {
		fmt.Printf("任务执行失败: %v\n", err)
		return
	}
}
