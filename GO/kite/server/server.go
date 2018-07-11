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

const isDev = false

func getCurrentPath() string {
	if isDev {
		return "E:\\git\\learn\\GO\\kite"
	} else {
		return util.GetCurrentPath()
	}
}

// Sev 服务入口
func Sev(path, work string) {
	if len(path) == 0 {
		path = getCurrentPath()
	}
	cfgPath := path + "/task.json"
	if !util.FileExists(cfgPath) {
		fmt.Printf("配置文件:%s 不存在\n", cfgPath)
		return
	}
	taskList := task.NewList()
	err := task.Load(cfgPath, &taskList)
	if err != nil {
		fmt.Printf("任务加载失败: %v\n", err)
		return
	}
	branchMan := task.NewBranchManager(path + "/config.json")
	if branchMan == nil {
		return
	}
	ctx, cancelFunc := context.WithCancel(context.Background())
	session := task.NewSession(ctx, "root", os.Stdout, branchMan)
	session.SetWorkSpace(work)
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
	err = taskList.Run(session)
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
