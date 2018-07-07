package test

import (
	"fmt"
)

type Buffer struct {
	buf     []byte
	initial [64]byte
}

func (this *Buffer) Len() int {
	return len(this.buf)
}

func (this *Buffer) Grow(x int) {
	if this.buf == nil {
		this.buf = this.initial[:0]
	}
	if len(this.buf)+x > cap(this.buf) {
		buf := make([]byte, this.Len(), 2*cap(this.buf)+x)
		copy(buf, this.buf)
		this.buf = buf
	}
}

func BufferTest() {
	buf := Buffer{}
	fmt.Printf("init:%v\n", buf)
	buf.Grow(10)
	fmt.Printf("grow1:%v\n", buf)
	buf.Grow(100)
	fmt.Printf("grow2:%v\n", buf)

}
