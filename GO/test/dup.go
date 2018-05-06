package test

import (
	"os"
	"fmt"
	"bufio"
)

func Dup(){
	var context = make(map[string]int)

	input := bufio.NewScanner(os.Stdin)
	for input.Scan() {
		context[input.Text()]++
	}

	for line, n := range context {
		if n > 1 {
			fmt.Printf("%d\t%s\n", n , line)
		}
	}
}