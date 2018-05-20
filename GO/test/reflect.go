package test

import (
	"io/ioutil"
	"unsafe"
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

type LocalUser struct {
	Address string
	Name string
	Age int
}

var handler = func(u *LocalUser, message string) {
	fmt.Fprintln(ioutil.Discard,"Hello, My Name is %s, I am %d year old! so, %s\n", u.Name, u.Age, message)
}

func FiltName(u *LocalUser, messgae string){
	fn := reflect.ValueOf(handler)
	uv := reflect.ValueOf(u)
	name := uv.Elem().FieldByName("Name")
	name.SetString("***")
	fn.Call([]reflect.Value{uv, reflect.ValueOf(messgae)})
}

var offset uintptr = 0xFFFF
func FiltNameWithOffset(u *LocalUser, messgae string){
	if offset == 0xFFFF {
		t := reflect.TypeOf(*u)//.Elem()
		name, _ := t.FieldByName("Name")
		offset = name.Offset
	}
	up := (*[2]uintptr)(unsafe.Pointer(&u))
	upnamePtr := (*string)(unsafe.Pointer(up[0]+offset))
	*upnamePtr = "~~~"
	fn := reflect.ValueOf(handler)
	uv := reflect.ValueOf(u)
	fn.Call([]reflect.Value{uv, reflect.ValueOf(messgae)})
}


func ReflectTesting(){
	fmt.Println("reflect===============")
	FiltName(&LocalUser{"",  "PLM", 34 }, "hehe")
	fmt.Println("reflect offset========")
	FiltNameWithOffset(&LocalUser{ "",  "PLM", 34 }, "hehe")
	
}

