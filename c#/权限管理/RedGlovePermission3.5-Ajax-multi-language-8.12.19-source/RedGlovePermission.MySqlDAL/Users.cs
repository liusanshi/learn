using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.MySqlDAL
{
	/// <summary>
	/// 数据访问类Users。
	/// </summary>
	public class Users:IUsers
	{
		public Users()
		{}

		/// <summary>
		/// 用户是否存在该
		/// </summary>
        public bool UserExists(string UserName)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Users");
            strSql.Append(" where UserName=?UserName ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,128)};
            parameters[0].Value = UserName;

			return RedGlovePermission.DBUtility.MySqlHelper.Exists(strSql.ToString(),parameters);
		}
        
		/// <summary>
        /// 创建一个新用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int CreateUser(RedGlovePermission.Model.Users model)
        {
            int ret = 0;
            if (!UserExists(model.UserName))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Users(");
                strSql.Append("UserName,Password,Question,Answer,RoleID,UserGroup,CreateTime,IsLimit)");
                strSql.Append(" values (");
                strSql.Append("?UserName,?Password,?Question,?Answer,?RoleID,?UserGroup,?CreateTime,?IsLimit)");
                strSql.Append(";select LAST_INSERT_ID()");
                MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,128),
					new MySqlParameter("?Password", MySqlDbType.VarChar,128),
					new MySqlParameter("?Question", MySqlDbType.VarChar,100),
					new MySqlParameter("?Answer", MySqlDbType.VarChar,100),
					new MySqlParameter("?RoleID", MySqlDbType.Int32,11),
					new MySqlParameter("?UserGroup", MySqlDbType.Int32,11),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?IsLimit", MySqlDbType.Bit,1)};
                parameters[0].Value = model.UserName;
                parameters[1].Value = model.Password;
                parameters[2].Value = model.Question;
                parameters[3].Value = model.Answer;
                parameters[4].Value = model.RoleID;
                parameters[5].Value = model.UserGroup;
                parameters[6].Value = DateTime.Now;
                parameters[7].Value = model.IsLimit;


                object obj = RedGlovePermission.DBUtility.MySqlHelper.GetSingle(strSql.ToString(), parameters);
                if (obj != null)
                {
                    ret = 1;
                }               
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        /// <summary>
        /// 用户登录检测
        /// </summary>
        public bool CheckLogin(string UserName, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Users");
            strSql.Append(" where UserName=?UserName and Password=?Password");
            MySqlParameter[] parameters = {
                     new MySqlParameter("?UserName", MySqlDbType.VarChar,128),
					 new MySqlParameter("?Password", MySqlDbType.VarChar,128)};
            parameters[0].Value = UserName;
            parameters[1].Value = pwd;

            return RedGlovePermission.DBUtility.MySqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// 更新用户登录时间
        /// </summary>
        /// <param name="id"></param>
        public void UpdateLoginTime(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("LastLoginTime=?LastLoginTime");
            strSql.Append(" where UserID=?UserID");

            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int32,11),
					new MySqlParameter("?LastLoginTime", MySqlDbType.Datetime)};
            parameters[0].Value = id;
            parameters[1].Value = DateTime.Now;

            RedGlovePermission.DBUtility.MySqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 判断用户原密码是否正确
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="pwd">原密码</param>
        /// <returns></returns>
        public bool VerifyPassword(int id, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Users");
            strSql.Append(" where UserID=?UserID and Password=?Password");
            MySqlParameter[] parameters = {
                     new MySqlParameter("?UserID", MySqlDbType.Int32,11),
					 new MySqlParameter("?Password", MySqlDbType.VarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = pwd;

            return RedGlovePermission.DBUtility.MySqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        public bool ChangePassword(int id, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("Password=?Password");
            strSql.Append(" where UserID=?UserID");

            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int32,11),
					new MySqlParameter("?Password", MySqlDbType.VarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = pwd;

            if (RedGlovePermission.DBUtility.MySqlHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("Question=?Question,Answer=?Answer");
            strSql.Append(" where UserID=?UserID");

            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int32,11),
					new MySqlParameter("?Question", MySqlDbType.VarChar,100),
					new MySqlParameter("?Answer", MySqlDbType.VarChar,100)};
            parameters[0].Value = id;
            parameters[1].Value = question;
            parameters[2].Value = answer;

            if (RedGlovePermission.DBUtility.MySqlHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// 删除用户
		/// </summary>
		/// <param name="UserID">用户ID</param>
		public bool DeleteUser(int UserID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Users ");
			strSql.Append(" where UserID=?UserID ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int32,11)};
			parameters[0].Value = UserID;

            if (RedGlovePermission.DBUtility.MySqlHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
		}

		/// <summary>
		/// 根据ID得到用户对象实体
		/// </summary>
		public RedGlovePermission.Model.Users GetUserModel(int UserID)
		{			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from Users ");
			strSql.Append(" where UserID=?UserID ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int32,11)};
			parameters[0].Value = UserID;

			RedGlovePermission.Model.Users model=new RedGlovePermission.Model.Users();
			DataSet ds=RedGlovePermission.DBUtility.MySqlHelper.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
				}
				model.UserName=ds.Tables[0].Rows[0]["UserName"].ToString();
				model.Password=ds.Tables[0].Rows[0]["Password"].ToString();
				model.Question=ds.Tables[0].Rows[0]["Question"].ToString();
				model.Answer=ds.Tables[0].Rows[0]["Answer"].ToString();
				if(ds.Tables[0].Rows[0]["RoleID"].ToString()!="")
				{
					model.RoleID=int.Parse(ds.Tables[0].Rows[0]["RoleID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["UserGroup"].ToString()!="")
				{
					model.UserGroup=int.Parse(ds.Tables[0].Rows[0]["UserGroup"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LastLoginTime"].ToString()!="")
				{
					model.LastLoginTime=DateTime.Parse(ds.Tables[0].Rows[0]["LastLoginTime"].ToString());
				}
                if (ds.Tables[0].Rows[0]["Status"].ToString() != "")
                {
                    model.Status = int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
                }				
				if(ds.Tables[0].Rows[0]["IsOnline"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsOnline"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsOnline"].ToString().ToLower()=="true"))
					{
						model.IsOnline=true;
					}
					else
					{
						model.IsOnline=false;
					}
				}
				if(ds.Tables[0].Rows[0]["IsLimit"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsLimit"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsLimit"].ToString().ToLower()=="true"))
					{
						model.IsLimit=true;
					}
					else
					{
						model.IsLimit=false;
					}
				}
				return model;
			}
			else
			{
				return null;
			}
		}

        /// <summary>
        /// 根据用户名得到用户对象实体
        /// </summary>
        public RedGlovePermission.Model.Users GetUserModel(string UserName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Users ");
            strSql.Append(" where UserName=?UserName ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,128)};
            parameters[0].Value = UserName;

            RedGlovePermission.Model.Users model = new RedGlovePermission.Model.Users();
            DataSet ds = RedGlovePermission.DBUtility.MySqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                model.Question = ds.Tables[0].Rows[0]["Question"].ToString();
                model.Answer = ds.Tables[0].Rows[0]["Answer"].ToString();
                if (ds.Tables[0].Rows[0]["RoleID"].ToString() != "")
                {
                    model.RoleID = int.Parse(ds.Tables[0].Rows[0]["RoleID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserGroup"].ToString() != "")
                {
                    model.UserGroup = int.Parse(ds.Tables[0].Rows[0]["UserGroup"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LastLoginTime"].ToString() != "")
                {
                    model.LastLoginTime = DateTime.Parse(ds.Tables[0].Rows[0]["LastLoginTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Status"].ToString() != "")
                {
                    model.Status = int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsOnline"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsOnline"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsOnline"].ToString().ToLower() == "true"))
                    {
                        model.IsOnline = true;
                    }
                    else
                    {
                        model.IsOnline = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["IsLimit"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsLimit"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsLimit"].ToString().ToLower() == "true"))
                    {
                        model.IsLimit = true;
                    }
                    else
                    {
                        model.IsLimit = false;
                    }
                }
                return model;
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetUserList(string strWhere,string strOrder)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("SELECT ");
            strSql.Append("users.UserID,");
            strSql.Append("users.UserName,");
            strSql.Append("users.Password,");
            strSql.Append("users.Question,");
            strSql.Append("users.Answer,");
            strSql.Append("users.RoleID,");
            strSql.Append("users.UserGroup,");
            strSql.Append("users.CreateTime,");
            strSql.Append("users.LastLoginTime,");
            strSql.Append("users.`Status`,");
            strSql.Append("users.IsOnline,");
            strSql.Append("users.IsLimit,");
            strSql.Append("rgp_roles.RoleName,");
            strSql.Append("rgp_groups.GroupName");
            strSql.Append(" FROM ");
            strSql.Append("rgp_roles ");
            strSql.Append("INNER JOIN users ON (rgp_roles.RoleID = users.RoleID) ");
            strSql.Append("INNER JOIN rgp_groups ON (users.UserGroup = rgp_groups.GroupID)");

			if(strWhere.Trim()!="")
			{
                strSql.Append(" where " + strWhere.Replace("dbo.", ""));
			}

            if (strOrder.Trim() != "")
            {
                strSql.Append(" " + strOrder.Replace("dbo.", ""));
            }
			return RedGlovePermission.DBUtility.MySqlHelper.Query(strSql.ToString());
		}
	}
}

