#coding=utf-8

from DB import MSSQL

def get_object_fields(obj):
	"返回对象的所有字段信息"
	return ['[' + x +  ']' for x in vars(obj).keys() if x[:1] != '_' and x != 'Id']

def get_object_field_value(obj):
	"返回对象的所有字段的值"
	# print([(k,x) for k,x in vars(obj).items()])
	return tuple([v for k,v in vars(obj).items() if k[:1] != '_' and k != 'Id'])

def get_object_field_type(obj):
	"返回对象的所有字段的类型"
	return [convert_type(type(x)) for x in get_object_field_value(obj)]

def convert_type(t):
	"将类型转换为占位符"
	if str(t) == "<class 'int'>":
		return '%d'
	return '%s'

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

class EntityBase(object):
	"""实体对象基类"""
	def __init__(this, tablename=''):
		super(EntityBase, this).__init__()
		this._tablename = tablename
		this._db = DataBase().get_db_server()

	def save(this):
		"保存数据库"
		sql = 'insert into [' + this._tablename + '] ('+ ','.join(get_object_fields(this)) +') values ('+ ','.join(get_object_field_type(this)) \
		+');select SCOPE_IDENTITY() as id'
		
		# print(sql)
		# print(get_object_field_value(this))

		this.Id = int(this._db.ExecScalar(sql, get_object_field_value(this)))

	def get_by_id(this):
		"根据Id查询数据"
		fields = get_object_fields(this)
		sql = 'select top 1 ' + ','.join(fields) + ' from [' + this._tablename + '] where [Id] = %d ' % this.Id
		reslist = this._db.ExecQuery(sql)
		if reslist:
			return dict(zip(fields, reslist))
		return None
	
class NetAddress(EntityBase):
	"""docstring for NetAddress"""
	def __init__(this, id=0, title='', url='', parent=0, count=0, typename='', status=0):
		super(NetAddress, this).__init__('NetAddress')
		this.Id = id
		this.Title = title
		this.Url = url
		this.Parent = parent
		this.Count = count
		this.TypeName = typename
		this.Status = status

	def update_status(this):
		"修改状态"
		if this.Id > 0 :
			sql = 'update [NetAddress] set [Status] = %d where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Status, this.Id))

	def update_count(this):
		"修改数量"
		if this.Id > 0 :
			sql = 'update [NetAddress] set [Count]=%d where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Count, this.Id))

	def get_need_operate_page(this):
		"返回需要处理的网址"
		sql = "select top 500 id,title, url from [NetAddress] where Status = 0 and Parent>=0 and id > isnull((select MAX(pageid) from CompanyInfo),0) order by id"
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0: break
			for id,title, url in reslist:
				yield (id,title, url)

	def get_child_address(this):
		"获取子目录"
		sql = "select top 500 id,title, url from [NetAddress] where Status = %d and Parent = %d order by id" % (this.Status, this.Id)
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0: break
			for id,title, url in reslist:
				yield (id,title, url)

	def get_need_operate_cate(this, maxParent):
		"获取需要处理的分类"
		sql = "select id,title, url from [NetAddress] where Status =  %d and Parent>=0 and Parent<=%d order by id" % (this.Status, maxParent)
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0: break
			for id,title, url in reslist:
				yield (id,title, url)

	def get_cate_count(this):
		"获取当前页已经抓取了多少数据"
		sql = "select count(1) from [NetAddress] where Parent=%d" % this.Id
		res = this._db.ExecQuery(sql)
		return int(res[0][0])

	def get_page_index(this, page_size):
		"获取数据说出的页数"
		sql = ""

	def is_end(this):
		"判断实体是否已经抓取完成"
		old_num = this.get_cate_count()
		reqriue_num = this.get_by_id()['Id']
		return old_num == reqriue_num

class CompanyInfo(EntityBase):
	"""docstring for CompanyInfo"""

	_company_info_fieldes = ['b.[id]', 'a.[Title]', 'a.[typename]', '[introduction]', '[mainproducts]', '[telephone]', '[mobile]', '[fax]', '[email]', 
	'[netaddress]', '[compaddress]', '[person]', '[register_money]', '[established_time]', '[corporation]', '[imgid]']

	_output_fields = ['id', 'company_name', 'weixin_public_account', 'introduction', 'business_scope', 'contact_telephone', 'contact_mobile', 'fax', 'contact_mail', 
	'website', 'address', 'contact_name', 'reg_capital' ,'establish_year', 'legal_name', 'company_logo']

	def __init__(this
		,id=0 ,pageid=0 ,introduction='' ,mainproducts='' ,logopath='' ,telephone=''
		,mobile='' ,fax='' ,email='' ,netaddress='' ,compaddress='',zipcode=''
		,other='' ,status=0 ,person='' ,register_money='' ,established_time=''
		,register_area='' ,corporation='' ,imgid ='', cfstatus=0, casestatus=0):
		super(CompanyInfo, this).__init__('CompanyInfo')
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
		this.cfStatus = cfstatus
		this.caseStatus = casestatus
	
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

	def get_need_upload_intro_company(this):
		"获取需要上传简介的公司[临时使用，后面应该不需要；在上传基本信息的时候上传简介数据]"
		sql = 'select [status] as supplierid,introduction from companyinfo where status > 0 and DATALENGTH (introduction) > 0'
		while True:
			reslist = this._db.ExecQuery(sql)
			if len(reslist) == 0 : break
			for supplierid,introduction in reslist:
				try:
					yield supplierid,introduction.encode('latin-1').decode('gbk')
				except UnicodeDecodeError:
					pass

class Certificate(EntityBase):
	"""docstring for Certificate"""

	_output_fields = ['oid', 'id', 'name', 'imgid']

	def __init__(this,id = 0,pageid = 0,name = '',imgpath = '',imgid = '',status =0):
		super(Certificate, this).__init__('Certificate')
		this.Id = id
		this.Pageid = pageid
		this.Name = name
		this.Imgpath = imgpath
		this.Imgid = imgid
		this.Status = status

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

class Case(EntityBase):
	"""docstring for Case"""
	def __init__(self,id = 0,pageid=0, name='', url='', status=0):
		super(Case, self).__init__('Case')
		self.Id = id
		self.Pageid = pageid
		self.Name = name
		self.Url = url
		self.Status = status

	def update_status(this):
		"修改状态"
		if this.Id > 0 :
			sql = 'update [Case] set [Status] = %d where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Status, this.Id))

class CaseDetail(EntityBase):
	"""docstring for CaseDetail"""
	def __init__(self, id = 0, caseid = 0, imgpath = '',imgid = ''):
		super(CaseDetail, self).__init__('CaseDetail')
		self.Id = id
		self.Caseid = caseid
		self.Imgpath = imgpath
		self.Imgid = imgid

	def update_imgid(this):
		"修改图片id"
		if this.Id > 0 :
			sql = 'update [CaseDetail] set [Imgid] = %s where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Imgid, this.Id))

class PageDetail(EntityBase):
	"""docstring for PageDetail"""
	def __init__(this, id=0, pageid=0, pagetype=0, source='', pageurl='', cdpageid=0):
		super(PageDetail, this).__init__('PageDetail')
		this.Id = id
		this.Pageid = pageid
		this.Pagetype = pagetype
		this.Source = source
		this.Pageurl = pageurl
		this.Cdpageid = cdpageid

	def update_cdpageid(this):
		"修改cdpageid"
		if this.Id > 0 :
			sql = 'update [PageDetail] set [Cdpageid] = %s where [Id] = %d'
			this._db.ExecNonQueryWithParam(sql, (this.Cdpageid, this.Id))

	def is_exists(this):
		"是否存在记录"
		sql = 'select top 1 1 from [PageDetail] where Pageid=%d' % this.Pageid
		reslist = this._db.ExecQuery(sql)
		return len(reslist) > 0