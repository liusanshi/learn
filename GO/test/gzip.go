package test

import (
	"bytes"
	"compress/gzip"
	"fmt"
	"io"
	"log"
	"os"
	"time"
)

func TestComp() {
	var buf bytes.Buffer
	zw := gzip.NewWriter(&buf)

	//设置标题字段是可选的
	zw.Name = ""
	zw.Comment = ""
	zw.ModTime = time.Date(2018, 11, 1, 0, 0, 0, 0, time.UTC)

	_, err := zw.Write([]byte("你好我十一个中文"))
	if err != nil {
		log.Fatal(err)
	}

	if err := zw.Close(); nil != err {
		log.Fatal(err)
	}

	zr, err := gzip.NewReader(&buf)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("Name: %s\nComment: %s\nModTime: %s\n\n", zr.Name, zr.Comment, zr.ModTime.UTC())

	if _, err := io.Copy(os.Stdout, zr); err != nil {
		log.Fatal(err)
	}

	if err := zr.Close(); err != nil {
		log.Fatal(err)
	}
}
