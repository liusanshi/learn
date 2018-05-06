package test

import (
	"os"
	"fmt"
	"strings"
)

func TestPrint(count int, fun func([]string)){
	args := os.Args[1:]
	var i int
	for i < count {
		fun(args);
		i++
	}
}

func PrintArgs(args []string){
	var s, sep string
	for _, i := range args {
		s +=  sep + i;
		sep = " "
	}	
	// fmt.Println(s);
}

func PrintArgsStrings(args []string){
	//fmt.Println(strings.Join(args, " "))
	_ = strings.Join(args, " ");
}

func PrintArgsSystem(args []string){
	fmt.Println(args)
}

func init(){
	fmt.Println("test/args/init...")
}