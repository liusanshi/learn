using System;
using System.Data;

namespace RedGlovePermission.IDAL
{
	/// <summary>
	/// 业务逻辑类Users 的摘要说明。
	/// </summary>
	public interface IUsers
	{		
        /// <summary>
        /// 用户是否存在
        /// </summary>
        bool UserExists(string UserName);

        /// <summary>
        /// 创建一个新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int CreateUser(RedGlovePermission.Model.Users model);
        
        /// <summary>
        /// 用户登录检测
        /// </summary>
        bool CheckLogin(string UserName,string pwd);

        /// <summary>
        /// 更新用户登录时间
        /// </summary>
        /// <param name="id"></param>
        void UpdateLoginTime(int id);

        /// <summary>
        /// 判断用户原密码是否正确
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="pwd">原密码</param>
        /// <returns></returns>
        bool VerifyPassword(int id, string pwd);
        
        /// <summary>
        /// 更改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        bool ChangePassword(int id, string pwd);
      
        /// <summary>
        /// 更新安全信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="question">问题</param>
        /// <param name="answer">答案</param>
        /// <returns></returns>
        bool ChangeSecureInfo(int id, string question, string answer);      

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="UserID">用户ID</param>
        bool DeleteUser(int UserID);
        
        /// <summary>
        /// 根据ID得到用户对象实体
        /// </summary>
        RedGlovePermission.Model.Users GetUserModel(int UserID);
       
        /// <summary>
        /// 根据用户名得到用户对象实体
        /// </summary>
        RedGlovePermission.Model.Users GetUserModel(string UserName);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetUserList(string strWhere, string strOrder);
	}
}

