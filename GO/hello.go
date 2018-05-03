package main

import (
	"fmt"
	"./test"
)
//import "unsafe"

func main(){
	var a = 1.5
	var b = 2
	fmt.Println("a:", a);
	fmt.Printf("pointer:%p\n", &a);
	aa := &a;
	*aa = 2.6;
	str := "asdadsasdadad";
	fmt.Printf("a:%v\n", str);
	fmt.Println("aa:", *aa);

	fmt.Println(b);

	var arr [5]int;
	fmt.Printf("arr:%v\n", arr);
	//arr = []int{1, 2, 3, 4}

	fmt.Println("Hello, World!");

	test.Array()
}

