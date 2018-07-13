package message

import (
	"fmt"
	"io"
	"net/url"
	"os"
	"path/filepath"
	"strconv"

	"../../util"
)

//FileMessage 文件消息 用于上传文件
type FileMessage struct {
	Length int64
	Path   string
	Branch string
	file   io.ReadWriteCloser
}

//String 将数据转换为字符串
func (f *FileMessage) String() string {
	return fmt.Sprintf("%s:%s:%d", f.Branch, f.Path, f.Length)
}

//Close 关闭资源
func (f *FileMessage) Close() error {
	if f.file != nil {
		return f.file.Close()
	}
	return nil
}

//Parse 读取数据
func (f *FileMessage) Parse(req *Request) error {
	var err error
	f.Length, err = strconv.ParseInt(req.Get("length"), 10, -1)
	if err != nil {
		return err
	}
	f.Path, err = url.PathUnescape(req.Get("path"))
	if err != nil {
		return err
	}
	f.Branch, err = url.PathUnescape(req.Get("branch"))
	if err != nil {
		return err
	}
	f.file = req.file
	return nil
}

//WriteTo 写入数据
func (f *FileMessage) WriteTo(w io.Writer) (int64, error) {
	// cmd?length=xx&path=xx
	// _, err := io.WriteString(w, fmt.Sprintf("%d/%s\n", f.Length, f.Path))
	_, err := io.WriteString(w, fmt.Sprintf("/upload?length=%d&path=%s&branch=%s\n", f.Length, url.PathEscape(f.Path), url.PathEscape(f.Branch)))
	if err != nil {
		return 0, err
	}
	return io.Copy(w, f.file)
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
		return err
	}
	_, err = io.Copy(file, f.file)
	return err
}

//NewFileMessage 文件消息
func NewFileMessage(path, dstPath, branch string) (*FileMessage, error) {
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
		Branch: branch,
		file:   file,
	}, nil
}

//readOnly 只读的流
type readOnly struct {
	read io.Reader
}

func (r *readOnly) Read(p []byte) (n int, err error) {
	return r.read.Read(p)
}

func (r *readOnly) Write(p []byte) (n int, err error) {
	return
}
func (r *readOnly) Close() error {
	return nil
}
