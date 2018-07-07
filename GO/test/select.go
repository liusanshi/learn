package test

import "fmt"

func Select() {
	var c1, c2, c3 chan int
	var i1, i2 int

	for {
		select {
		case i1 = <-c1:
			fmt.Println("received ", i1, " from c1")
		case c2 <- i2:
			fmt.Println("send ", i2, " to c2")
		case i3, ok := <-c3:
			if ok {
				fmt.Println("received ", i3, " from c3")
			} else {
				fmt.Println("c3 is closed")
			}
		default:
			fmt.Println("no communication")
			break
		}
	}
}
