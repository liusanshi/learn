import urllib.request
import os,uuid,re
import requests
from os import path

from DB import MSSQL

img_dir = 'D:\\img'
ms = None

#server_url = 'http://upload.b2btst.com/uploadFile.do'
server_url = 'http://upload.b2bdev.com/uploadFile.do' #开发环境
server_url = 'http://upload.mycaigou.com/uploadFile.do'
img_field = 'imgid'

def upload_img(path, usage):
	'上传图片'
	if os.path.exists(path) :
		print('上传文件', path)
		param = {'type': 'img', 'chunk': 0, 'chunks': 1, 'usage': 'supplier-' + usage}
		param['name'] = str(uuid.uuid1()) + os.path.splitext(path)[1]
		file = {'file': (os.path.basename(path), open(path,'rb')) }
		r = requests.post(server_url, param, files=file)
		if r.text.find('文件上传成功') > 0 :
			match = re.search('"id":"([^"]+)"', r.text)
			if match:
				return match.group(1)
	return ''

def get_img_path(tb):
	"""
	获取数据
	使用方式：

	get_certificate_img_path = get_img_path('[certificate]')
	get_casedetail_img_path = get_img_path('[casedetail]')

	"""
	def _get_img_path():
		'获取数据'
		sql = "select top 100 id,imgpath from "+ tb +" where imgpath > '' and (%s is null or %s = '')" % (img_field, img_field)
		return get_mssql_connect().ExecQuery(sql)
	return _get_img_path

def get_logo_path():
	"获取供应商logo地址"
	sql = "select top 100 id, logopath as imgpath from [companyinfo] where logopath > '' and (%s is null or %s = '')" % (img_field, img_field)
	return get_mssql_connect().ExecQuery(sql)

def save_img_id(tb):
	"""
	保存图片id数据
	使用方式：

	save_certificate_img_id = save_img_id('[certificate]')
	save_casedetail_img_id = save_img_id('[casedetail]')

	"""
	def _save_img_id(data):
		"保存图片id"
		sql = 'update '+ tb +' set '+ img_field +'=%s where id=%d'
		ms = get_mssql_connect()
		ms.ExecNonQueryWithParam(sql, data)
	return _save_img_id

def get_mssql_connect():
    '获取数据连接'
    global ms
    if ms is None:
        ms = MSSQL(host="localhost",user="sa",pwd="sa",db="JZData")
    return ms

def operate_obj(get_data_func, save_data_func, usage):
	'操作一个对象'
	while True:
		reslist = get_data_func()
		if len(reslist) == 0: break
		for id,imgpath in reslist:
			imgid = upload_img(img_dir + '\\' + imgpath, usage)
			if len(imgid) > 0:
				save_data_func((imgid,id))

def main():
	get_certificate_img_path = get_img_path('[certificate]')	
	save_certificate_img_id = save_img_id('[certificate]')
	operate_obj(get_certificate_img_path, save_certificate_img_id, '4') #上传荣誉证书

	save_companyinfo_img_id = save_img_id('[companyinfo]')
	operate_obj(get_logo_path, save_companyinfo_img_id, 'operation')	#上传logo

	get_casedetail_img_path = get_img_path('[casedetail]')
	save_casedetail_img_id = save_img_id('[casedetail]')
	#operate_obj(get_casedetail_img_path, save_casedetail_img_id, 1)


if __name__ == '__main__':
	main()
