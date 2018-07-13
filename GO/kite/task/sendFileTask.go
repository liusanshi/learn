package task

import (
	"context"
	"fmt"
	"io"
	"net"
	"os"
	"path/filepath"
	"strings"
	"sync"

	"../util"
	"./core"
	"./message"
)

const (
	//maxUpload 最大上传线程
	maxUpload = 4
)

//SendFileTask 发送文件的任务
type SendFileTask struct {
	//Path 本地路径
	Path string
	//DstPath 目标路径
	DstPath string
	//IP ip
	IP string
	//Port 端口
	Port string
	//排除文件
	Exclude []string
}

func init() {
	util.RegisterType((*SendFileTask)(nil))
}

//Init 数据初始化
func (s *SendFileTask) Init(data map[string]interface{}) error {
	var ok bool
	if s.Path, ok = data["Path"].(string); !ok {
		return fmt.Errorf("SendFileTask Path type error")
	}
	if s.DstPath, ok = data["DstPath"].(string); !ok {
		return fmt.Errorf("SendFileTask DstPath type error")
	}
	if s.IP, ok = data["IP"].(string); !ok {
		return fmt.Errorf("SendFileTask IP type error")
	}
	if s.Port, ok = data["Port"].(string); !ok {
		return fmt.Errorf("SendFileTask Port type error")
	}
	exclude, _ := data["Exclude"].(string)
	s.Exclude = strings.Split(exclude, " ")
	return nil
}

//ToMap 数据转换为map
func (s *SendFileTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["IP"] = s.IP
	data["Port"] = s.Port
	data["Path"] = s.Path
	data["DstPath"] = s.DstPath
	data["Exclude"] = strings.Join(s.Exclude, " ")
	return data
}

//Run 执行任务
func (s *SendFileTask) Run(session *core.Session) error {
	wait := new(sync.WaitGroup)
	filepipe := s.newUploader(session, wait)
	err := filepath.Walk(s.Path, func(path string, f os.FileInfo, err error) error {
		if f == nil {
			return err
		}
		if f.IsDir() {
			if f.Mode()&os.ModeSymlink == os.ModeSymlink { //过滤掉link文件
				return filepath.SkipDir
			}
			return nil
		}
		if f.Mode()&os.ModeSymlink == os.ModeSymlink { //过滤掉link文件
			return filepath.SkipDir
		}
		//排除不需要的文件
		for _, ex := range s.Exclude {
			if strings.Index(path, ex) > -1 {
				return filepath.SkipDir
			}
		}
		filepipe <- path
		return nil
	})
	close(filepipe)
	wait.Wait() //等待上传完成
	return err
}

//创建上传器
func (s *SendFileTask) newUploader(session *core.Session, wait *sync.WaitGroup) chan<- string {
	var filepipe = make(chan string, maxUpload)
	wait.Add(maxUpload)
	ctx, cancel := context.WithCancel(session.Ctx)
	for i := 0; i < maxUpload; i++ {
		go func() {
			defer wait.Done()
			for file := range filepipe {
				conn, err := net.Dial("tcp", s.IP+":"+s.Port)
				if err != nil {
					cancel()
					conn.Close()
					return
				}
				if isEnd(ctx) {
					conn.Close()
					return
				}
				msg, err := message.NewFileMessage(file, s.Path, s.DstPath, session.Branch)
				if err != nil {
					cancel()
					msg.Close()
					conn.Close()
					return
				}
				io.WriteString(session, "begin upload:"+file+"\n")
				_, err = msg.WriteTo(conn)
				msg.Close()
				conn.Close()
				if err != nil {
					cancel()
					return
				}
			}
		}()
	}
	return filepipe
}

//isEnd 是否结束
func isEnd(ctx context.Context) bool {
	select {
	case <-ctx.Done():
		return true
	default:
		return false
	}
}
