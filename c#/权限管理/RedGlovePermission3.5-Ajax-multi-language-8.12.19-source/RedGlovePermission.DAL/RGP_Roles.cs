using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using RedGlovePermission.DBUtility;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.SQLServerDAL
{
	/// <summary>
    /// 角色管理数据访问类
	/// </summary>
	public class RGP_Roles:IRGP_Roles
	{
		public RGP_Roles()
		{ }

        #region 角色管理

        /// <summary>
        /// 判断角色是否存在
        /// </summary>
        /// <param name="RoleName">角色名称</param>
        /// <param name="RoleGroupID">角色分组ID</param>
        /// <returns></returns>
        public bool RoleExists(string RoleName,int RoleGroupID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from RGP_Roles");
            strSql.Append(" where RoleName=@RoleName and RoleGroupID=@RoleGroupID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleName", SqlDbType.NVarChar,30),
                    new SqlParameter("@RoleGroupID", SqlDbType.Int,4)};
            parameters[0].Value = RoleName;
            parameters[1].Value = RoleGroupID;

			return SqlServerHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="model">角色实体类</param>
        /// <returns></returns>
		public int CreateRole(RedGlovePermission.Model.RGP_Roles model)
        {
            int ret = 0;
            if (!RoleExists(model.RoleName, model.RoleGroupID))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into RGP_Roles(");
                strSql.Append("RoleGroupID,RoleName,RoleDescription)");
                strSql.Append(" values (");
                strSql.Append("@RoleGroupID,@RoleName,@RoleDescription)");
                SqlParameter[] parameters = {
					new SqlParameter("@RoleGroupID", SqlDbType.Int,4),
					new SqlParameter("@RoleName", SqlDbType.NVarChar,30),
					new SqlParameter("@RoleDescription", SqlDbType.NVarChar,50)};
                parameters[0].Value = model.RoleGroupID;
                parameters[1].Value = model.RoleName;
                parameters[2].Value = model.RoleDescription;

                if (SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
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
        /// 更新角色
        /// </summary>
        /// <param name="model">角色实体类</param>
        /// <returns></returns>
        public bool UpdateRole(RedGlovePermission.Model.RGP_Roles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update RGP_Roles set ");
            strSql.Append("RoleGroupID=@RoleGroupID,");
			strSql.Append("RoleName=@RoleName,");
			strSql.Append("RoleDescription=@RoleDescription");
			strSql.Append(" where RoleID=@RoleID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4),
					new SqlParameter("@RoleGroupID", SqlDbType.Int,4),
					new SqlParameter("@RoleName", SqlDbType.NVarChar,30),
					new SqlParameter("@RoleDescription", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.RoleID;
            parameters[1].Value = model.RoleGroupID;
			parameters[2].Value = model.RoleName;
			parameters[3].Value = model.RoleDescription;

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
        /// 删除角色
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns></returns>
        public int DeleteRole(int RoleID)
		{	
            int ret=0;
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)};
			parameters[0].Value = RoleID;

            string strSql1 = "Select userid from Users where RoleID=@RoleID";
            
            //删除角色的同时删除相关的权限
            ArrayList list = new ArrayList();
            list.Add("delete RGP_RoleAuthorityList where RoleID=" + RoleID.ToString());
            list.Add("delete RGP_Roles where RoleID=" + RoleID.ToString());            

            if (!SqlServerHelper.Exists(strSql1, parameters))
            {
                try
                {
                    SqlServerHelper.ExecuteSqlTran(list);
                    ret = 1;
                }
                catch
                {
                    ;
                }
            }
            else
            {
                ret = 2;
            }
            return ret;
		}

        /// <summary>
        /// 获取角色实体
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns></returns>
        public RedGlovePermission.Model.RGP_Roles GetRoleModel(int RoleID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select top 1 * from RGP_Roles ");
			strSql.Append(" where RoleID=@RoleID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)};
			parameters[0].Value = RoleID;

			RedGlovePermission.Model.RGP_Roles model=new RedGlovePermission.Model.RGP_Roles();
			DataSet ds=SqlServerHelper.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["RoleID"].ToString()!="")
				{
					model.RoleID=int.Parse(ds.Tables[0].Rows[0]["RoleID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["RoleRoleID"].ToString()!="")
				{
                    model.RoleGroupID = int.Parse(ds.Tables[0].Rows[0]["RoleGroupID"].ToString());
				}
				model.RoleName=ds.Tables[0].Rows[0]["RoleName"].ToString();
				model.RoleDescription=ds.Tables[0].Rows[0]["RoleDescription"].ToString();
				return model;
			}
			else
			{
				return null;
			}
		}

        /// <summary>
        /// 获得角色数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetRoleList(string strWhere,string strOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM RGP_Roles ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (strOrder.Trim() != "")
            {
                strSql.Append(" " + strOrder);
            }

            return SqlServerHelper.Query(strSql.ToString());
        }

        #endregion

        #region 角色授权

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        public bool RoleAuthorityExists(RedGlovePermission.Model.RGP_RoleAuthorityList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from RGP_RoleAuthorityList where ");
            if (model.UserID != 0)//判断是角色权限还是用户权限
            { strSql.Append("UserID=@UserID"); }
            else
            { strSql.Append("RoleID=@RoleID"); }
            strSql.Append(" and ModuleID=@ModuleID and AuthorityTag=@AuthorityTag");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@RoleID", SqlDbType.Int,4),
					new SqlParameter("@ModuleID", SqlDbType.Int,4),
					new SqlParameter("@AuthorityTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.RoleID;
            parameters[2].Value = model.ModuleID;
            parameters[3].Value = model.AuthorityTag;

            return SqlServerHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 修改角色模块权限
        /// </summary>
        public bool UpdateRoleAuthority(ArrayList list)
        {
            ArrayList AuthorityList = new ArrayList();

            for (int i = 0; i < list.Count; i++)
            {
                string[] val = list[i].ToString().Split('|');
                if (val[3] == "0")//如果为0就删除权限
                {
                    AuthorityList.Add("delete RGP_RoleAuthorityList where RoleID=" + val[0] + " and ModuleID=" + val[1] + " and AuthorityTag='" + val[2] + "'");
                }
                else
                {
                    AuthorityList.Add("insert into RGP_RoleAuthorityList(RoleID,ModuleID,AuthorityTag) values (" + val[0] + "," + val[1] + ",'" + val[2] + "')");
                }
            }

            try
            {
                SqlServerHelper.ExecuteSqlTran(AuthorityList);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改用户模块权限
        /// </summary>
        public bool UpdateUserAuthority(ArrayList list)
        {
            ArrayList AuthorityList = new ArrayList();

            for (int i = 0; i < list.Count; i++)
            {
                string[] val = list[i].ToString().Split('|');
                if (val[3] == "0")//如果为0就删除权限
                {
                    AuthorityList.Add("delete RGP_RoleAuthorityList where UserID=" + val[0] + " ModuleID=" + val[1] + " and AuthorityTag='" + val[2] + "'");
                }
                else
                {
                    AuthorityList.Add("insert into RGP_RoleAuthorityList(UserID,ModuleID,AuthorityTag) values (" + val[0] + "," + val[1] + ",'" + val[2] + "')");
                }
            }

            try
            {
                SqlServerHelper.ExecuteSqlTran(AuthorityList);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取角色的模块权限
        /// </summary>
        public DataSet GetRoleAuthorityList(int RoleID, int ModuleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM RGP_RoleAuthorityList where RoleID=" + RoleID.ToString() + " and ModuleID=" + ModuleID.ToString());

            return SqlServerHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 读取用户的模块权限
        /// </summary>
        public DataSet GetUserAuthorityList(int UserID, int ModuleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM RGP_RoleAuthorityList where UserID=" + UserID.ToString() + " and ModuleID=" + ModuleID.ToString());

            return SqlServerHelper.Query(strSql.ToString());
        }

        #endregion
    }
}

