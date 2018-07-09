package server

import (
	"context"
	"fmt"
	"os"
	"os/signal"
	"syscall"

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
	ctx, cancelFunc := context.WithCancel(context.Background())
	//监听取消信号
	go func() {
		sign := listenSysSign()
		select {
		case <-sign:
			cancelFunc()
			fmt.Println("监听到取消信号")
			os.Exit(0)
		}
	}()
	err = taskQueue.Start(ctx, os.Stdout)
	if err != nil {
		fmt.Printf("任务执行失败: %v\n", err)
		return
	}
}

// listenSysSign 监听系统退出命令
func listenSysSign() chan os.Signal {
	c := make(chan os.Signal)
	signal.Notify(c, syscall.SIGHUP, syscall.SIGINT, syscall.SIGTERM, syscall.SIGQUIT)
	return c
}
