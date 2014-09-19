#coding=utf-8

import urllib.request
import os
from os import path

from DB import MSSQL
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.common.exceptions import NoSuchElementException

driver = webdriver.PhantomJS('E:\\phantomjs-1.9.7-windows\\phantomjs.exe')
imgpath = 'D:\\img'

####################################################certificate####################################################
def get_all_certificate(id, url):
#获取所有的证书数据
    data = get_certificate(url)
    return list(map(lambda s:[id] + s, data))

def get_certificate(url):
#获取荣誉证书
    driver.get(url + 'certificate/')
    els = driver.find_elements_by_css_selector('.credit-list > li')
    result = []
    for el in els:
        name = el.find_element_by_tag_name('h4').text
        imgurl = el.find_element_by_css_selector('img.lazy').get_attribute('data-org-src')
        #http://img3.jc001.cn/img/125/1657125/1408/1453f3ed16d9995_s.jpg
        index = imgurl.find('_s')
        if index >= 0: imgurl = imgurl[:index] + imgurl[index+2:]
        imgurl = down_img(imgurl) #下载图片

        result.append([name,imgurl])        
    return result

def create_certificate(ms, info):
#将数据保存至数据库
	sql = ''
	para = []
	for inf in info:
		sql += 'insert into [certificate] ([pageid],[name],[imgpath]) values (%d,%s,%s); update NetAddress set cfstatus = 2 where id=%d; '
		para += inf + [inf[0]]
	ms.ExecNonQueryWithParam(sql, tuple(para))
    #ms.ExecNonQueryWithParam('insert into [certificate] ([pageid],[name],[imgpath]) values (%d,%s,%s); update NetAddress set cfstatus = 1 where id=%d', tuple(list(info) + [info[0]]))

def get_certificate_data(ms):
	while True:
		resList = ms.ExecQuery('select top 1000 id,url from NetAddress where cfstatus = 1 order by id')
		if len(resList) == 0:break
		for (id, url) in resList:
			info = get_all_certificate(id, url) #获取所有的荣誉
			print(url)
			#print(info)
			create_certificate(ms, info) #保存信息
####################################################certificate####################################################

####################################################case####################################################
def get_case(url):
#获取所有的成功案例的页面地址
	driver.get(url + 'case/')
	els = driver.find_elements_by_css_selector('.case-list > li')
	result = []
	for el in els:
		name = el.find_element_by_tag_name('h4').text
		pageurl = el.find_element_by_tag_name('a').get_attribute('href')
		result.append([name, pageurl])
	return result

def get_all_case(id, url):
#获取所有的成功案例页面的数据
	data = get_case(url)
	return list(map(lambda s:[id] + s, data))

def ceate_case(ms, info):
#将所有成功案例的数据保存到数据库
	sql = ''
	para = []
	for inf in info:
		sql += 'insert into [case] ([pageid],[name],[url]) values (%d,%s,%s); update NetAddress set casestatus = 2 where id=%d; '
		para += inf + [inf[0]]
	ms.ExecNonQueryWithParam(sql, tuple(para))

def get_case_data(ms):
#获取所有需要处理的成功案例的数据
	while True:
		resList = ms.ExecQuery("select top 1000 id,url from NetAddress where casestatus = 1 order by id")
		if len(resList) == 0 : break
		for (id, url) in resList:
			info = get_all_case(id, url) #获取所有的荣誉
			print(url)
			#print(info)
			ceate_case(ms, info) #保存信息
####################################################case####################################################

####################################################case_detail####################################################
def get_case_detail(url):
	#获取所有的成功案例的页面地址
	driver.get(url)
	els = driver.find_elements_by_css_selector('.cnt div.cnt img')
	result = []
	for el in els:
		imgurl = el.get_attribute('src')
		imgurl = down_img(imgurl) #下载图片
		result.append(imgurl)
	return result

def get_all_case_detail(id, url):
#获取所有的成功案例详细页面的数据
	data = get_case_detail(url)
	if len(data) > 0:
		return list(map(lambda s:[id, s], data))
	else:
		return [[id, '', id]]

def ceate_case_detail(ms, info):
#将所有成功案例详细的数据保存到数据库
	sql = ''
	para = []
	for inf in info:
		if len(inf[1]) > 0:
			sql += 'insert into [casedetail] ([caseid],[imgpath]) values (%d,%s); '
			para += inf
		sql += 'update [case] set status = 1 where id=%d; '
		para += [inf[0]]
	ms.ExecNonQueryWithParam(sql, tuple(para))

def get_case_detail_data(ms):
#获取所有需要处理的成功案例的详细列表
	while True:
		#[case]
		resList = ms.ExecQuery("select top 1000 id,url from [case] where status = 0 order by id")
		if len(resList) == 0 : break
		for (id, url) in resList:
			info = get_all_case_detail(id, url) #获取所有的成功案例详细
			print(url)
			#print(info)
			ceate_case_detail(ms, info) #保存信息

####################################################case_detail####################################################

def down_img(url):
#urllib.request.urlretrieve('http://img2.jc001.cn/img/617/1417617/1301/1350ff4d1d5984e.png', 'D:\\11.png')
#下载图片
    
    parturls = url.split('/')
    path = imgpath + '\\' + '\\'.join(parturls[4:-1])
    create_path(path)
    urllib.request.urlretrieve(url, path + '\\' + parturls[-1])
    return '\\'.join(parturls[4:])

def create_path(part_path):
#创建文件夹
    if not path.exists(part_path) :
        ps = path.split(part_path)
        if len(ps) > 1 :
            #print(ps)
            create_path(ps[0])
        os.mkdir(part_path)

def main():
## ms = MSSQL(host="localhost",user="sa",pwd="123456",db="PythonWeiboStatistics")
## #返回的是一个包含tuple的list，list的元素是记录行，tuple的元素是每行记录的字段
## ms.ExecNonQuery("insert into WeiBoUser values('2','3')")

    ms = MSSQL(host="localhost",user="sa",pwd="sa",db="JZData")
    get_certificate_data(ms)
    get_case_data(ms)
    get_case_detail_data(ms)

if __name__ == '__main__':
	main()