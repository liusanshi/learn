#coding=utf-8

import requests, json, uuid, re
from os import path as Path


class My_Caigou_API(object):
	"""
	明源采购系统的api

	login 调用登录服务, 返回session
	save_supplier 上传供应商数据
	save_certificate 上传供应商荣誉证书数据

	使用方法：
	Mycaigou = My_Caigou_API()
	if Mycaigou.login():
		Mycaigou.save_supplier(data)
		Mycaigou.save_certificate(data)
		
	"""
	Host = 'http://admin.b2bdev.com'

	_login_server_url = '/Mycaigou/Business/Common/LoginService.Login.ajax' #登录服务地址
	_save_certificate_server_url = '/Mycaigou/Business/SupplierManage/SupplierService.SaveAward.ajax' #添加供应商荣誉证书服务地址
	_get_supplier_id_server_url = '/WebPage/SupplierManage/SupplierEdit.aspx?mode=1' #获取供应商id的服务地址
	_save_supplier_server_url = '/Mycaigou/Business/SupplierManage/SupplierService.SaveSupplier.ajax' #保存供应商服务地址
	_check_companyname_server_url = '/Mycaigou/Business/SupplierManage/SupplierService.CheckCompanyName.ajax' #检查公司名称是否重复服务地址
	_save_form_server_url = '/Mycaigou/Business/Control/AppFormService.Save.ajax' #表单保存的方法
	_user = ''
	_pwd = ''
	_session = None #访问服务的会话

	def __init__(this, username = 'db2014', password = 'e10adc3949ba59abbe56e057f20f883e'):
		"正式环境账号：db2014 测试环境账号：admin"
		this._user = username
		this._pwd = password
	
	@property
	def login_server_url(this):
		"登录服务地址"
		return My_Caigou_API.Host + this._login_server_url

	@property
	def get_supplier_id_server_url(this):
		"获取供应商id的服务地址"
		return My_Caigou_API.Host + this._get_supplier_id_server_url

	@property
	def save_certificate_server_url(this):
		"添加供应商荣誉证书服务地址"
		return My_Caigou_API.Host + this._save_certificate_server_url

	@property
	def save_supplier_server_url(this):
		"保存供应商服务地址"
		return My_Caigou_API.Host + this._save_supplier_server_url

	@property
	def check_companyname_server_url(this):
		"检查公司名称是否重复服务地址"
		return My_Caigou_API.Host + this._check_companyname_server_url

	@property
	def save_form_server_url(this):
		"保存表单信息的服务地址"
		return My_Caigou_API.Host + this._save_form_server_url

	def login(this):
		"调用登录服务, 返回session"
		session = this._session = requests.Session()
		r = session.post(this.login_server_url, { "code": this._user, "pwd": this._pwd })
		if r.json().get('result', False):
			return session
		return None

	def get_supplier_id(this, index=0):
		"获取供应商id"
		r = this._session.get(this._get_supplier_id_server_url)
		match = re.findall('"__oid":"([^"]+)"', r.text)
		if 0 <= index < len(match):
			return int(match[index], 10)
		return -1
	
	def check_companyname(this, oid, company_name):
		"检查公司名称是否重复"
		data = {"cdata": json.dumps({'__mode' : 1, 'supplier_id' : oid, 'company_name': company_name })}
		r = this._session.post(this._check_companyname_server_url, data)
		if r.json().get('result', False):
			return True
		return False

	def save_supplier(this, data, source='九正建材'):
		"""保存供应商
			id : 0 #数据的id
			data的数据格式：
			{
			   company_name: ''#公司名称
			   ,establish_year : ''#成立年份
			   ,legal_name: '' #法人姓名
			   ,reg_capital = ''#注册资本
			   ,business_scope = ''#业务范围
			   ,website = ''#企业网址
			   ,weixin_public_account = ''#企业类别
			   ,telephone = ''#公司总机
			   ,fax = ''#公司传真
			   ,address = ''#公司地址
			   ,contact_name = ''#联系人
			   ,contact_mobile = ''#联系人手机
			   ,contact_telephone = ''#联系人座机
			   ,contact_mail = ''#联系人email
			   ,company_logo = '' #公司logofileid
			}
		"""
		temp_data = dict([(k,v) for k,v in data.items()]) #克隆一份数据
		oid = this.get_supplier_id() #当前数据的id
		company_name = temp_data.get('company_name','') #公司名称
		#数据验证
		if temp_data.get('company_logo') is None: temp_data['company_logo'] = ''
		if temp_data.get('establish_year') is not None: temp_data['establish_year'] = temp_data['establish_year'][:4]
		if temp_data.get('reg_capital') is not None: temp_data['reg_capital'] = temp_data['reg_capital'][:-1]

		if oid > -1 and len(company_name) > 0:
			temp_data['__mode'] = 1
			temp_data['__oid'] = oid
			temp_data['info_source'] = source
			
			temp_data = {"cdata": json.dumps(temp_data)}

			if this.check_companyname(oid, company_name):#检查公司是否重复
				print('导入供应商基本信息数据', data)
				r = this._session.post(this.save_supplier_server_url, temp_data)
				if r.json().get('result', False):
					print('返回结果', r.json())
					return oid
		return oid

	def save_certificate(this, data):
		"""保存供应商荣誉证书信息
			data的数据格式：
			{
				oid : ''#供应商id
				,name: ''#荣誉名称
				,imgid: ''#图片文件id
			}
		"""
		oid = data['oid']#供应商id
		award_name = data['name']#荣誉名称
		file_id = data['imgid']#图片文件id

		data = {'supplier_id': oid,'award_name':award_name,'award_file':file_id}

		# print('导入供应商荣誉证书', data)
		r = this._session.post(this._save_certificate_server_url, {"cdata": json.dumps(data)})
		return r.json().get('result', False)

	def save_company_introduction(this, supplier_id, intro, mode=1, metadataId='eca4ce68'):
		"上传简介信息 正式环境：metadataId='eca4ce68'； 测试环境: metadataId='26585ad4'"
		if len(intro) == 0: return True
		data = {
			'__mode' : mode,
			'__controlId' : 'form4',
			'__oid' : supplier_id,
			'__metadataId' : metadataId,
			'supplier_intro' : intro
		}
		r = this.submit_form(data)
		print(r)
		return r.get('result', False)

	def submit_form(this, data):
		"""提交表单
			data的数据格式：
			{
				__mode=1, #1:新增、2：修改
				__oid=7615, #对象的id
				__controlId=form4, #空间的id
				__metadataId=26585ad4, #元数据的id
			}
			返回字典 {"result":true,"message":"7615"}
		"""
		r = this._session.post(this.save_form_server_url, {"cdata": json.dumps(data)})
		return r.json()


class My_Caigou_Upload_Img(object):
	"""明源采购系统的 图片上传"""

	Host = 'http://admin.b2bdev.com'

	_uplaod_url = '/uploadFile.do' #上传路径

	def __init__(self, host=''):
		if host:
			My_Caigou_Upload_Img.Host = host

	@property
	def uplaod_url(this):
		"上传路径"
		return My_Caigou_Upload_Img.Host + this._uplaod_url


	def upload_img(this, path, usage=4):
		"上传图片"
		if Path.exists(path) :
			# print('上传文件', path)
			param = {'type': 'img', 'chunk': 0, 'chunks': 1, 'usage': 'supplier-' + usage}
			param['name'] = str(uuid.uuid1()) + Path.splitext(path)[1]
			file = {'file': (Path.basename(path), open(path,'rb')) }
			r = requests.post(this.uplaod_url, param, files=file)
			if r.text.find('文件上传成功') > 0 : #这个地方可以修改为 r.json()的返回值
				match = re.search('"id":"([^"]+)"', r.text)
				if match:
					return match.group(1)
		return ''
