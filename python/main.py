#coding=utf-8

import urllib.request
import os
from os import path

from DB import MSSQL
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.common.exceptions import NoSuchElementException


driver = None
imgpath = 'D:\\img'
logpath = 'd:\\log\\log.txt'
ms = None
PAGE_TYPE = {'about':1,'contact':2,'certificate':3,'case':4, 'casedetail':5}

def get_company_info(id, title, url):
    """
##获取公司信息
    """
    jianjie, product, logo, has_certificate, has_case,register_money, established_time, register_area, corporation = get_company_about(id,url)
    if logo:
        logo = down_img(logo)
    else :
        logo = ''
    
    contact = get_contact(id,url)
    person = contact.get('联系人')
    telephone = contact.get('联系电话')
    mobile = contact.get('联系手机')
    fax = contact.get('传真号码')
    email = contact.get('联系邮箱')
    netaddress = contact.get('商家网址')
    compaddress = contact.get('公司地址')
    zipcode = contact.get('邮政编码')
    other = contact.get('其它联系方式')

    if person is None:person =''
    if telephone is None:telephone =''
    if mobile is None:mobile =''
    if fax is None:fax =''
    if email is None:email =''
    if netaddress is None:netaddress =''
    if compaddress is None:compaddress =''
    if zipcode is None:zipcode =''
    if other is None:other =''
    
    return (id, jianjie, product, logo,telephone
            ,mobile,fax,email,netaddress,compaddress,zipcode,other,0,person
            , register_money, established_time, register_area, corporation
            , has_certificate, has_case)

def get_company_about(id,url):
    """
##获取公司简介
##返回简介信息
    """
    url += 'about/'
    driver.get(url)
    els = driver.find_elements_by_css_selector('#about > p')
    jianjie = ''
    logo = ''
    product = ''
    has_certificate = 0
    has_case = 0
    register_money = ''#注册资本
    established_time = ''#成立时间
    register_area = ''#注册地
    corporation = '' #法人
    
    
    for el in els:
        jianjie += el.text

    try:
        pel = driver.find_element_by_css_selector('.shopName > p')
        product = pel.text[5:]
    except NoSuchElementException:
        pass

    try:
        lel = driver.find_element_by_css_selector('.shopLogo > a > img')
        logo = lel.get_attribute('src')
    except  NoSuchElementException:
        pass

    try:
        driver.find_element_by_id('nav_certificate')
        has_certificate = 1
    except NoSuchElementException:
        has_certificate = 0
        pass

    try:
        driver.find_element_by_id('nav_case')
        has_case = 1
    except NoSuchElementException:
        has_case = 0
        pass

    #基本信息
    try:
        jbel = driver.find_element_by_css_selector('div.text-danger')
        if jbel.text.find('基本信息') > -1:
            tbel = driver.find_element_by_css_selector('div.text-danger + table')
            register_money = tbel.find_element_by_css_selector('tr:nth-child(1) > td:nth-child(2)').text
            established_time = tbel.find_element_by_css_selector('tr:nth-child(1) > td:nth-child(4)').text
            register_area = tbel.find_element_by_css_selector('tr:nth-child(2) > td:nth-child(2)').text
            corporation = tbel.find_element_by_css_selector('tr:nth-child(2) > td:nth-child(4)').text
    except NoSuchElementException:
        pass

    save_page_source(id,PAGE_TYPE['about'],url,driver.page_source)#保存页面源文件
    
    #driver.close()
    return (jianjie, product, logo, has_certificate, has_case,
            register_money, established_time, register_area, corporation)


def get_contact(id,url):
    """
## 获取联系方式
    """
    url += 'contact.html'
    driver.get(url)
    els = driver.find_elements_by_css_selector('.blk2_br div.contact > p')
    result = {}
    text = ''
    items = ['联系人','联系电话','联系手机','传真号码','联系邮箱','商家网址','公司地址','邮政编码','其它联系方式']
    for el in els:
        text = el.text
        for item in items:
            if text.find(item) == 0:
                result[item] = text[len(item) + 1:]

    save_page_source(id,PAGE_TYPE['contact'],url,driver.page_source)#保存页面源文件
    
    #driver.close()
    return result

def down_img(url):
    """
##urllib.request.urlretrieve('http://img2.jc001.cn/img/617/1417617/1301/1350ff4d1d5984e.png', 'D:\\11.png')
##下载图片
    """
    try:
        parturls = url.split('/')
        path = imgpath + '\\' + '\\'.join(parturls[4:-1])
        create_path(path)
        urllib.request.urlretrieve(url, path + '\\' + parturls[-1])
        return '\\'.join(parturls[4:])
    except:
        return ''
    
def create_path(part_path):
    '创建文件夹'
    if not path.exists(part_path) :
        ps = path.split(part_path)
        if len(ps) > 1 :
            #print(ps)
            create_path(ps[0])
        os.mkdir(part_path)

def create_comp_info(info, url):
    '将数据保存至数据库 register_money, established_time, register_area, corporation'
    try:
        ms = get_mssql_connect()
        ms.ExecNonQueryWithParam('INSERT INTO [CompanyInfo] \
    ([pageid],[introduction],[mainproducts],[logopath],[telephone],[mobile],[fax],[email],[netaddress],[compaddress],[zipcode],[other],[status],[person],[register_money], [established_time], [register_area], [corporation]) \
    VALUES (%d,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%d,%s,%s,%s,%s,%s); update NetAddress set Status = 1,cfstatus=%d,casestatus=%d where id=%d', tuple(list(info) + [info[0]]))
        id = info[0]
        if info[18] == 1:
            #处理荣誉证书
            certificates = get_all_certificate(id, url) #获取所有的荣誉
            create_certificate(ms, certificates) #保存信息
        if info[19] == 1:
            #处理成功案例
            case = get_all_case(id, url) #获取所有的成功案例
            data = ceate_case(ms, case) #保存信息

            for (cid,curl) in data:
                case_detail = get_all_case_detail(id, cid, curl) #获取所有的成功案例详细
                ceate_case_detail(ms, case_detail) #保存信息
    except Exception as Argument:
        log(Argument, info)

def log(exp, data):
    '记录日志'
    fp = open(logpath, 'a+', encoding='utf-8')
    fp.write(repr(exp))
    fp.write('\r\n')
    fp.write(repr(data))
    fp.write('\r\n')
    fp.write('================================================')
    fp.write('\r\n')
    fp.close()

####################################################certificate####################################################
def get_all_certificate(id, url):
    '获取所有的证书数据'
    data = get_certificate(id, url)
    return list(map(lambda s:[id] + s, data))

def get_certificate(id, url):
    '获取荣誉证书'
    
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

    save_page_source(id,PAGE_TYPE['certificate'],(url + 'certificate/'),driver.page_source)#保存页面源文件
    #driver.close()
    return result

def create_certificate(ms, info):
    '将数据保存至数据库'
    sql = ''
    para = []
    for inf in info:
        sql += 'insert into [certificate] ([pageid],[name],[imgpath]) values (%d,%s,%s); update NetAddress set cfstatus = 2 where id=%d; '
        para += inf + [inf[0]]
    ms.ExecNonQueryWithParam(sql, tuple(para))
#ms.ExecNonQueryWithParam('insert into [certificate] ([pageid],[name],[imgpath]) values (%d,%s,%s); update NetAddress set cfstatus = 1 where id=%d', tuple(list(info) + [info[0]]))

####################################################certificate####################################################


####################################################case####################################################
def get_case(id, url):
    '获取所有的成功案例的页面地址'
    driver.get(url + 'case/')
    els = driver.find_elements_by_css_selector('.case-list > li')
    result = []
    for el in els:
        name = el.find_element_by_tag_name('h4').text
        pageurl = el.find_element_by_tag_name('a').get_attribute('href')
        result.append([name, pageurl])

    save_page_source(id,PAGE_TYPE['case'],(url + 'case/'),driver.page_source)#保存页面源文件
    #driver.close()
    return result

def get_all_case(id, url):
    '获取所有的成功案例页面的数据'
    data = get_case(id, url)
    return list(map(lambda s:[id] + s, data))

def ceate_case(ms, info):
    '将所有成功案例的数据保存到数据库'
    sql = ''
    para = []
    for inf in info:
        sql += 'insert into [case] ([pageid],[name],[url]) values (%d,%s,%s); update NetAddress set casestatus = 2 where id=%d; '
        para += inf + [inf[0]]
    ms.ExecNonQueryWithParam(sql, tuple(para))
    if len(info) > 0:
        return ms.ExecQuery("select id,url from [case] where status = 0 and pageid="+ str(info[0][0]) +" order by id")
    else:
        return []
        
####################################################case####################################################

####################################################case_detail####################################################
def get_case_detail(id, cid, url):
    '获取所有的成功案例的页面地址'
    driver.get(url)
    els = driver.find_elements_by_css_selector('.cnt div.cnt img')
    result = []
    for el in els:
        imgurl = el.get_attribute('src')
        imgurl = down_img(imgurl) #下载图片
        result.append(imgurl)

    save_page_source(id,PAGE_TYPE['casedetail'], url, driver.page_source, cdid=cid)#保存页面源文件        
    #driver.close()
    return result

def get_all_case_detail(id, cid, url):
    '获取所有的成功案例详细页面的数据'
    data = get_case_detail(id, cid, url)
    if len(data) > 0:
        return list(map(lambda s:[cid, s], data))
    else:
        return [[cid, '', cid]]

def ceate_case_detail(ms, info):
    '将所有成功案例详细的数据保存到数据库'
    sql = ''
    para = []
    for inf in info:
        if len(inf[1]) > 0:
            sql += 'insert into [casedetail] ([caseid],[imgpath]) values (%d,%s); '
            para += inf
            sql += 'update [case] set status = 1 where id=%d; '
            para += [inf[0]]
    ms.ExecNonQueryWithParam(sql, tuple(para))

####################################################case_detail####################################################

def save_page_source(pageid, pagetype, pageurl, source, cdid=None):
    '保存页面记录信息'
    ms = get_mssql_connect()
    if cdid is None:
        ms.ExecNonQueryWithParam('insert into pagedetail (pageid,pagetype,pageurl,[source]) values (%d,%d,%s,%s);', (pageid, pagetype, pageurl, source))
    else:
        ms.ExecNonQueryWithParam('insert into pagedetail (pageid,pagetype,pageurl,[source],cdpageid) values (%d,%d,%s,%s,%d);', (pageid, pagetype, pageurl, source,cdid))

def get_mssql_connect():
    '获取数据连接'
    global ms
    if ms is None:
        ms = MSSQL(host="localhost",user="sa",pwd="sa",db="JZData")
    return ms

def disp_dirver():
    "释放资源"
    global driver
    if driver is not None:
        driver.quit()

def create_dirver():
    '创建dirver'
    global driver
    disp_dirver()
    driver = webdriver.PhantomJS(service_args=['--load-images=false'])#不加载图片

def recode(data):
    '记录日志'
    fp = open(r'd:\log\recode.txt', 'a+', encoding='utf-8')
    s = '({0},{1},"{2}","{3}")'.format(*data)
    fp.write(s)
    fp.write('\r\n')
    fp.close()
    print(s)

def main():
    """
    ms = MSSQL(host="localhost",user="sa",pwd="123456",db="PythonWeiboStatistics")
    返回的是一个包含tuple的list，list的元素是记录行，tuple的元素是每行记录的字段
    ms.ExecNonQuery("insert into WeiBoUser values('2','3')")
    """
    import time
    ms = get_mssql_connect()
    while True:
        try:
            resList = ms.ExecQuery("select top 500 id,title, url from NetAddress where Status = 0 and id > (select MAX(pageid) from CompanyInfo) order by id")
            if len(resList) == 0 : break
            create_dirver()#创建资源
            for (id, title, url) in resList:
                t1 = time.time()
                info = get_company_info(id, title, url)
                #print(url)
                #print(info)
                create_comp_info(info, url)
                recode([(time.time() - t1), id, title, url]) #记录日志
        except:
            pass
    else:
        os.popen('shutdown -s -t 0')#关机
        

if __name__ == '__main__':
    main()
