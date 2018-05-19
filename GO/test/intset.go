package test

import (
	// "io"
	"fmt"
	"bytes"

)

const PLAT = 32 << (^ uint(0) >> 63)

type IntSet struct {
	words []uint
}

func (this *IntSet) Has(x int) bool{
	word, bit := x / PLAT, uint(x % PLAT)
	return word < len(this.words) && (this.words[word] & (1 << bit) != 0)
}

func (this *IntSet) Add(x int){
	word, bit := x / PLAT, uint(x % PLAT)
	for word >= len(this.words) {
		this.words = append(this.words, 0)
	}
	this.words[word] |= (1 << bit)
}

func (this *IntSet) UnionWith(t *IntSet){
	for i, word := range t.words {
		if i < len(this.words){
			this.words[i] |= word
		} else {
			this.words = append(this.words, word)
		}
	}
}

func (this *IntSet) IntersectWith(t *IntSet){
	for i, word := range t.words {
		if i < len(this.words){
			this.words[i] &= word
		}
	}
}

func (this *IntSet) DifferenceWith(t *IntSet){
	for i, word := range t.words {
		if i < len(this.words){
			this.words[i] &= (^word)
		}
	}
}

func (this *IntSet) SymmetricWith(t *IntSet){
	for i, word := range t.words {
		if i < len(this.words){
			this.words[i] ^= word
		}
	}
}

func (this *IntSet) Elems() []int {
	elemens := make([]int, 0, len(this.words) * 32)
	for i, word := range this.words {
		for j := 0; j < PLAT; j++ {
			if word & (1 << uint(j)) != 0 {
				elemens = append(elemens, PLAT * i + j)
			}
		}
	}
	return elemens
}

func (this IntSet) String() string {
	var buf *bytes.Buffer = bytes.NewBuffer(make([]byte, 0, 50))
	buf.WriteString("{")
	for i, word := range this.words {
		if word == 0 {
			continue
		}
		for j := 0; j < PLAT; j++ {
			if word & (1 << uint(j)) != 0 {
				if buf.Len() > 1 {
					buf.WriteString(" ")
				}			
				fmt.Fprintf(buf, "%d", PLAT * i + j)
			}
		}
	} 
	buf.WriteString("}")
	return buf.String()
}

func (this *IntSet) Len() int {
	len := 0
	for _, word := range this.words {
		for j := 0; j < PLAT; j++ {
			if word & (1 << uint(j)) != 0 {
				len++
			}			
		}
	} 
	return len
}

func (this *IntSet) Remove(x int){
	word, bit := x / PLAT, x % PLAT
	if len(this.words) > word {
		this.words[word] &= (^(1 << uint(bit)))
	}
}

func (this *IntSet) Copy() *IntSet{
	words := make([]uint, len(this.words))

	copy(words, this.words)

	return &IntSet{words: words}
}

func (this *IntSet) Clear(){
	this.words = nil
}

func IntSetTest(){
	var x, y IntSet;
	x.Add(1)
	x.Add(144)
	x.Add(9)

	fmt.Println(x.String())
	fmt.Println(&x)

	y.Add(9)
	y.Add(42)
	fmt.Println(y.String())
	fmt.Println(y)
	
	x.UnionWith(&y)
	z := x.Copy()
	fmt.Println(x.String())
    x.Add(146)
	z.Add(147)
	fmt.Println(x.String())
	fmt.Println(z.String())

	x.DifferenceWith(z)
	fmt.Printf("DifferenceWith {1 9 42 144} => %s\n", x)

	fmt.Printf("ele:%v\n", z.Elems())
	
	fmt.Printf("x len:%d\n", x.Len())
	fmt.Printf("x has 9: %v, x has 123: %v\n", x.Has(9), x.Has(123))

	//判断平台
	fmt.Printf("plat:%v", PLAT)
}