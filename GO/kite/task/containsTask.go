package task

import (
	"fmt"
	"io/ioutil"
	"strings"

	"../util"
	"./core"
)

// ContainsTask 是否包含某个字符串的任务
type ContainsTask struct {
	FilePath  string
	SubString string
	result    bool
}

func init() {
	util.RegisterType((*ContainsTask)(nil))
}

//Init 数据初始化
func (c *ContainsTask) Init(data map[string]interface{}) error {
	var ok bool
	if c.FilePath, ok = data["FilePath"].(string); !ok {
		return fmt.Errorf("ContainsTask FilePath type error")
	}
	if c.SubString, ok = data["SubString"].(string); !ok {
		return fmt.Errorf("ContainsTask SubString type error")
	}
	return nil
}

//ToMap 数据转换为map
func (c *ContainsTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["FilePath"] = c.FilePath
	data["SubString"] = c.SubString
	return data
}

//Run 创建分支
func (c *ContainsTask) Run(session *core.Session) error {
	data, err := ioutil.ReadFile(c.FilePath)
	if err != nil {
		return err
	}
	c.result = strings.Contains(string(data), c.SubString)
	return nil
}

// GetResult 获取条件的执行结果
func (c *ContainsTask) GetResult() bool {
	return c.result
}
