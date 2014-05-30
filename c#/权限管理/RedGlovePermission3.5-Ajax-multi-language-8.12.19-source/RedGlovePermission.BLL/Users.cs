using System;
using System.Data;
using System.Collections.Generic;

using RedGlovePermission.Lib;
using RedGlovePermission.Model;
using RedGlovePermission.DALFactory;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.BLL
{
	/// <summary>
	/// 业务逻辑类Users 的摘要说明。
	/// </summary>
	public class Users
	{
        private readonly IUsers dal = DataAccess.CreateUsers();
		public Users()
		{}

        /// <summary>
        /// 用户是否存在
        /// </summary>
        public bool UserExists(string UserName)
        {
            return dal.UserExists(UserName);
        }

        /// <summary>
        /// 创建一个新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateUser(RedGlovePermission.Model.Users model)
        {
            return dal.CreateUser(model);
        }

        /// <summary>
        /// 用户登录检测
        /// </summary>
        public bool CheckLogin(string UserName,string pwd)
        {
            return dal.CheckLogin(UserName, pwd);
        }

        /// <summary>
        /// 更新用户登录时间
        /// </summary>
        /// <param name="id"></param>
        public void UpdateLoginTime(int id)
        {
            dal.UpdateLoginTime(id);
        }

        /// <summary>
        /// 判断用户原密码是否正确
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="pwd">原密码</param>
        /// <returns></returns>
        public bool VerifyPassword(int id, string pwd)
        {
            return dal.VerifyPassword(id, pwd);
        }

        /// <summary>
        /// 更改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        public bool ChangePassword(int id, string pwd)
        {
            return dal.ChangePassword(id, pwd);
        }

        /// <summary>
        /// 更新安全信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="question">问题</param>
        /// <param name="answer">答案</param>
        /// <returns></returns>
        public bool ChangeSecureInfo(int id, string question, string answer)
        {
            return dal.ChangeSecureInfo(id, question, answer);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="UserID">用户ID</param>
        public bool DeleteUser(int UserID)
        {
            return dal.DeleteUser(UserID);
        }

        /// <summary>
        /// 根据ID得到用户对象实体
        /// </summary>
        public RedGlovePermission.Model.Users GetUserModel(int UserID)
        {
            return dal.GetUserModel(UserID);
        }

        /// <summary>
        /// 根据用户名得到用户对象实体
        /// </summary>
        public RedGlovePermission.Model.Users GetUserModel(string UserName)
        {
            return dal.GetUserModel(UserName);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetUserList(string strWhere,string strOrder)
        {
            return dal.GetUserList(strWhere,strOrder);
        }
	}
}

