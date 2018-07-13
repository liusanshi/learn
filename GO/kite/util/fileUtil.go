package util

import (
	"log"
	"os"
	"path/filepath"
	"strings"
)

const isDev = false

//getCurrentPath 获取当前运行的路径
func getCurrentPath() string {
	dir, err := filepath.Abs(filepath.Dir(os.Args[0]))
	if err != nil {
		log.Fatal(err)
		return ""
	}
	return strings.Replace(dir, "\\", "/", -1)
}

//GetCurrentPath 获取当前运行的路径
func GetCurrentPath() string {
	if isDev {
		return "E:\\git\\learn\\GO\\kite"
	}
	return getCurrentPath()
}

//FileExists 判断文件路径是否存在
func FileExists(path string) bool {
	_, err := os.Stat(path)
	if err != nil {
		if os.IsNotExist(err) {
			return false
		}
	}
	return true
}

//IsDir 判断所给路径是否为文件夹
func IsDir(path string) bool {
	s, err := os.Stat(path)
	if err != nil {
		return false
	}
	return s.IsDir()
}

//IsFile 判断所给路径是否为文件
func IsFile(path string) bool {
	return !IsDir(path)
}

//IndexOf 查询字符串在字符串数组的位置
func IndexOf(list []string, sub string) int {
	for i, s := range list {
		if s == sub {
			return i
		}
	}
	return -1
}
