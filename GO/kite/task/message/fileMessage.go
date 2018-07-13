package message

import (
	"bufio"
	"bytes"
	"fmt"
	"io"
	"os"
	"path/filepath"
	"strconv"

	"../../util"
)

//FileMessage 文件消息 用于上传文件
type FileMessage struct {
	Length int64
	Path   string
	file   io.ReadWriteCloser
	innerData
}

//String 将数据转换为字符串
func (f *FileMessage) String() string {
	return fmt.Sprintf("%s:%d", f.Path, f.Length)
}

//Close 关闭
func (f *FileMessage) Close() error {
	if f.file != nil {
		return f.file.Close()
	}
	return nil
}

//ReadFrom 读取数据
func (f *FileMessage) ReadFrom(r io.Reader) (int64, error) {
	nr := bufio.NewReader(r)
	head, err := nr.ReadBytes('\n')
	if err != nil {
		return 0, err
	}
	head = bytes.TrimSpace(head)
	index := bytes.IndexByte(head, '/')
	length, err := strconv.ParseInt(string(head[0:index]), 10, 0)
	if err != nil {
		return 0, err
	}
	f.Length = length
	f.Path = string(head[index+1:])
	return io.Copy(f.file, nr)
}

//WriteTo 写入数据
func (f *FileMessage) WriteTo(w io.Writer) (int64, error) {
	nw := bufio.NewWriter(w)
	_, err := nw.WriteString(fmt.Sprintf("%d/%s\n", f.Length, f.Path))
	if err != nil {
		return 0, err
	}
	return io.Copy(nw, f.file)
}

// Save 保存消息
func (f *FileMessage) Save(path string) error {
	path += f.Path
	basename := filepath.Base(path)
	//判断文件夹路径是否存在
	if !util.FileExists(basename) {
		if err := os.MkdirAll(basename, os.ModePerm); err != nil { //不存在则创建路径
			return err
		}
	}
	file, err := os.OpenFile(path, os.O_CREATE, os.ModePerm)
	if err != nil {
		return nil
	}
	_, err = io.Copy(file, f.file)
	return err
}

//NewFileMessage 文件消息
func NewFileMessage(path, dstPath string) (*FileMessage, error) {
	if !util.FileExists(path) {
		return nil, os.ErrNotExist
	}
	file, err := os.OpenFile(path, os.O_RDONLY, os.ModePerm)
	if err != nil {
		return nil, err
	}
	info, err := file.Stat()
	if err != nil {
		return nil, err
	}
	return &FileMessage{
		Length: info.Size(),
		Path:   dstPath,
		file:   file,
	}, nil
}
