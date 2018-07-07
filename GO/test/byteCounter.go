package test

import (
	"fmt"
)

type ByteCounter int

func (this *ByteCounter) Write(p []byte) (int, error) {
	*this += ByteCounter(len(p))
	return len(p), nil
}

func ByteCounterTest() {
	var bc ByteCounter
	fmt.Fprint(&bc, []byte{1, 2, 3, 4})
	fmt.Println(bc)
}
