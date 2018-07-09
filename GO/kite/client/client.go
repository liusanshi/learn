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
		path = util.GetCurrentPath() + "/task.json"
	}
	if !util.FileExists(path) {
		fmt.Printf("配置文件:%s 不存在\n", path)
		return
	}
	taskMap := task.NewMap()
	err := task.Load(path, &taskMap)
	if err != nil {
		fmt.Printf("任务加载失败: %v\n", err)
		return
	}
	taskList, ok := taskMap[cmd]
	if !ok {
		fmt.Printf("任务:%v不存在\n", cmd)
		return
	}

	err = taskList.Run(context.Background(), os.Stdout)
	if err != nil && err != io.EOF {
		fmt.Printf("任务执行失败: %v\n", err)
		return
	}
}
