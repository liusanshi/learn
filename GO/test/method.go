package test

import (
	"sync"
	"time"
	"fmt"
	"math"

)

type Point struct {
	X, Y float64
}

type Path []Point

func Distance(p, q Point) float64 {
	return math.Hypot(q.X - p.X, q.Y - p.Y)
}

func (this Point) Distance(q Point) float64 {
	return math.Hypot(q.X - this.X, q.Y - this.Y)
}

func (this *Point)  ScaleBy(factor float64){
	this.X *= factor
	this.Y *= factor
}

func (this Path) Distance() float64 {
	sum := 0.0
	for i := range this {
		if i > 0 {
			sum += this[i - 1].Distance(this[i])
		}
	}
	return sum
}

type Values map[string][]string

func (this Values)Get(key string) string {
	if val := this[key]; len(val) > 0 {
		return val[0]
	}
	return ""
}

func (this Values) Add(key, val string) {
	this[key] = append(this[key], val)
}

var cache = struct{
	sync.RWMutex
	mapping map[string]string
}{
	mapping: make(map[string]string),
}

func Lookup(key string) string{
	cache.RLock()
	v := cache.mapping[key]
	cache.RUnlock()
	return v
}

func MethodTest(){
	p := Point{1, 2}
	q := Point{4, 6}

	q.ScaleBy(3)
	fmt.Println(q)

	fmt.Println(Distance(p, q))
	fmt.Println(p.Distance(q))
	// fmt.Println(Point.Distance(p, q))

	const day = 24 * time.Hour
	fmt.Println(day.Seconds())
	fmt.Println(day)

	path := Path{p, q, {4,5}}

	fmt.Println(path.Distance())

	var strMap Values = Values{"lang": {"en"}}
	strMap.Add("item", "1")
	strMap.Add("item", "2")

	fmt.Println(strMap.Get("item"))
	fmt.Println(strMap.Get("lang"))
	fmt.Println(strMap.Get("item1"))
	fmt.Println(strMap["item2"])

	fmt.Println("======nil========")
	strMap = nil;
	fmt.Println(strMap.Get("item"))
	fmt.Println(strMap["item"])
	strMap.Add("item", "2")
}