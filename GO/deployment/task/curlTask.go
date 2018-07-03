package task

import (
	"io/ioutil"
	"fmt"
	"strings"
	"log"
	// "encoding/json"
	"context"
	"net/http"
	"../util"
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

func (this *CurlTask) Init(data map[string]interface{}) (error) {
	var ok bool
	this.Url, ok = data["Url"].(string)
	if !ok {
		return fmt.Errorf("CurlTask Url type error")
	}
	this.Param, ok = data["Param"].(string)
	if !ok {
		return fmt.Errorf("CurlTask Param type error")
	}
	method := data["Method"].(float64)
	if !ok {
		return fmt.Errorf("CurlTask CurlMethod type error")
	} else {
		this.Method = CurlMethod(int(method))
	}
	if data["Head"] != nil {
		this.Head = make(map[string]string)
		if head, ok := data["Head"].(map[string]interface{}); ok {
			for k, v := range head {
				this.Head[k] = v.(string)
			}
		} else {
			return fmt.Errorf("CurlTask Head type error")
		}
	}	
	return nil
}

func (this *CurlTask) ToMap() map[string]interface{} {
	data := make(map[string]interface{})
	data["Url"] = this.Url
	data["Param"] = this.Param
	data["Method"] = this.Method
	data["Head"] = this.Head
	return data
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

func init(){
	util.RegisterType((*CurlTask)(nil))
}