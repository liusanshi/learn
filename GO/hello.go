package main

import (
	"os"
	"fmt"
	"time"
)
//import "unsafe"

func main(){
	var a = 1.5
	var b = 2
	fmt.Println("a:", a);
	fmt.Printf("pointer:%P\n", &a);
	aa := &a;
	*aa = 2.6;
	str := "asdadsasdadad";
	fmt.Printf("a:%P\n", str);
	fmt.Println("aa:", *aa);

	fmt.Println(b);

	var arr [5]int;
	fmt.Printf("arr:%t\n", arr);
	//arr = []int{1, 2, 3, 4}

	fmt.Println("Hello, World!");


	ch := make(chan int, 100);

	go func(){
		for i := 0; i < 10; i++ {
			ch <- i;
			// println(time.Second)
			time.Sleep(time.Second);
		}

		os.Exit(0)
	}()

	for{
		select{
			case n := <- ch:
				println(n);
			case <- timeAfter(time.Second*2):

		}
	}
}

func timeAfter(d time.Duration) chan int{
	q := make(chan int, 1)
	time.AfterFunc(d, func(){
		q <- 1
		println("run");
	})
	return q;
}