package ch8

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

func Parall() {
	files := make(chan string)
	go func() {
		files <- "1"
		files <- "2"
		files <- "3"
		files <- "4"
		close(files)
	}()
	fmt.Println(work(files))
}

func work(files <-chan string) int {
	var group sync.WaitGroup
	size := make(chan int)
	r := rand.New(rand.NewSource(time.Now().UnixNano()))
	for file := range files {
		group.Add(1)
		file := file
		go func() {
			var rnd = r.Intn(50)
			time.Sleep(time.Duration(rnd) * time.Microsecond)
			fmt.Printf("file:%s;size:%d\n", file, rnd)
			size <- rnd
			group.Done()
		}()
	}

	go func() {
		group.Wait()
		close(size)
	}()

	sum := 0
	for s := range size {
		sum += s
	}
	return sum
}
