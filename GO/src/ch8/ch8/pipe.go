package ch8

import (
	"fmt"
)

func PipeLine() {
	naturals := make(chan int)
	squares := make(chan int)

	go counter(naturals)

	go squarer(naturals, squares)

	printer(squares)

	// var a myType = 1
	// var i interface{} = &a
	// fmt.Println(i.(*myType).Add(2))
}

func counter(out chan<- int) {
	for x := 1; x <= 100; x++ {
		out <- x
	}
	close(out)
}

func squarer(in <-chan int, out chan<- int) {
	for x := range in {
		out <- x * x
	}
	close(out)
}
func printer(in <-chan int) {
	for x := range in {
		fmt.Println(x)
	}
}

type myType int

func (this myType) Add(i int) int {
	return int(this) + i
}
