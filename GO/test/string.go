package test

import (
	"bytes"
	"strings"
	"unicode/utf8"
	"fmt"
	"path/filepath"
)

func UtfString(str string){
	fmt.Printf("len:%d\n", len(str))
	fmt.Printf("char number:%d\n", utf8.RuneCountInString(str))
	for i := 0; i < len(str); {
		r, size := utf8.DecodeRuneInString(str[i:])
		fmt.Printf("%d\t%c\n", i, r)
		i += size
	}
	for i, r := range str {
		fmt.Printf("%d\t%q\t%d\n", i, r, r)
	}
}

func StrToRune(str string){
	fmt.Printf("% x \n", str)
	r := []rune(str)
	fmt.Printf("%x \n", r)
	for _, s := range r{
		fmt.Printf("str:%s; code:%x \n", string(s), s)
	}

	fmt.Println(string(1234567))
}

func BasenameCustom(path string, spe int) string{
	for i := len(path) - 1; i >= 0; i-- {
		if int(path[i]) == spe {
			path = path[i+1:]
			break
		}
	}
	for i := len(path) - 1; i >= 0; i-- {
		if path[i] == '.' {
			path = path[:i]
			break
		}
	}
	return path
}

func BasenamewithStrings(path string, spe int) string{
	index  := strings.LastIndex(path, string(rune(spe)))
	path = path[index+1:]

	if index := strings.LastIndex(path, "."); index > -1 {
		path = path[:index]
	}
	return path
}

func Basename(path string) string{
	return filepath.Base(path)
}

func Comma(s string) string{
	l := len(s)
	if l <= 3 {
		return s
	}
	const LEN = 3
	var str *bytes.Buffer = bytes.NewBufferString("")
	index, length := l % LEN, l / LEN
	str.WriteString(s[:index])
	s = s[index:]
	for i := 0; i < length; i++ {
		if index == 0 && i == 0 {
			fmt.Fprintf(str, "%s", s[i * LEN : (i+1)*LEN])
		} else {
			fmt.Fprintf(str, ",%s", s[i * LEN : (i+1)*LEN])
		}
	}
	return str.String()
	/* var i int
	for i = l - 3; i >= 0; i -= 3 {
		str = "," + s[i:i+3] + str
	}
	if i + 3 > 0 {
		str = s[:i + 3] + str
	} else {
		str = str[1:]
	}
	return str */
}

func CommaDecimal(str string) string {
	index := strings.Index(str, ".")
	return Comma(str[:index]) + str[index:]
}

func IntsToString(list []int)string{
	var buff bytes.Buffer
	buff.WriteString("[")
	if len(list) > 0 {
		fmt.Fprintf(&buff, "%d", list[0])
		for _, i := range list[1:] {
			fmt.Fprintf(&buff, ", %d", i)
		}
	}
	buff.WriteString("]")
	return buff.String()
}