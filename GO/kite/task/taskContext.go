package task

import (
	"encoding/json"
	"io/ioutil"
	"log"
	"os"
)

// Branch 分支信息
type Branch struct {
	// Name 分支名称
	Name string `json:"name"`
	// Path 路径
	Path string `json:"path"`
	// Version 版本号
	Version int `json:"version"`
	// Time 最后更新时间
	Time string `json:"time"`
}

// Context 任务的上下文
type Context struct {
	// data 临时数据
	data map[string]interface{}
	// List 分支信息
	List []*Branch
	// Path 保存路径
	Path string
}

// GetVal 获取数据
func (c *Context) GetVal(key string) (interface{}, bool) {
	data, ok := c.data[key]
	return data, ok
}

// SetVal 设置值
func (c *Context) SetVal(key string, val interface{}) {
	c.data[key] = val
}

// Save 保存配置
func (c *Context) Save() error {
	data, err := json.Marshal(c.List)
	if err != nil {
		log.Fatalf("配置文件序列化失败：%v", err)
		return err
	}
	err = ioutil.WriteFile(c.Path, data, os.ModePerm)
	if err != nil {
		log.Fatalf("配置文件保存失败：%v", err)
	}
	return err
}

// Load 加载配置
func (c *Context) Load() error {
	content, err := ioutil.ReadFile(c.Path)
	if err != nil {
		log.Fatalf("配置文件:%s 加载失败:\n%v", c.Path, err)
		return err
	}
	if len(content) > 0 {
		err = json.Unmarshal(content, &c.List)
		if err != nil {
			log.Fatalf("配置文件解析失败: %v", err)
			return err
		}
	}
	c.data = make(map[string]interface{})
	return nil
}

// GetBranch 获取分支
func (c *Context) GetBranch(name string) (*Branch, bool) {
	for _, item := range c.List {
		if item.Name == name {
			return item, true
		}
	}
	return nil, false
}

//DelBranch 删除分支
func (c *Context) DelBranch(name string) {
	list := make([]*Branch, 0, len(c.List)-1)
	for _, item := range c.List {
		if item.Name != name {
			list = append(list, item)
		}
	}
	c.List = list
}

//NewContext 创建一个任务的上下文
func NewContext(path string) *Context {
	ctx := &Context{Path: path}
	err := ctx.Load()
	if err != nil {
		return nil
	}
	return ctx
}
