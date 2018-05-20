package test

import (
	"reflect"
	"unsafe"
	"testing"
)

//go test .\test\benchmark_test.go -v -bench=".*"
// test 后面跟随制定的文件路径
// -v: 是否输出全部的单元测试用例（不管成功或者失败），默认没有加上，所以只输出失败的单元测试用例
// -bench=".*" 匹配所有的测试用例
// -benchmem 是否在性能测试的时候输出内存情况
// -blockprofile block.out : 是否输出内部goroutine阻塞的性能分析文件

// 将byte切片转换为string
func BytesToString(b []byte) string {
	bh := (*reflect.SliceHeader)(unsafe.Pointer(&b))
	sh := reflect.StringHeader{bh.Data, bh.Len}
	return  *(*string)(unsafe.Pointer(&sh))
}

//将string转换为byte切片
func StringToBytes(str string) []byte {
	sh := (*reflect.StringHeader)(unsafe.Pointer(&str))
	bh := reflect.SliceHeader{sh.Data, sh.Len, 0}
	return *(*[]byte)(unsafe.Pointer(&bh))
}


func BenchmarkBytesToStringReflect(b *testing.B){
	for i := 0; i < b.N; i++ {
		bt := []byte("nihao")
		_ = BytesToString(bt)
	}
}

func BenchmarkBytesToStringNomal(b *testing.B){
	for i := 0; i < b.N; i++ {
		bt := []byte("nihao")
		_ = string(bt)
	}
}

func TestMyFristTesting(t *testing.T) {
	t.Log("hi~")
}
