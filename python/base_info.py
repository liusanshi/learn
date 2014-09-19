from DB import MSSQL
from selenium import webdriver
from selenium.common.exceptions import NoSuchElementException

driver = webdriver.PhantomJS('E:\\phantomjs-1.9.7-windows\\phantomjs.exe')
logpath = 'd:\\log\\log.txt'

def get_company_about(id, url):
    register_money = ''#注册资本
    established_time = ''#成立时间
    register_area = ''#注册地
    corporation = '' #法人

    driver.get(url + 'about/')

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

    return (register_money, established_time, register_area, corporation, id)

def modify_comp_info(ms, info, url):
#将数据保存至数据库 register_money, established_time, register_area, corporation
    try:
        ms.ExecNonQueryWithParam('update companyinfo set [register_money]=%s, [established_time]=%s, [register_area]=%s, [corporation]=%s where id = %d', info)
    except Exception as Argument:
        log(Argument, info)

def log(exp, data):
#记录日志
    fp = open(logpath, 'a+', encoding='utf-8')
    fp.write(repr(exp))
    fp.write('\r\n')
    fp.write(repr(data))
    fp.write('\r\n')
    fp.write('================================================')
    fp.write('\r\n')
    fp.close()

def main():
## ms = MSSQL(host="localhost",user="sa",pwd="123456",db="PythonWeiboStatistics")
## #返回的是一个包含tuple的list，list的元素是记录行，tuple的元素是每行记录的字段
## ms.ExecNonQuery("insert into WeiBoUser values('2','3')")

    ms = MSSQL(host="localhost",user="sa",pwd="sa",db="JZData")
    while True:
        resList = ms.ExecQuery("select a.id,b.Url from companyinfo a inner join NetAddress b on a.pageid=b.id where register_money is null ")
        if len(resList) == 0 : break
        for (id, url) in resList:
            info = get_company_about(id, url)
            #print(url)
            print(info)
            modify_comp_info(ms, info, url)
    else:
        os.popen('shutdown -s -t 0')#关机
        

if __name__ == '__main__':
    main()