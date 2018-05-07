package test

import (
	"fmt"
)

//逃逸分析影响数据的结果。千万小心
//逃逸成功的数据存放堆
//没有逃逸的数据存放栈
func Escapes(){
	s := []byte("")
	fmt.Println(cap(s), len(s))//14行注释之后输出：32 0；取消注释之后输出0 0

	s1 := append(s, 'a')
	s2 := append(s, 'b')

	fmt.Println(s1, ",", s2)//输出：[97] [98]；
	fmt.Println(string(s1), ",", string(s2)) //14行注释之后输出：b , b；取消注释之后输出a , b
}