using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using RedGlovePermission.DBUtility;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.SQLServerDAL
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
            strSql.Append(" where UserName=@UserName ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,128)};
            parameters[0].Value = UserName;

			return SqlServerHelper.Exists(strSql.ToString(),parameters);
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
                strSql.Append("@UserName,@Password,@Question,@Answer,@RoleID,@UserGroup,@CreateTime,@IsLimit)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,128),
					new SqlParameter("@Password", SqlDbType.NVarChar,128),
					new SqlParameter("@Question", SqlDbType.NVarChar,100),
					new SqlParameter("@Answer", SqlDbType.NVarChar,100),
					new SqlParameter("@RoleID", SqlDbType.Int,4),
					new SqlParameter("@UserGroup", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@IsLimit", SqlDbType.Bit,1)};
                parameters[0].Value = model.UserName;
                parameters[1].Value = model.Password;
                parameters[2].Value = model.Question;
                parameters[3].Value = model.Answer;
                parameters[4].Value = model.RoleID;
                parameters[5].Value = model.UserGroup;
                parameters[6].Value = DateTime.Now;
                parameters[7].Value = model.IsLimit;


                object obj = SqlServerHelper.GetSingle(strSql.ToString(), parameters);
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
            strSql.Append(" where UserName=@UserName and Password=@Password");
            SqlParameter[] parameters = {
                     new SqlParameter("@UserName", SqlDbType.NVarChar,128),
					 new SqlParameter("@Password", SqlDbType.NVarChar,128)};
            parameters[0].Value = UserName;
            parameters[1].Value = pwd;

            return SqlServerHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户登录时间
        /// </summary>
        /// <param name="id"></param>
        public void UpdateLoginTime(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Users set LastLoginTime=@LastLoginTime where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LastLoginTime", SqlDbType.DateTime)};
            parameters[0].Value = id;
            parameters[1].Value = DateTime.Now;

            SqlServerHelper.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append(" where UserID=@UserID and Password=@Password");
            SqlParameter[] parameters = {
                     new SqlParameter("@UserID", SqlDbType.Int,4),
					 new SqlParameter("@Password", SqlDbType.NVarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = pwd;

            return SqlServerHelper.Exists(strSql.ToString(), parameters);
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
            strSql.Append("Password=@Password");
            strSql.Append(" where UserID=@UserID");

            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Password", SqlDbType.NVarChar,128)};
            parameters[0].Value = id;
            parameters[1].Value = pwd;

            if (SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
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
            strSql.Append("Question=@Question,Answer=@Answer");
            strSql.Append(" where UserID=@UserID");

            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Question", SqlDbType.NVarChar,100),
					new SqlParameter("@Answer", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;
            parameters[1].Value = question;
            parameters[2].Value = answer;

            if (SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
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
			strSql.Append("delete Users ");
			strSql.Append(" where UserID=@UserID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;

            if (SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
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
			strSql.Append("select  top 1 * from Users ");
			strSql.Append(" where UserID=@UserID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;

			RedGlovePermission.Model.Users model=new RedGlovePermission.Model.Users();
			DataSet ds=SqlServerHelper.Query(strSql.ToString(),parameters);
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
            strSql.Append("select  top 1 UserID,UserName,Password,Question,Answer,RoleID,UserGroup,CreateTime,LastLoginTime,Status,IsOnline,IsLimit from Users ");
            strSql.Append(" where UserName=@UserName ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,128)};
            parameters[0].Value = UserName;

            RedGlovePermission.Model.Users model = new RedGlovePermission.Model.Users();
            DataSet ds = SqlServerHelper.Query(strSql.ToString(), parameters);
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
            strSql.Append("SELECT dbo.Users.UserID, dbo.Users.UserName, dbo.Users.Password, dbo.Users.Question, dbo.Users.Answer,"); 
            strSql.Append("dbo.Users.RoleID, dbo.Users.UserGroup,dbo.Users.CreateTime, dbo.Users.LastLoginTime, dbo.Users.Status,");
            strSql.Append("dbo.Users.IsOnline, dbo.Users.IsLimit, dbo.RGP_Groups.GroupName, dbo.RGP_Roles.RoleName "); 
            strSql.Append("FROM dbo.Users INNER JOIN "); 
            strSql.Append("dbo.RGP_Roles ON dbo.Users.RoleID = dbo.RGP_Roles.RoleID INNER JOIN ");
            strSql.Append("dbo.RGP_Groups ON dbo.Users.UserGroup = dbo.RGP_Groups.GroupID"); 
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}

            if (strOrder.Trim() != "")
            {
                strSql.Append(" " + strOrder);
            }
			return SqlServerHelper.Query(strSql.ToString());
		}
	}
}

