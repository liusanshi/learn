package task

import (
	"bytes"
	"fmt"
	"os/exec"

	"../util"
	"./core"
	"./message"
)

//ShellTask shell任务
type ShellTask struct {
	Cmd    string
	Ignore bool //忽略错误
	Args   []string
}

//Init 数据初始化
func (s *ShellTask) Init(data map[string]interface{}) error {
	var ok bool
	if s.Cmd, ok = data["Cmd"].(string); !ok {
		return fmt.Errorf("ShellTask Cmd type error")
	}
	if ignore, ok := data["Ignore"].(string); ok {
		if ignore == "1" {
			s.Ignore = true
		}
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
	if s.Ignore {
		data["Ignore"] = 1
	} else {
		data["Ignore"] = 0
	}
	return data
}

//Run 执行任务
func (s *ShellTask) Run(session *core.Session) error {
	args := make([]string, len(s.Args)+1)
	args[0] = "-c"
	copy(args[1:], s.Args)
	cmd := exec.Command(s.Cmd, args...)
	if session.IsCancel() {
		return core.ErrCANCEL
	}
	var (
		out    bytes.Buffer
		errOut bytes.Buffer
	)
	cmd.Stdout = &out
	cmd.Stderr = &errOut
	err := cmd.Run()
	if err != nil && !s.Ignore {
		return fmt.Errorf("err:%v; info:%s", err, errOut.Bytes())
	} else if err != nil { //需要忽略错误
		fmt.Printf("err:%v; info:%s\n", err, errOut.Bytes())
		session.Printf(true, message.SystemMessage, "ignore err:%v; info:%s", err, errOut.Bytes())
	}
	session.Write(out.Bytes())
	return nil
}

func init() {
	util.RegisterType((*ShellTask)(nil))
}
