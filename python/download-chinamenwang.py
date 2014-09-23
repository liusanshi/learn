#codeing=utf-8
# 下载chinamenwang 数据

import requests, re, os
from bs4 import BeautifulSoup
from os import path as Path
from time import time
from urllib.request import urlretrieve
from my_caigou_db_api import *

host = 'http://www.chinamenwang.com'	#主机域名
index_url = 'http://www.chinamenwang.com/company/' #下载数据的首页
encoding = 'gb2312'
html_save_path = "d:\\html\\chinamenwang"
DataBase.DB = 'chinamenwang'
imgpath = 'd:\\img_chinamenwang'

def operate_page(id, title, url, cb):
	"处理一个页面的基本方法"
	resp = requests.get(url, headers={
		'User-Agent': 'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36'
		,'Referer': 'http://www.chinamenwang.com/company/'
		,'Connection': 'keep-alive'
		,'Cache-Control': 'max-age=0'
	})
	resp.encoding = encoding
	save_html(id, resp.text, url) #将网页保存起来
	soup = BeautifulSoup(resp.text)
	return cb(soup)


def download_category(id, title, url):
	"下载分类数据"
	def _download_category(soup):
		data = []
		for cate in soup.select('#sideMain1 > dl'):
			data.append(get_category(cate))
		return data

	return operate_page(id, title, url, _download_category)

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
	url = a.attrs['href']
	if url.find('http://') >= 0:
		return {
		'url' : a.attrs['href'],
		'title' : a.text
		}
	else:
		return {
			'url' : host + a.attrs['href'],
			'title' : a.text
		}

def get_item_info(soup):
	"返回页面上所有的公司信息"
	comp_list = soup.select('div.leftCOnt2List > dl > dt')
	for comp in comp_list:
		a = comp.select('b > a')
		if len(a) > 0:
			yield a_to_data(a[0])

def get_cate_total_count(soup):
	"获取当前分类的数据的总量"
	count_control = soup.select('div.leftCOnt2Head > b > i:nth-of-type(2)')
	if count_control:
		return int(count_control[0].text)
	return 1

def get_page_count(soup):
	"获取当前类型的中页数"
	page_control = soup.select('div.list_pages > span.fl')
	if page_control:
		match = re.search('共(\d+)页', page_control[0].text)
		if match:
			return int(match.group(1))
	return 1

def get_page_list(id, url, page_index = 1):
	"获取当前类型的所有页"

	curl = url if page_index==1 else url + '_' + str(page_index)
	resp = requests.get(curl)
	resp.encoding = encoding
	save_html(id, resp.text, curl) #将网页保存起来
	soup = BeautifulSoup(resp.text)

	page_count = get_page_count(soup)
	
	total = get_cate_total_count(soup)
	print('page_count:',page_count,' total:',total)

	# 保存一些汇总信息
	if page_index==1:
		NetAddress(id=id, count=total).update_count()

	for d in get_item_info(soup): # 第一页的信息
		yield d
	page_index = page_index + 1

	while page_index <= page_count:
		curl = url + '_' + str(page_index)
		for x in operate_page(id, '', curl, get_item_info):
		 	yield x
		page_index = page_index + 1

def get_root_page():
	"获取根目录"
	na = NetAddress(id=-1)
	result = None
	for i in na.get_child_address():
		result = i
		break
	else:
		#新建
		na.Title = 'chinamenwang index'
		na.Url = index_url
		na.Parent = -1
		na.save()
		result = (na.Id, na.Title, na.Url)
	return result

def save_html(pageid, text, url):
	"保存网页文件"
	# path = html_save_path + '\\' + name
	# with open(path, 'wb') as fp:
	# 	#fp.write(text.encode(encoding))
	# 	fp.write(text.encode())

	pd = PageDetail(pageid=pageid, source=text, pageurl=url)
	if not pd.is_exists():
		pd.save()

def recode_all_cate():
	"记录总目录"
	root_page = get_root_page()
	#print(root_page)
	data = download_category(*root_page) #下载总目录
	root_page_id = root_page[0]

	for item in data:
		na = NetAddress(title=item['title'], url=item['url'], parent=root_page_id, status=1)
		na.save()
		cur_page_id = na.Id
		for citem in item['child']:
			na = NetAddress(title=citem['title'], url=citem['url'], parent=cur_page_id)
			na.save()

def down_load_dir():
	"下载目录"
	for pid,ptitle,purl in NetAddress(id=16).get_child_address(): #一级目录
		for id,title, url in NetAddress(id=pid).get_child_address(): #二级目录
			total = NetAddress(id=id).get_cate_count()
			page_index =  total // 10
			page_index = (page_index + 1) if total/10 >= page_index else page_index
			print(id,title, url, page_index)
			for x in get_page_list(id, url,page_index) : #目录中的所有文档
				NetAddress(title=x['title'],url=x['url'],parent=id).save()
			NetAddress(status=1, id=id).update_status() #将数据标识为已经下载

def down_img(url):
    """
http://www.chinamenwang.com/Userfiles/130403929390/201433164717.jpg
http://www.chinamenwang.com/u/180-135-a/Userfiles/130403929390/201433164717.jpg
##urllib.request.urlretrieve('http://img2.jc001.cn/img/617/1417617/1301/1350ff4d1d5984e.png', 'D:\\11.png')
##下载图片
    """
    try:
        parturls = url.split('/')
        if len(parturls) > 6:
        	parturls[-5:-3] = [] #将中间的路径删除
        path = imgpath + '\\' + parturls[-2]
        create_path(path)
        # print(url, path + '\\' + parturls[-1])
        urlretrieve('/'.join(parturls), path + '\\' + parturls[-1])
        return '\\'.join(parturls[-2:])
    except:
        return ''

def create_path(part_path):
    '创建文件夹'
    if not Path.exists(part_path) :
        ps = Path.split(part_path)
        if len(ps) > 1 :
            #print(ps)
            create_path(ps[0])
        os.mkdir(part_path)

def get_company_from_page(soup):
	"从单个页面上分析数据"
	top_block = soup.select('div.cp_top2 > b')
	main_product = ''
	company_name =''
	company_address =''
	logo_path =''
	company_intro =''
	person = ''
	telephone = ''
	netaddress = ''
	fax = ''
	email = ''
	# brand_t
	jianxie = ''

	if len(top_block) == 3:
		main_product,company_name,company_address = [i.text for i in top_block]
	
	# logo图片地址
	logo_img = soup.select('div.cp_logo > a > img')
	if logo_img:
		logo_path = down_img(logo_img[0].attrs['src'])
	
	# 公司简介
	# company_intro = '\n'.join((i.text for i in soup.select('.c_infos > p')))
	infos = soup.select('.c_infos')
	company_intro1 = ''.join((i.text for i in infos[0].select('b'))).replace('&nbsp;', ' ')
	company_intro = '\n'.join((i.text for i in infos))[len(company_intro1) + 1:]
	
	# 图片列表
	imgs = [down_img(i.attrs['src']) for i in soup.select('.Policy > ul:nth-of-type(1) > li > a > img')]
	
	# 联系人信息列表
	dd=soup.select('.cc_c > dl dd')
	if len(dd) == 6:
		company_address,telephone,netaddress,fax,person,email = [i.text for i in dd]

	jianxie_control = soup.select('h1.brand_t')
	if jianxie_control:
		jianxie = jianxie_control[0].text

	return (company_name,main_product,company_address,logo_path,
		company_intro,person,telephone,netaddress,fax,email, imgs, jianxie)

def get_all_product_from_page(soup):
	"下载产品"
	for li in soup.select('ul.mclist > li'): # ul.mclist > li > a
		url = ''.join([a.attrs['href'] for a in li.select('a')])
		title = ''.join([p.text for p in li.select('a > p')])
		yield (url, title)

def main():
	# recode_all_cate() #下载所有目录
	# down_load_dir()  # 下载所有的文件

	for pid,ptitle,purl in NetAddress(id=16, status=1).get_child_address(): #一级目录
		for cid,ctitle,curl in NetAddress(id=pid, status=1).get_child_address(): #二级目录
			for id,title, url in NetAddress(id=cid).get_child_address(): #子文件
				t1 = time()

				data = operate_page(id,title, url + 'intro/', get_company_from_page)
				imgs = data[10]
				CompanyInfo(pageid=id,introduction=data[4],mainproducts=data[1], compaddress=data[2],other=data[11]
				,logopath=data[3],telephone=data[6], fax=data[8], netaddress=data[7],email=data[9],person=data[5]).save() #保存数据

				for p in imgs:
					Certificate(pageid = id,name = '企业风采',imgpath = p).save()

				NetAddress(id=id, status=1).update_status()#修改状态

				for ppurl,pptitle in operate_page(id,title, url + 'products/', get_all_product_from_page):
					Case(pageid=id, name=pptitle, url=ppurl).save()

				print(time() - t1, url, title)


if __name__ == '__main__':
	main()
