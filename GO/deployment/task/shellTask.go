package task

import (
	// "encoding/json"
	"os/exec"
	"context"
	// "log"
	"bytes"
)

type ShellTask struct {
	Cmd string
	Args []string
}

// func (this *ShellTask) MarshalJSON() ([]byte, error) {
// 	data, err := json.Marshal(this)
// 	if err != nil {
// 		log.Printf("ShellTask MarshalJSON fail:%v\n", err)
// 		return nil, err
// 	}
// 	return data, nil
// }

// func (this *ShellTask) UnmarshalJSON(data []byte) error {
// 	err := json.Unmarshal(data, &this)
// 	if err != nil {
// 		log.Printf("ShellTask UnmarshalJSON fail:%v\n", err)
// 		return err
// 	}
// 	return nil
// }

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