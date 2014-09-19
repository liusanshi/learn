#coding=utf-8

import pymssql, DB

def get_object_fields(obj):
	"返回对象的所有字段信息"
	return ['[' + x +  ']' for x in vars(obj).keys() if x[:1] != '_']

def get_object_field_value(obj):
	"返回对象的所有字段的值"
	return tuple([x for x in vars(obj).values() if x[:1] != '_'])

def get_object_field_type(obj):
	"返回对象的所有字段的类型"
	return [convert_type(type(x)) for x in get_object_field_value(obj)]

def convert_type(t):
	"将类型转换为占位符"
	if str(t) == "<class 'int'>":
		return '%d'
	return '%s'

def exec_insert_sql(db, tablename, obj):
	"执行插入脚本"
	sql = 'insert into [' + tablename + '] ('+ ','.join(get_object_fields(obj)) +') values ('+ ','.join(get_object_field_type(obj)) \
		+');select SCOPE_IDENTITY() as id'
	obj.Id = db.ExecScalar(sql, get_object_field_value)


class DataBase(object):
	"""docstring for DataBase"""
	database = None
	Host = 'localhost'
	User = 'sa'
	PWD = 'sa'
	DB = 'JZData'

	def __init__(this):
		this.host = DataBase.Host
        this.user = DataBase.User
        this.pwd = DataBase.PWD
        this.db = DataBase.DB

    def get_db_server(this):
    	"获取数据库服务"
    	if DataBase.database is None:
    		DataBase.database = MSSQL(this.host, this.user, this.pwd, this.db)
    	return DataBase.database

	
class NetAddress(object):
	"""docstring for NetAddress"""
	def __init__(this, id=0, title='', url='', parent=0, count=0, typename='', status=0, cfstatus=0, casestatus=0):
		this.Id = id
		this.Title = title
		this.Url = url
		this.Parent = parent
		this.Count = count
		this.TypeName = typename
		this.Status = status
		this.cfStatus = cfstatus
		this.caseStatus = casestatus
		this._db = DataBase()

	def save(this):
		"保存数据库"
		exec_insert_sql('NetAddress', this)

	def update_status(this):
		"修改状态"
		if this.Id > 0 :
			sql = 'update [NetAddress] set [Status] = %d where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Status, this.Id))

	def get_need_operate_page(this):
		"返回需要处理的网址"
		sql = "select top 500 id,title, url from [NetAddress] where Status = 0 and id > (select MAX(pageid) from CompanyInfo) order by id"
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0: break
			for id,title, url in reslist:
				yield (id,title, url)


class CompanyInfo(object):
	"""docstring for CompanyInfo"""

	_company_info_fieldes = ['b.[id]', 'a.[Title]', 'a.[typename]', '[introduction]', '[mainproducts]', '[telephone]', '[mobile]', '[fax]', '[email]', 
	'[netaddress]', '[compaddress]', '[person]', '[register_money]', '[established_time]', '[corporation]', '[imgid]']

	_output_fields = ['id', 'company_name', 'weixin_public_account', 'introduction', 'business_scope', 'contact_telephone', 'contact_mobile', 'fax', 'contact_mail', 
	'website', 'address', 'contact_name', 'reg_capital' ,'establish_year', 'legal_name', 'company_logo']

	def __init__(this
		,id=0 ,pageid=0 ,introduction='' ,mainproducts='' ,logopath='' ,telephone=''
		,mobile='' ,fax='' ,email='' ,netaddress='' ,compaddress='',zipcode=''
		,other='' ,status=0 ,person='' ,register_money='' ,established_time=''
		,register_area='' ,corporation='' ,imgid =''):
		this.Id = id
		this.PageId = pageid
		this.Introduction = introduction
		this.MainProducts = mainproducts
		this.LogoPath = logopath
		this.Telephone = telephone
		this.Mobile = mobile
		this.Fax = fax
		this.Email = email
		this.NetAddress = netaddress
		this.CompAddress = compaddress
		this.ZipCode = zipcode
		this.Other = other
		this.Status = status
		this.Person = person
		this.Register_money = register_money
		this.Established_time = established_time
		this.Register_area = register_area
		this.Corporation = corporation
		this.Imgid = imgid
		this._db = DataBase()
	
	def save(this):
		"保存数据库"
		exec_insert_sql('CompanyInfo', this)

	def update_status(this):
		"修改状态"
		if this.Id > 0 :
			sql = 'update [CompanyInfo] set [Status] = %d where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Status, this.Id))

	def get_need_operate_company(this, typename):
		"获取所有需要处理上传的公司信息"
		sql = "select top 500 " + ','.join(CompanyInfo._company_info_fieldes)
		sql += " from dbo.NetAddress a inner join dbo.CompanyInfo b on a.Id = b.pageid where (telephone > '' or mobile > '') and b.[status] = 0 and typename like '"
		sql += typename + "%' "

		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0 : break
			for d in reslist:
				yield dict(zip(CompanyInfo._output_fields, d))

	def get_need_upload_logo(this, img_field='imgid'):
		"返回需要上传的图片信息"
		sql = "select top 500 id,imgpath from [CompanyInfo] where imgpath > '' and (%s is null or %s = '')" % (img_field, img_field)
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0 : break
			for d in reslist:
				yield d

class Certificate(object):
	"""docstring for Certificate"""

	_output_fields = ['oid', 'id', 'name', 'imgid']

	def __init__(this,id = 0,pageid = 0,name = '',imgpath = '',imgid = '',status =0):
		this.Id = id
		this.Pageid = pageid
		this.Name = name
		this.Imgpath = imgpath
		this.Imgid = imgid
		this.Status = status
		this._db = DataBase()

	def save(this):
		"保存数据库"
		exec_insert_sql('Certificate', this)

	def update_status(this):
		"修改状态"
		if this.Id > 0 :
			sql = 'update [Certificate] set [Status] = %d where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Status, this.Id))

	def update_imgid(this):
		"修改图片id"
		if this.Id > 0 :
			sql = 'update [Certificate] set [Imgid] = %s where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Imgid, this.Id))

	def get_need_operate_certificate(this):
		"获取公司所有需要处理上传的冗余荣誉证书信息"
		sql = "select top 500 a.status as oid,b.id,b.name,b.imgid from companyinfo a inner join [certificate] b on a.id = b.pageid where b.status = 0 and b.imgid > '' and a.status > 0 "
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0 : break
			for d in reslist:
				yield dict(zip(Certificate._output_fields, d))

	def get_need_upload_logo(this, img_field='imgid'):
		"返回需要上传的图片信息"
		sql = "select top 500 id,imgpath from [Certificate] where imgpath > '' and (%s is null or %s = '')" % (img_field, img_field)
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0 : break
			for d in reslist:
				yield d

class Case(object):
	"""docstring for Case"""
	def __init__(self,id = 0,pageid=0, name='', url='', status=0):
		self.Id = id
		self.Pageid = pageid
		self.Name = name
		self.Url = url
		self.Status = status
		this._db = DataBase()
	
	def save(this):
		"保存数据库"
		exec_insert_sql('Case', this)

	def update_status(this):
		"修改状态"
		if this.Id > 0 :
			sql = 'update [Case] set [Status] = %d where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Status, this.Id))

class CaseDetail(object):
	"""docstring for CaseDetail"""
	def __init__(self, id = 0, caseid = 0, imgpath = '',imgid = ''):
		self.Id = id
		self.Caseid = caseid
		self.Imgpath = imgpath
		self.Imgid = imgid
		this._db = DataBase()
	
	def save(this):
		"保存数据库"
		exec_insert_sql('CaseDetail', this)

	def update_imgid(this):
		"修改图片id"
		if this.Id > 0 :
			sql = 'update [CaseDetail] set [Imgid] = %s where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Imgid, this.Id))

class PageDetail(object):
	"""docstring for PageDetail"""
	def __init__(self, id=0, pageid=0, pagetype=0, source='', pageurl='', cdpageid=0):
		this.Id = id
		this.Pageid = pageid
		this.Pagetype = pagetype
		this.Source = source
		this.Pageurl = pageurl
		this.Cdpageid = cdpageid
		this._db = DataBase()

	def save(this):
		"保存数据库"
		exec_insert_sql('PageDetail', this)

	def update_cdpageid(this):
		"修改cdpageid"
		if this.Id > 0 :
			sql = 'update [PageDetail] set [Cdpageid] = %s where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Cdpageid, this.Id))