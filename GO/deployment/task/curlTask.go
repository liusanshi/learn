package task

import (
	"io/ioutil"
	"fmt"
	"strings"
	"log"
	"encoding/json"
	"context"
	"net/http"
)

type CurlMethod int

const (
	GET = CurlMethod(iota)
	POST = CurlMethod(iota)
)

//curl任务
type CurlTask struct {
	Url string
	Param string
	Method CurlMethod
	Head map[string]string
}

func (this *CurlTask) MarshalJSON() ([]byte, error) {
	data, err := json.Marshal(this)
	if err != nil {
		log.Printf("CurlTask MarshalJSON fail:%v\n", err)
		return nil, err
	}
	return data, nil
}

func (this *CurlTask) UnmarshalJSON(data []byte) error {
	err := json.Unmarshal(data, &this)
	if err != nil {
		log.Printf("CurlTask UnmarshalJSON fail:%v\n", err)
		return err
	}
	return nil
}

func (this *CurlTask) Run(ctx context.Context) (string, error){
	var method string = "GET"
	var url string = this.Url
	var body string = this.Param
	if this.Method == POST {
		method = "POST"
	} else if len(this.Param) > 0 {
		if strings.Index(this.Param, "?") > -1 {
			url += "&" + this.Param
		} else {
			url += "?" + this.Param
		}
		body = ""
	}
	request, err := http.NewRequest(method, url, strings.NewReader(body))
	if err != nil {
		log.Printf("Curl NewRequest fail:%v\n", err)
		return "", err
	}
	if len(this.Head) > 0 {
		for k, v := range this.Head {
			request.Header.Add(k, v)
		}
	}
	select {
	case <- ctx.Done():
		return CANCEL, nil
	default:
		break
	}
	request = request.WithContext(ctx)
	resp, err := http.DefaultClient.Do(request)
	if err != nil {
		log.Printf("Curl request fail:%v\n", err)
		return "", err
	}
	defer resp.Body.Close()
	if resp.StatusCode != http.StatusOK {
		log.Printf("get url err code: %d\n", resp.StatusCode)
		return "", fmt.Errorf("get url err code: %d\n", resp.StatusCode)
	}
	result, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		log.Printf("Curl result resolve fail:%v\n", err)
		return "", err
	}
	return string(result), nil
}