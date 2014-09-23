#coding=utf-8 
#!/usr/bin/env python
#-------------------------------------------------------------------------------
# Name: pymssqlTest.py
# Purpose: 测试 pymssql库，该库到这里下载：http://www.lfd.uci.edu/~gohlke/pythonlibs/#pymssql
#
# Author: scott
#
# Created: 04/02/2012
#-------------------------------------------------------------------------------

import pymssql

class MSSQL:
    """
    对pymssql的简单封装
    pymssql库，该库到这里下载：http://www.lfd.uci.edu/~gohlke/pythonlibs/#pymssql
    使用该库时，需要在Sql Server Configuration Manager里面将TCP/IP协议开启

    用法：

    """

    def __init__(self,host,user,pwd,db):
        self.host = host
        self.user = user
        self.pwd = pwd
        self.db = db

    def __GetConnect(self):
        """
        得到连接信息
        返回: conn.cursor()
        """
        if not self.db:
            raise(NameError,"没有设置数据库信息")
        self.conn = pymssql.connect(host=self.host,user=self.user,password=self.pwd,database=self.db,charset="utf8")
        cur = self.conn.cursor()
        if not cur:
            raise(NameError,"连接数据库失败")
        else:
            return cur

    def ExecQuery(self,sql):
        """
        执行查询语句
        返回的是一个包含tuple的list，list的元素是记录行，tuple的元素是每行记录的字段

        调用示例：
                ms = MSSQL(host="localhost",user="sa",pwd="123456",db="PythonWeiboStatistics")
                resList = ms.ExecQuery("SELECT id,NickName FROM WeiBoUser")
                for (id,NickName) in resList:
                    print str(id),NickName
        """
        cur = self.__GetConnect()
        cur.execute(sql)
        resList = cur.fetchall()

        #查询完毕后必须关闭连接
        self.conn.close()
        return resList

    def ExecNonQuery(self,sql):
        """
        执行非查询语句

        调用示例：
            cur = self.__GetConnect()
            cur.execute(sql)
            self.conn.commit()
            self.conn.close()
        """
        cur = self.__GetConnect()
        cur.execute(sql)
        self.conn.commit()
        self.conn.close()

    def ExecNonQueryWithParam(self, sql, param):
        """
        支持参数化查询
        """

        cur = self.__GetConnect()
        cur.execute(sql, param)
        self.conn.commit()
        self.conn.close()

    def ExecQueryWithParam(this, sql, param):
        "参数化查询"
        cur = self.__GetConnect()
        cur.execute(sql, param)
        resList = cur.fetchall()
        self.conn.close()
        return resList

    def ExecScalar(self, sql, param):
        "参数化查询,返回第一行第一列"
        cur = self.__GetConnect()
        cur.execute(sql, param)
        # res = cur.fetchall()
        result = None
        for row in cur:
            result = row[0]
        self.conn.commit()
        self.conn.close()
        return result
