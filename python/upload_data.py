import os,re,json
from os import path
import requests

from DB import MSSQL

# from selenium import webdriver
# from selenium.webdriver.common.keys import Keys
# from selenium.common.exceptions import NoSuchElementException
# import urllib.request
# from http import cookiejar

driver = None
imgpath = 'D:\\img'
ms = None
PAGE_TYPE = {'about':1,'contact':2,'certificate':3,'case':4, 'casedetail':5}
user_password = ('db2014', '123456')

ligin_url = 'http://admin.b2bdev.com/Login.aspx'
add_data_url = 'http://admin.b2bdev.com/WebPage/SupplierManage/SupplierEdit.aspx?mode=1'
add_certificate_url = 'http://admin.b2bdev.com/WebPage/SupplierManage/SupplierEdit.aspx?mode=2&oid={0}'


# login_server = 'http://admin.b2bdev.com/Mycaigou/Business/Common/LoginService.Login.ajax' #登录服务
# save_certificate_server = 'http://admin.b2bdev.com/Mycaigou/Business/SupplierManage/SupplierService.SaveAward.ajax' #添加供应商荣誉证书服务
# save_supplier_server = 'http://admin.b2bdev.com/Mycaigou/Business/SupplierManage/SupplierService.SaveSupplier.ajax' #保存供应商服务
# check_company_name = 'http://admin.b2bdev.com/Mycaigou/Business/SupplierManage/SupplierService.CheckCompanyName.ajax' #检查公司名称是否重复服务

## test
# login_server = 'http://localhost:31843/Mycaigou/Business/Common/LoginService.Login.ajax'
# save_certificate_server = 'http://localhost:31843/Mycaigou/Business/SupplierManage/SupplierService.SaveAward.ajax'
# save_supplier_server = 'http://localhost:31843/Mycaigou/Business/SupplierManage/SupplierService.SaveSupplier.ajax' #保存供应商服务
# check_company_name = 'http://localhost:31843/Mycaigou/Business/SupplierManage/SupplierService.CheckCompanyName.ajax' #检查公司名称是否重复服务
# add_data_url = 'http://localhost:31843/WebPage/SupplierManage/SupplierEdit.aspx?mode=1'

# 正式环境
add_data_url = 'http://admin.mycaigou.com/WebPage/SupplierManage/SupplierEdit.aspx?mode=1'
login_server = 'http://admin.mycaigou.com/Mycaigou/Business/Common/LoginService.Login.ajax' #登录服务
save_certificate_server = 'http://admin.mycaigou.com/Mycaigou/Business/SupplierManage/SupplierService.SaveAward.ajax' #添加供应商荣誉证书服务
save_supplier_server = 'http://admin.mycaigou.com/Mycaigou/Business/SupplierManage/SupplierService.SaveSupplier.ajax' #保存供应商服务
check_company_name = 'http://admin.mycaigou.com/Mycaigou/Business/SupplierManage/SupplierService.CheckCompanyName.ajax' #检查公司名称是否重复服务

#全部加载
all_load = {"新型建材","门窗","地板","洁具","板材","管材","阀门","灯具","钢材","陶瓷",
        "电气","安防","橱柜厨具","基础建材","吊顶","钢结构","节能环保","防水","铝材幕墙壁纸","管件","电梯"}

#部分加载
partial_load = {
	  "五金":[ "装饰五金", "门窗五金", "卫浴五金" ]
	, "涂料":[ "建筑涂料", "水性漆", "防腐涂料", "纳米漆","墙漆","油漆" ]#涂料（建筑涂料  水性漆  防腐涂料  纳米漆  墙漆  油漆）
	, "电工电气":[ "开关插座", "电线电缆" ]#电工电气（开关插座  电线电缆  ）
	, "家具":[ "办公家具", "卧室家具", "书房家具", "户外家具","客厅家具","门厅家具", "红木家具" ]#家具（办公家具  卧室家具   书房家具  户外家具   客厅家具  门厅家具   红木家具）
	, "石材":[ "荒料板材", "环境装饰", "异形工艺", "石雕","砂岩","石材家具" ]#石材 （荒料板材   环境装饰   异形工艺  石雕   砂岩   石材家具） 
	, "家用电器":[ "厨房电器", "卫浴电器", "空调" ]#家用电器（厨房电器   卫浴电器    空调） 
	, "玻璃":[ "建筑玻璃", "装饰玻璃" ]#玻璃（建筑玻璃   装饰玻璃  ）
	, "采暖":[ "地板采暖", "壁挂炉", "地暖材料", "空气能壁挂炉","锅炉","太阳能热水器" ]#采暖（地板采暖    壁挂炉   地暖材料   空气能壁挂炉   锅炉   太阳能热水器   ）
	, "油漆":[ "聚酯漆", "树脂", "防火油漆" ]#油漆（聚酯漆　树脂　防火油漆）
	, "家居":[ "智能家居"]#家居（智能家居）
	, "园艺":[ "绿化苗木", "园林设施", "花卉园艺" ]#园艺（绿化苗木　　园林设施　　花卉园艺）
	, "楼梯":[ "实木楼梯", "钢木楼梯", "不锈钢楼梯", "铁艺楼梯","旋转楼梯","玻璃楼梯" ]#楼梯（实木楼梯　　钢木楼梯　　不锈钢楼梯　　铁艺楼梯　　旋转楼梯　玻璃楼梯）
	, "锁具":[ "门窗锁", "室内门锁" ]#锁具（门窗锁　　室内门锁　）
	, "布艺":[ "布艺窗帘" ]#布艺（布艺窗帘）
	, "窗帘":[ "电动窗帘", "布艺窗帘", "办公窗帘" ]#窗帘（电动窗帘　布艺窗帘　办公窗帘）
	, "铁艺":[ "铁艺建筑装饰" ]#铁艺（铁艺建筑装饰）
	, "家纺":[ "毯类系列" ]#家纺（毯类系列）
}

#需要使用到的字段
company_info_fieldes = ['b.[id]', 'a.[Title]', 'a.[typename]', '[introduction]', '[mainproducts]', '[logopath]', '[telephone]', '[mobile]', '[fax]', '[email]', 
'[netaddress]', '[compaddress]', '[zipcode]', '[other]', '[person]', '[register_money]', '[established_time]', '[register_area]', 
'[corporation]', '[imgid]']

#荣誉证书使用到的字段
certificate_fieldes = ['oid','id','name','imgid']

#数据对应的索引
data_dic = dict(zip(company_info_fieldes, range(len(company_info_fieldes))))
certificate_data_dic = dict(zip(certificate_fieldes, range(len(certificate_fieldes)))) #荣誉证书的数据字典

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
    driver = webdriver.PhantomJS('E:\\phantomjs-1.9.7-windows\\phantomjs.exe', service_args=['--load-images=false'])#不加载图片

def login(user, psd):
    "登录"
    create_dirver()
    driver.get(ligin_url)
    driver.find_element_by_id('txtCode$text').send_keys(user)
    driver.find_element_by_id('txtPwd$text').send_keys(psd)
    driver.find_element_by_id('btnLogin').click()

def add_data(data):
	driver.get(add_data_url)
	
	oid = get_oid(driver.page_source, 0)#当前数据的id
	id = get_data_by_name(data, 'b.[id]')#数据id

	company_name = get_data_by_name(data, 'a.[Title]')#公司名称
	established_time = get_data_by_name(data, '[established_time]')[:4]#成立年份
	legal_name = get_data_by_name(data, '[corporation]')#法人姓名
	reg_capital = get_data_by_name(data, '[register_money]')[:-1]#注册资本
	business_scope = get_data_by_name(data, '[mainproducts]')#业务范围
	website = get_data_by_name(data, '[netaddress]')#企业网址
	company_cate = get_data_by_name(data, 'a.[typename]')#企业类别
	telephone = ''#公司总机
	fax = get_data_by_name(data, '[fax]')#公司传真
	company_address = get_data_by_name(data, '[compaddress]')#公司地址
	contact_name = get_data_by_name(data, '[person]')#联系人
	contact_mobile = get_data_by_name(data, '[mobile]')#联系人手机
	contact_telephone = get_data_by_name(data, '[telephone]')#联系人座机
	contact_mail = get_data_by_name(data, '[email]')#联系人email

	phContent_form1 = driver.find_element_by_id('phContent_form1')
	phContent_form1.find_element_by_id('phContent_form1_txtCompanyName$text').send_keys(company_name)#公司名称
	phContent_form1.find_element_by_id('txtYear').send_keys(established_time)#成立年份
	phContent_form1.find_element_by_id('phContent_form1_txtLegalName$text').send_keys(legal_name)#法人姓名
	phContent_form1.find_element_by_id('phContent_form1_anRegCapital$text').send_keys(reg_capital)#注册资本
	phContent_form1.find_element_by_id('phContent_form1_taBusinessScope$text').send_keys(business_scope)#业务范围
	phContent_form1.find_element_by_id('phContent_form1_txtWebsite$text').send_keys(website)#企业网址
	phContent_form1.find_element_by_id('phContent_form1_txtWeixinPublicAccount$text').send_keys(company_cate)#企业类别
	phContent_form1.find_element_by_id('phContent_form1_txtInfoSource$text').send_keys('九正建材')#数据来源
	
	phContent_form1.find_element_by_id('phContent_form1_txtTelephone$text').send_keys(telephone)#公司总机
	phContent_form1.find_element_by_id('phContent_form1_txtFax$text').send_keys(fax)#公司传真
	phContent_form1.find_element_by_id('phContent_form1_txtAddress$text').send_keys(company_address)#公司地址
	phContent_form1.find_element_by_id('phContent_form1_txtContactName$text').send_keys(contact_name)#联系人
	phContent_form1.find_element_by_id('phContent_form1_txtContactMobile$text').send_keys(contact_mobile)#联系人手机
	phContent_form1.find_element_by_id('phContent_form1_txtContactTelephone$text').send_keys(contact_telephone)#联系人座机
	phContent_form1.find_element_by_id('phContent_form1_txtContactMail$text').send_keys(contact_mail)#联系人email

	if check_data() :
		driver.find_element_by_id('phContent_btnSaveSupplier').click()#点击保存按钮
		return id, oid
	else :
		return id, -1 #数据不合法

def check_data():
	"验证数据的合法性"
	try:
		driver.find_element_by_class_name('prompt_msg_error')
		return True
	except NoSuchElementException:
		return False

def add_data_requests(session, data):
	"使用requests上传数据"

	id = get_data_by_name(data, 'b.[id]')#数据id
	oid = get_cur_oid(session, 0)#当前数据的id
	if oid == -1: return id, oid

	company_name = get_data_by_name(data, 'a.[Title]')#公司名称
	established_time = get_data_by_name(data, '[established_time]')[:4]#成立年份
	legal_name = get_data_by_name(data, '[corporation]')#法人姓名
	reg_capital = get_data_by_name(data, '[register_money]')[:-1]#注册资本
	business_scope = get_data_by_name(data, '[mainproducts]')#业务范围
	website = get_data_by_name(data, '[netaddress]')#企业网址
	company_cate = get_data_by_name(data, 'a.[typename]')#企业类别
	telephone = ''#公司总机
	fax = get_data_by_name(data, '[fax]')#公司传真
	company_address = get_data_by_name(data, '[compaddress]')#公司地址
	contact_name = get_data_by_name(data, '[person]')#联系人
	contact_mobile = get_data_by_name(data, '[mobile]')#联系人手机
	contact_telephone = get_data_by_name(data, '[telephone]')#联系人座机
	contact_mail = get_data_by_name(data, '[email]')#联系人email
	company_logo = get_data_by_name(data, '[imgid]') #公司logofileid
	if company_logo is None: company_logo = ''

	data = {'__mode':1, '__oid':oid, 'company_name': company_name,'establish_year':established_time,'legal_name':legal_name,
			'reg_capital':reg_capital, 'business_scope':business_scope,'website':website, 'weixin_public_account':company_cate,
			'fax' : fax, 'address':company_address, 'info_source':'九正建材', 'contact_name':contact_name, 'contact_mobile':contact_mobile,
			'contact_telephone':contact_telephone, 'contact_mail':contact_mail,'company_logo':company_logo
			}
	data = {"cdata": json.dumps(data)}

	#检查公司是否重复
	if check_data_requests(session, oid, company_name):
		print('导入供应商基本信息数据', data)
		r = session.post(save_supplier_server, data) #上传数据
		if r.json().get('result', False):
			print('返回结果', r.json())
			return id, oid
	return id, -1

def check_data_requests(session, oid, company_name):
	"检查公司名称是否重复"
	data = {"cdata": json.dumps({'__mode' : 1, 'supplier_id' : oid, 'company_name': company_name })}

	r = session.post(check_company_name, data)
	if r.json().get('result', False):
		return True
	return False

def get_cur_oid(session, index):
	'获取oid'
	#add_data_url
	r = session.get(add_data_url)
	match = re.findall('"__oid":"([^"]+)"', r.text)
	if 0 <= index < len(match):
		return int(match[index], 10)
	return -1


def upload_certificate_data(data):
	"上传荣誉证书数据"
	oid = get_data_by_name(data, 'oid',certificate_data_dic)#荣誉名称
	award_name = get_data_by_name(data, 'name',certificate_data_dic)#荣誉名称
	file_id = get_data_by_name(data, 'imgid', certificate_data_dic)#文件id
	id = get_data_by_name(data, 'id', certificate_data_dic)#文件id

	print(add_certificate_url.format(oid))

	driver.get(add_certificate_url.format(oid))

	driver.find_element_by_id('mini-1$3').click() #找到荣誉证书页签点击
	#phContent_form3 = driver.find_element_by_id('phContent_form3')

	driver.find_element_by_id('phContent_form3_txtAwardName$text').send_keys(award_name)#荣誉名称
	driver.execute_script('document.querySelector(\'#uploadAward > input[type="hidden"]\').value="'+ file_id +'"')
	
	# driver.find_element_by_css_selector('#uploadAward > input[type="hidden"]').send_keys(file_id)#荣誉名称
	print(driver.find_element_by_css_selector('#uploadAward > input[type="hidden"]').text)
	
	driver.find_element_by_id('phContent_btnSaveAward').click() #点击保存按钮
	return id

def login_session():
	"login_session"
	# cookieJar = cookiejar.CookieJar()
	# cookie = urllib.request.HTTPCookieProcessor(cookieJar)
	# opener = urllib.request.build_opener(cookie)
	# data = { "code": 'admin', "pwd": 'e10adc3949ba59abbe56e057f20f883e' }
	# data = urllib.parse.urlencode(data).encode()
	# req = urllib.request.Request(login_server, data)
	# result = opener.open(req)
	# return opener

	# 使用requests 来登录
	session = requests.Session()
	r = session.post(login_server, { "code": 'db2014', "pwd": 'e10adc3949ba59abbe56e057f20f883e' })
	print(r.text)
	if r.json().get('result', False):
		return session
	return None


def upload_certificate_data_urllib(session, data):
	oid = get_data_by_name(data, 'oid',certificate_data_dic)#荣誉名称
	award_name = get_data_by_name(data, 'name',certificate_data_dic)#荣誉名称
	file_id = get_data_by_name(data, 'imgid', certificate_data_dic)#文件id
	id = get_data_by_name(data, 'id', certificate_data_dic)#文件id


	# { "cdata": mini.encode(data) }
	# data = {"cdata": {'supplier_id': oid,'award_name':award_name,'award_file':file_id}}
	# data = urllib.parse.urlencode(data).encode()
	# req = urllib.request.Request(save_certificate_server, data)
	# opener.open(req)
	data = {'supplier_id': oid,'award_name':award_name,'award_file':file_id}

	print('导入供应商荣誉证书', data)

	r = session.post(save_certificate_server, {"cdata": json.dumps(data)})
	if r.json().get('result', False):
		return id
	return -1
	

def get_data_by_name(data, name, dic = data_dic):
	"根据名称获取数据"
	index = dic.get(name, -1)
	if index > -1:
		return data[index]
	return None

def get_company_info(typename):
	'获取所有公司数据'
	sql = "select top 500 " + ','.join(company_info_fieldes)
	sql += " from dbo.NetAddress a inner join dbo.CompanyInfo b on a.Id = b.pageid where (telephone > '' or mobile > '') and b.[status] = 0 and typename like '"
	sql += typename + "%' and (logopath = '' or (logopath > '' and b.imgid > ''))"
	while True:
		reslist = get_mssql_connect().ExecQuery(sql)
		if len(reslist) == 0 : break
		for d in reslist:
			yield d
		
def save_data_id(id, oid):
	"保存数据的id"
	sql = 'update CompanyInfo set status = %d where id=%d'
	get_mssql_connect().ExecNonQueryWithParam(sql, (oid, id))
	

def get_all_company_info():
	"获取所有的公司数据"

	for d in get_company_info('门窗'):
		yield d


	# #all_load,先加载所有的数据
	# for al in all_load:
	# 	for d in get_company_info(al):
	# 		yield d

	# #partial_load,再部分加载数据
	# for k,v in partial_load.items():
	# 	for vi in v:
	# 		for d in get_company_info(k + '>>' + vi):
	# 			yield d


def get_oid(text, index):
    '获取oid'
    match = re.findall('"__oid":"([^"]+)"', text)
    if 0 <= index < len(match):
        return match[index]
    return ''

def get_certificate_data():
	"获取所有荣誉证书数据"
	sql = "select top 500 a.status as oid,b.id,b.name,b.imgid from companyinfo a inner join [certificate] b on a.id = b.pageid where b.status = 0 and b.imgid > '' and a.status > 0 "
	while True:
		reslist = get_mssql_connect().ExecQuery(sql)
		if len(reslist) == 0 : break
		for d in reslist:
			yield d

def save_certificate_data_id(id):
	"保存数据的id"
	sql = 'update [certificate] set status = %d where id=%d'
	get_mssql_connect().ExecNonQueryWithParam(sql, (1, id))

def main():
	# login(*user_password)	
	# index = 0
	# for d in get_all_company_info():
	# 	id, oid = add_data(d)
	# 	save_data_id(id, oid)
	# 	index += 1
	# 	if index == 500:
	# 		index = 0
	# 		login(*user_password)#重新登录释放内存

	session = login_session() #登陆，获取登录时的cookie
	if session:
		for d in get_all_company_info():#导入供应商基本信息数据
			id, oid = add_data_requests(session, d)
			if oid > 0:
				save_data_id(id, oid)

		for d in get_certificate_data():#处理荣誉证书
			id = upload_certificate_data_urllib(session, d)
			if id > 0:
				save_certificate_data_id(id)


if __name__ == '__main__':
    main()
