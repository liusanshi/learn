package client

import (
	"context"
	"fmt"
	"os"

	"../task"
	"../util"
)

//Client 执行命令
func Client(path string) {
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
	err = taskMap.Run(context.Background(), os.Stdout)
	if err != nil {
		fmt.Printf("任务执行失败: %v\n", err)
		return
	}
}
