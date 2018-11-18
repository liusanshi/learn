package test

import (
	"fmt"
	"io/ioutil"
	"reflect"
	"runtime"
	"unsafe"
	// "time"
)

func GetFunctionName(i interface{}) string {
	fmt.Println("*********GetFunctionName*********")
	return runtime.FuncForPC(reflect.ValueOf(i).Pointer()).Name()
}

func Decorator(decoptr, fn interface{}) (err error) {
	var decoratedFunc, targetFunc reflect.Value

	decoratedFunc = reflect.ValueOf(decoptr).Elem()
	targetFunc = reflect.ValueOf(fn)

	v := reflect.MakeFunc(targetFunc.Type(), func(in []reflect.Value) (out []reflect.Value) {
		fmt.Println("before")
		out = targetFunc.Call(in)
		// time.Sleep(time.Second)
		fmt.Println("end")
		return
	})

	decoratedFunc.Set(v)

	return
}

type LocalUser struct {
	Address string
	Name    string
	Age     int
}

type MyUser LocalUser

var handler = func(u *LocalUser, message string) {
	fmt.Fprintln(ioutil.Discard, "Hello, My Name is %s, I am %d year old! so, %s\n", u.Name, u.Age, message)
}

func FiltName(u *LocalUser, messgae string) {
	fn := reflect.ValueOf(handler)
	uv := reflect.ValueOf(u)
	name := uv.Elem().FieldByName("Name")
	name.SetString("***")
	fn.Call([]reflect.Value{uv, reflect.ValueOf(messgae)})
}

var offset uintptr = 0xFFFF

func FiltNameWithOffset(u *LocalUser, messgae string) {
	if offset == 0xFFFF {
		t := reflect.TypeOf(*u) //.Elem()
		name, _ := t.FieldByName("Name")
		offset = name.Offset
	}
	up := (*[2]uintptr)(unsafe.Pointer(&u))
	upnamePtr := (*string)(unsafe.Pointer(up[0] + offset))
	*upnamePtr = "~~~"
	fn := reflect.ValueOf(handler)
	uv := reflect.ValueOf(u)
	fn.Call([]reflect.Value{uv, reflect.ValueOf(messgae)})
}

var rCache = map[uintptr]map[string]uintptr{}

func FiltNameWithCache(u *LocalUser, messgae string) {
	// itab := *(**uintptr)(unsafe.Pointer(&u))
	up := *(*[2]uintptr)(unsafe.Pointer(&u)) //这样处理才是每种类型都是固定的
	fmt.Println(up)
	m, ok := rCache[up[1]]
	if !ok {
		m = make(map[string]uintptr)
		rCache[up[1]] = m
	}

	offset, ok := m["Name"]
	if !ok {
		uv := reflect.TypeOf(u).Elem()
		field, _ := uv.FieldByName("Name")
		offset = field.Offset
		m["Name"] = offset
	}

	upPtr := (*string)(unsafe.Pointer(up[0] + offset))
	*upPtr = "###"
	fn := reflect.ValueOf(handler)
	fn.Call([]reflect.Value{reflect.ValueOf(u), reflect.ValueOf(messgae)})
}

func ReflectTesting() {
	user := &LocalUser{"", "PLM", 34}
	fmt.Println("reflect===============")
	FiltName(user, "hehe")
	fmt.Println("reflect offset========")
	FiltNameWithOffset(user, "hehe")
	fmt.Println("reflect cache========")
	FiltNameWithCache(user, "hehe")

	var inter interface{} = user
	up := *(*[2]uintptr)(unsafe.Pointer(&inter))
	fmt.Println(up)

	up = *(*[2]uintptr)(unsafe.Pointer(&user))
	fmt.Println(up)
}
