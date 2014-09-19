#codeing=utf-8
# 下载chinamenwang 数据

import requests
from bs4 import BeautifulSoup

host = 'http://www.chinamenwang.com'	#主机域名
index_url = 'http://www.chinamenwang.com/company/' #下载数据的首页
encoding = 'gb2312'
html_save_path = "d:\\html\\chinamenwang"

def download_category():
	"下载分类数据"
	resp = requests.get(index_url)
	resp.encoding = encoding
	save_html('index.html', resp.text) #将网页保存起来
	soup = BeautifulSoup(resp.text)
	data = []
	for cate in soup.select('#sideMain1 > dl'):
		data.append(get_category(cate))

	return data

def get_category(cate):
	"获取分类信息"
	result = None
	cate0 = cate.select('dt > a')
	if cate0:
		cate0 = cate0[0]
		result = a_to_data(cate0)
		#下面是子分类
		cate_child = cate.select('dd > a')
		if cate_child:
			cc_arr = result['child'] = []
			for cc in cate_child:
				cc_arr.append(a_to_data(cc))
	return result

def a_to_data(a):
	"将A标签的属性转换为数据"
	return {
		'url' : host + a.attrs['href'],
		'title' : a.text
	}

def save_html(name, text):
	"保存网页文件"
	path = html_save_path + '\\' + name
	with open(path, 'wb') as fp:
		fp.write(text.encode(encoding))

def main():
	print (download_category())

if __name__ == '__main__':
	main()