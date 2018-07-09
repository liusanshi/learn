package task

import (
	"bytes"
	"context"
	"fmt"
	"io"
	"os/exec"

	"../util"
)

//ShellTask shell任务
type ShellTask struct {
	Cmd  string
	Args []string
}

//Init 数据初始化
func (s *ShellTask) Init(data map[string]interface{}) error {
	var ok bool
	s.Cmd, ok = data["Cmd"].(string)
	if !ok {
		return fmt.Errorf("ShellTask Cmd type error")
	}
	args, ok := data["Args"].([]interface{})
	if !ok {
		return fmt.Errorf("ShellTask Args type error")
	}
	for _, a := range args {
		s.Args = append(s.Args, a.(string))
	}
	return nil
}

//ToMap 数据转换为map
func (s *ShellTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Cmd"] = s.Cmd
	data["Args"] = s.Args
	return data
}

//Run 执行任务
func (s *ShellTask) Run(ctx context.Context, w io.Writer) error {
	args := make([]string, len(s.Args)+1)
	args[0] = "-c"
	copy(args[1:], s.Args)
	cmd := exec.Command(s.Cmd, args...)
	if isEnd(ctx) {
		return ErrCANCEL
	}
	var out bytes.Buffer
	cmd.Stdout = &out
	err := cmd.Run()
	if err != nil {
		return fmt.Errorf("err:%v; info:%s", err, out.Bytes())
	}
	w.Write(out.Bytes())
	return nil
}

func init() {
	util.RegisterType((*ShellTask)(nil))
}
