package task

import (
	"os/exec"
	"context"
	"fmt"
	"bytes"
	"../util"
)

type ShellTask struct {
	Cmd string
	Args []string
}

func (this *ShellTask) Init(data map[string]interface{}) (error) {
	var ok bool
	this.Cmd, ok = data["Cmd"].(string)
	if !ok {
		return fmt.Errorf("ShellTask Cmd type error")
	}
	args, ok := data["Args"].([]interface{})
	if !ok {
		return fmt.Errorf("ShellTask Args type error")
	} else {
		for _, a := range args {
			this.Args = append(this.Args, a.(string))
		}
	}
	return nil
}

func (this *ShellTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Cmd"] = this.Cmd
	data["Args"] = this.Args
	return data
}

func (this *ShellTask) Run(ctx context.Context) (string, error){
	args := make([]string, len(this.Args) + 1)
	args[0] = "-c";
	copy(args[1:], this.Args)
	cmd := exec.Command(this.Cmd, args...)
	select {
	case <- ctx.Done():
		return CANCEL, nil
	default:
		break
	}
	var out bytes.Buffer
	cmd.Stdout = &out
	err := cmd.Run()
	if err != nil {
		return out.String(), err
	}	
	return out.String(), nil
}

func init(){
	util.RegisterType((*ShellTask)(nil))
}