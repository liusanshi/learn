package test

import (
	"fmt"
)

func Array() {
	arr1 := []int{0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

	fmt.Printf("arr1 = %p\n", &arr1)
	fmt.Printf("arr1 = %d\n", &arr1)

	Arr1 := append(arr1, 10)

	fmt.Printf("Arr1 = %p\n", &Arr1)
	fmt.Printf("Arr1 = %d\n", &Arr1)

	arr2 := arr1[1:3]

	fmt.Printf("arr2 = %x\n", arr2)

	arr3 := append(arr2, 15)

	fmt.Printf("arr3 = %x\n", arr3)

	Arr2 := make([]int, 2, 3)
	fmt.Println(Arr2[1])
	// Arr2[2] = 1 //不能这样扩展，只能append
	// fmt.Println(Arr2[2])
	Arr3 := AppendInt(Arr2, 1, 2, 3, 4, 5, 6, 7, 8, 9)
	fmt.Println(Arr3)

	fmt.Printf("Arr2 addr:%p\n", Arr2)
	fmt.Printf("Arr3 addr:%p\n", Arr3)
	fmt.Printf("Arr3 cap:%d\n", cap(Arr3))

	Arr3 = AppendInt(Arr3, 2)
	fmt.Println(Arr3)

	fmt.Printf("Arr2 addr:%p\n", Arr2)
	fmt.Printf("Arr3 addr:%p\n", Arr3)
	fmt.Printf("Arr3 cap:%d\n", cap(Arr3))

	_ = Remove(Arr3, 5)
	fmt.Println(Arr3)
	fmt.Printf("Arr3 addr:%p\n", Arr3)
	fmt.Printf("Arr3 cap:%d\n", cap(Arr3))
}

func init() {
	fmt.Println("test/array/init...")
}

func Reverse(arr []int) {
	for i, j := 0, len(arr)-1; i < j; i, j = i+1, j-1 {
		arr[i], arr[j] = arr[j], arr[i]
	}
}

func Move(arr []int, index int) {
	if index >= len(arr) || index < 0 {
		panic("index 参数错误!")
	}
	Reverse(arr[:index])
	Reverse(arr[index:])
	Reverse(arr)
}

func Move2(arr []int, index int) {
	if index >= len(arr) || index < 0 {
		panic("index 参数错误!")
	}
	var list = make([]int, index, index)
	copy(list, arr[:index])
	copy(arr[:], arr[index:])
	copy(arr[len(arr)-index:], list)
}

func Move3(arr []int, index int) {
	var temp []int
	s := 0
	for i := 0; i < len(arr); i++ {
		if i < index {
			temp = append(temp, arr[i])
		}
		if i+index < len(arr) {
			arr[i] = arr[i+index]
		} else {
			if s == 0 {
				s = i
			}
			arr[i] = temp[i-s]
		}
	}
}

func AppendInt(arr []int, i ...int) []int {
	var res []int
	l, c, lin := len(arr)+len(i), cap(arr), len(i)
	if l <= c {
		res = arr[:l]
	} else {
		if c < 2*l {
			c = 2 * l
		}
		res = make([]int, l, c)
		copy(res, arr)
	}
	copy(res[l-lin:], i)
	return res
}

func Remove(arr []int, index int) []int {
	copy(arr[index:], arr[index+1:])
	return arr[:len(arr)-1]
}
