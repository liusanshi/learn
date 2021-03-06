package main

import (
	"fmt"

	"./test"
	// "time"
)

//import "unsafe"

func main() {
	var a = 1.5
	var b = 2
	fmt.Println("a:", a)
	fmt.Printf("pointer:%p\n", &a)
	aa := &a
	*aa = 2.6
	str := "asdadsasdadad"
	fmt.Printf("a:%v\n", str)
	fmt.Println("aa:", *aa)

	fmt.Println(12)
	fmt.Println(b)

	var arr [5]int
	fmt.Printf("arr:%v\n", arr)
	//arr = []int{1, 2, 3, 4}

	fmt.Println("Hello, World!")

	// fmt.Println(test.GetFunctionName(test.Array))

	var getFunctionName func(interface{}) string
	test.Decorator(&getFunctionName, test.GetFunctionName)

	fmt.Println(getFunctionName(test.Array))

	test.TestComp()

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
		// "www.baidu.com",
	})

	test.FetchAllSeq([]string{
		"www.qq.com",
		// "www.baidu.com",
	})

	fmt.Println(test.CommaDecimal("123146.122323"))
	fmt.Println(test.IntsToString([]int{1, 2, 3, 4, 5, 6, 7, 8, 9}))
	// fmt.Println("basename:", test.Basename("/a/b/你好acn.x.s."))
	// test.StrToRune("hello 世界!")
	// test.WebServer();

	// test.Escapes();

	Arr := []int{1, 2, 3, 4, 5, 6, 8, 23}
	test.Reverse(Arr)
	fmt.Println(Arr)

	// Arr = []int{1,2,3,4,5,6}
	test.Move3(Arr, 2)
	fmt.Println(Arr)

	test.Sort(Arr)
	fmt.Println(Arr)
	// test.Array()

	// test.PrintSearchIssues([]string{"repo:golang/go", "is:open", "json", "decoder"})

	// fmt.Println(test.CountWordAndImage("https://github.com/liusanshi"))

	test.ReflectTesting()
}

func init() {
	fmt.Println("main init ....")
}
