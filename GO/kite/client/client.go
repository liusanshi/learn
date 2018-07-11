package client

import (
	"context"
	"fmt"
	"io"
	"os"
	"strings"

	_ "../task" //只加载不执行
	"../task/core"
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
	taskMap := core.NewMap()
	err := core.Load(cfgPath, &taskMap)
	if err != nil {
		fmt.Printf("任务加载失败: %v\n", err)
		return
	}
	params := strings.Split(cmd, " ")
	taskList, ok := taskMap[params[0]]
	if !ok {
		fmt.Printf("任务:%v不存在\n", params[0])
		return
	}
	session := core.NewSession(context.Background(), "root", os.Stdout, nil)
	if len(params) > 1 {
		session.SetCurrentBranch(params[1])
	}
	err = taskList.Run(session)
	if err != nil && err != io.EOF {
		fmt.Printf("任务执行失败: %v\n", err)
		return
	}
}
