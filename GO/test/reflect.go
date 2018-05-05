package test

import (
	"fmt"
	"reflect"
	"runtime"
	// "time"
)


func GetFunctionName(i interface{}) string{
	fmt.Println("*********GetFunctionName*********")
	return runtime.FuncForPC(reflect.ValueOf(i).Pointer()).Name();
}

func Decorator(decoptr, fn interface{}) (err error){
	var decoratedFunc, targetFunc reflect.Value;

	decoratedFunc = reflect.ValueOf(decoptr).Elem()
	targetFunc = reflect.ValueOf(fn)

	v := reflect.MakeFunc(targetFunc.Type(), func(in []reflect.Value) (out []reflect.Value){
		fmt.Println("before")
		out = targetFunc.Call(in)
		// time.Sleep(time.Second)
		fmt.Println("end")
		return
	})
	
	decoratedFunc.Set(v);

	return;
}