package main

import (
	"fmt"
	"./test"
	// "time"
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

	fmt.Println(12);
	fmt.Println(b);

	var arr [5]int;
	fmt.Printf("arr:%v\n", arr);
	//arr = []int{1, 2, 3, 4}

	fmt.Println("Hello, World!");

	// fmt.Println(test.GetFunctionName(test.Array))

	var getFunctionName func(interface{}) string;
	test.Decorator(&getFunctionName, test.GetFunctionName)

	fmt.Println(getFunctionName(test.Array))
	
/* 	const MAX = 100000;
	fmt.Println("PrintArgs => begin")
	begin := time.Now().UnixNano()
	test.TestPrint(MAX, test.PrintArgs)
	fmt.Println("PrintArgs => end", time.Now().UnixNano() - begin)


	fmt.Println("PrintArgsStrings => begin")
	begin = time.Now().UnixNano()
	test.TestPrint(MAX, test.PrintArgsStrings)
	fmt.Println("PrintArgsStrings => end", time.Now().UnixNano() - begin)
 */
	test.FetchAll([]string{
		"www.qq.com",
		"www.baidu.com",
	});

	test.FetchAllSeq([]string{
		"www.qq.com",
		"www.baidu.com",
	});

	test.WebServer();
}


func init(){
	fmt.Println("main init ....")
}
