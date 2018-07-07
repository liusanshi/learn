package test

import (
	"fmt"
	"os"
	"time"
)

func Testchanel() {
	ch := make(chan int, 100)

	go func() {
		for i := 0; i < 10; i++ {
			ch <- i
			// println(time.Second)
			time.Sleep(time.Second)
		}

		os.Exit(0)
	}()

	for {
		select {
		case n := <-ch:
			println(n)
		case <-timeAfter(time.Second * 2):

		}
	}
}

func timeAfter(d time.Duration) chan int {
	q := make(chan int, 1)
	time.AfterFunc(d, func() {
		q <- 1
		fmt.Println("run")
	})
	return q
}
