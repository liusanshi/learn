import urllib.request
from http import cookiejar

login_server = 'http://admin.b2bdev.com/Mycaigou/Business/Common/LoginService.Login.ajax'
cookieJar = cookiejar.CookieJar()
cookie = urllib.request.HTTPCookieProcessor(cookieJar)
opener = urllib.request.build_opener(cookie)
data = { "code": 'admin', "pwd": 'e10adc3949ba59abbe56e057f20f883e' }
data = urllib.parse.urlencode(data).encode()

req = urllib.request.Request(login_server, data)

result = opener.open(req)

print( result.read().decode())

oid=576
award_name='576'
file_id='576'

save_url = 'http://admin.b2bdev.com/Mycaigou/Business/SupplierManage/SupplierService.SaveAward.ajax'
# { "cdata": mini.encode(data) }
data = {"cdata": {'supplier_id': oid,'award_name':award_name,'award_file':file_id}}
print(urllib.parse.urlencode(data))
data = urllib.parse.urlencode(data).encode()
req = urllib.request.Request(save_url, data)

print(data)

#result = opener.open(req)

#print( result.read().decode())
