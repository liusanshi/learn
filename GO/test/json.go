package test

import (
	"encoding/json"
	"fmt"
)

type Movie struct {
	Title string
	Year int `json:"released"`
	Color bool `json:"color,omitempty"`
	Actors []string
}

var movies = []Movie{
	{Title: "天龙八部", Year: 1998, Color: true, Actors: []string{"陈浩民", "黄日华", "樊少皇"}},
	{Title: "倚天屠龙记", Year: 1968, Color: false, Actors: []string{"演员1", "演员2", "演员3"}},
	{Title: "射雕英雄传", Year: 1985, Color: true, Actors: []string{"翁美玲", "黄日华", "三哥"}},
}

func JsonMarshal(){

	// data, err := json.Marshal(movies)
	data, err := json.MarshalIndent(movies, "", "	")
	if err != nil {
		fmt.Println("json marshling failed:", err)
	}
	fmt.Printf("%s\n", data)
}

var strVovies = `[
	{"Title":"天龙八部","released":1998,"color":true,"Actors":["陈浩民","黄日华","樊少皇"]},
	{"Title":"倚天屠龙记","released":1968,"Actors":["演员1","演员2","演员3"]},
	{"title":"射雕英雄传","released":1985,"color":true,"Actors":["翁美玲","黄日华","三哥"]}
	]`

func JsonUnmarshal(){
	var movies []Movie;
	err := json.Unmarshal([]byte(strVovies), &movies)
	if err != nil {
		fmt.Println("json unmarshling failed:", err)
	}
	fmt.Println(movies)
}