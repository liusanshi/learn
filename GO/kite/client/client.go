package client

import (
	"context"
	"fmt"
	"io"
	"os"

	"../task"
	"../util"
)

//Client 执行命令
func Client(path, cmd string) {
	if len(path) == 0 {
		path = util.GetCurrentPath()
	}
	cfgPath := path + "/task_client.json"
	if !util.FileExists(cfgPath) {
		fmt.Printf("配置文件:%s 不存在\n", cfgPath)
		return
	}
	taskMap := task.NewMap()
	err := task.Load(cfgPath, &taskMap)
	if err != nil {
		fmt.Printf("任务加载失败: %v\n", err)
		return
	}
	taskList, ok := taskMap[cmd]
	if !ok {
		fmt.Printf("任务:%v不存在\n", cmd)
		return
	}
	session := task.NewSession(context.Background(), "root", os.Stdout, nil)
	err = taskList.Run(session)
	if err != nil && err != io.EOF {
		fmt.Printf("任务执行失败: %v\n", err)
		return
	}
}
