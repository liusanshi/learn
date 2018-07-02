package config

import (
	"fmt"
	"strings"
	"path/filepath"
	"os"
	"encoding/json"
	"log"
	"io/ioutil"
)


func List(key string){
	config := NewConfig()
	config.Load()
	if len(key) == 0 {
		fmt.Println(config.Data)
	} else {
		val, ok := config.GetValue(key)
		if !ok {
			fmt.Printf("配置%s:不存在\n", key)
		}
		fmt.Println(val)
	}
}

func Set(ket, val string) {
	config := NewConfig()
	config.Load()
	config.SetValue(ket, val)
	config.Store()
}

//配置文件的结构体
type Config struct{
	LocalPath string
	Data map[string]string
}

func NewConfig() *Config {
	dir, err := filepath.Abs(filepath.Dir(os.Args[0]))
	if err != nil {
		log.Fatal(err)
	}
	path := strings.Replace(dir, "\\", "/", -1) + "/config.json"
	_, err = os.Stat(path)
	if err != nil {
		if os.IsNotExist(err) {
			_, err = os.Create(path)
			if err != nil {
				log.Fatalf("配置文件创建失败：%v", err)
			}
		}
	}
	return &Config{
		LocalPath: path,
		Data: make(map[string]string),
	}
}

func (this *Config) SetServerHost(host string){
	this.SetValue("ServerHost", host)
}
func (this *Config) GetServerHost() (string, bool) {
	return this.GetValue("ServerHost")
}
func (this *Config) SetPort(port string){
	this.SetValue("Port", port)
}
func (this *Config) GetPort() (string, bool) {
	return this.GetValue("Port")
}

func (this *Config) SetWorkPath(path string){
	this.SetValue("SetWorkPath", path)
}
func (this *Config) GetWorkPath() (string, bool) {
	return this.GetValue("SetWorkPath")
}

func (this *Config) GetValue(key string) (string, bool) {
	val, ok := this.Data[key]
	return val, ok
}

func (this *Config) SetValue(key, val string) {
	this.Data[key] = val
}

func (this *Config) Store() error {
	data, err := json.Marshal(this.Data)
	if err != nil {
		log.Fatalf("配置文件序列化失败：%v", err)
		return err
	}
	err = ioutil.WriteFile(this.LocalPath, data, os.ModePerm)
	if err != nil {
		log.Fatalf("配置文件保存失败：%v", err)
	}
	return err
}

func (this *Config) Load() error {
	content, err := ioutil.ReadFile(this.LocalPath)
	if err != nil {
		log.Fatalf("配置文件:%s 加载失败:\n%v", this.LocalPath, err)
		return err
	}
	if len(content) > 0 {
		err = json.Unmarshal(content, &this.Data)
		if err != nil {
			log.Fatalf("配置文件解析失败: %v", err)
			return err
		}
	}
	return nil;
}