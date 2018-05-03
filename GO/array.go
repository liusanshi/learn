package main

import (
	"fmt"
)

func main(){
	arr1 := []int{0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

	fmt.Printf("arr1 = %p\n", &arr1)
	fmt.Printf("arr1 = %d\n", &arr1)

	Arr1 := append(arr1, 10)

	fmt.Printf("Arr1 = %p\n", &Arr1)
	fmt.Printf("Arr1 = %d\n", &Arr1)

	arr2 := arr1[1:3];

	fmt.Printf("arr2 = %x\n", arr2)

	arr3 := append(arr2, 15)

	fmt.Printf("arr3 = %x\n", arr3)
}