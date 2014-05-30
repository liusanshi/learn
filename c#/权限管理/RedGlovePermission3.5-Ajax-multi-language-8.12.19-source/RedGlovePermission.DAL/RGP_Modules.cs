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
    /// 功能模块数据访问类
    /// </summary>
    public class RGP_Modules:IRGP_Modules
    {
        public RGP_Modules()
        { }

        #region 模块分类
        /// <summary>
        /// 判断模块分类是否存在
        /// </summary>
        /// <param name="ModuleTypeName">模块分类名称</param>
        /// <returns></returns>
        public bool ModuleTypeExists(string ModuleTypeName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from RGP_ModuleType");
            strSql.Append(" where ModuleTypeName=@ModuleTypeName ");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTypeName", SqlDbType.NVarChar,30)};
            parameters[0].Value = ModuleTypeName;

            return SqlServerHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一个模块分类
        /// </summary>
        /// <param name="model">模块分类实体类</param>
        /// <returns></returns>
        public int CreateModuleType(RedGlovePermission.Model.RGP_ModuleType model)
        {
            int ret = 0;
            if (!ModuleTypeExists(model.ModuleTypeName))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into RGP_ModuleType(");
                strSql.Append("ModuleTypeName,ModuleTypeOrder,ModuleTypeDescription)");
                strSql.Append(" values (");
                strSql.Append("@ModuleTypeName,@ModuleTypeOrder,@ModuleTypeDescription)");
                SqlParameter[] parameters = {
					new SqlParameter("@ModuleTypeName", SqlDbType.NVarChar,30),
					new SqlParameter("@ModuleTypeOrder", SqlDbType.Int,4),
					new SqlParameter("@ModuleTypeDescription", SqlDbType.NVarChar,50)};
                parameters[0].Value = model.ModuleTypeName;
                parameters[1].Value = model.ModuleTypeOrder;
                parameters[2].Value = model.ModuleTypeDescription;

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
        /// 更新一条数据
        /// </summary>
        /// <param name="model">模块分类实体类</param>
        /// <returns></returns>
        public bool UpdateModuleType(RedGlovePermission.Model.RGP_ModuleType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RGP_ModuleType set ");
            strSql.Append("ModuleTypeName=@ModuleTypeName,");
            strSql.Append("ModuleTypeOrder=@ModuleTypeOrder,");
            strSql.Append("ModuleTypeDescription=@ModuleTypeDescription");
            strSql.Append(" where ModuleTypeID=@ModuleTypeID");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTypeID", SqlDbType.Int,4),
					new SqlParameter("@ModuleTypeName", SqlDbType.NVarChar,30),
					new SqlParameter("@ModuleTypeOrder", SqlDbType.Int,4),
					new SqlParameter("@ModuleTypeDescription", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ModuleTypeID;
            parameters[1].Value = model.ModuleTypeName;
            parameters[2].Value = model.ModuleTypeOrder;
            parameters[3].Value = model.ModuleTypeDescription;

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
        /// 删除模块分类
        /// </summary>
        /// <param name="ModuleTypeID">模块分类ID</param>
        /// <returns></returns>
        public int DeleteModuleType(int ModuleTypeID)
        {
            int ret = 0;
            string strSql1 = "Select ModuleID from RGP_Modules where ModuleTypeID=@ModuleTypeID";
            string strSql2 = "delete RGP_ModuleType where ModuleTypeID=@ModuleTypeID";

            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTypeID", SqlDbType.Int,4)};
            parameters[0].Value = ModuleTypeID;

            if (!SqlServerHelper.Exists(strSql1, parameters))
            {
                if (SqlServerHelper.ExecuteSql(strSql2, parameters) >= 1) { ret = 1; }
            }
            else
            {
                ret = 2;
            }
            return ret;
        }

        /// <summary>
        /// 得到一个模块分类实体
        /// </summary>
        /// <param name="ModuleTypeID">模块分类ID</param>
        /// <returns></returns>
        public RedGlovePermission.Model.RGP_ModuleType GetModuleTypeModel(int ModuleTypeID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ModuleTypeID,ModuleTypeName,ModuleTypeOrder,ModuleTypeDescription from RGP_ModuleType ");
            strSql.Append(" where ModuleTypeID=@ModuleTypeID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTypeID", SqlDbType.Int,4)};
            parameters[0].Value = ModuleTypeID;

            RedGlovePermission.Model.RGP_ModuleType model = new RedGlovePermission.Model.RGP_ModuleType();
            DataSet ds = SqlServerHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ModuleTypeID"].ToString() != "")
                {
                    model.ModuleTypeID = int.Parse(ds.Tables[0].Rows[0]["ModuleTypeID"].ToString());
                }
                model.ModuleTypeName = ds.Tables[0].Rows[0]["ModuleTypeName"].ToString();
                if (ds.Tables[0].Rows[0]["ModuleTypeOrder"].ToString() != "")
                {
                    model.ModuleTypeOrder = int.Parse(ds.Tables[0].Rows[0]["ModuleTypeOrder"].ToString());
                }
                model.ModuleTypeDescription = ds.Tables[0].Rows[0]["ModuleTypeDescription"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得模块分类数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetModuleTypeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM RGP_ModuleType ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by ModuleTypeOrder asc");

            return SqlServerHelper.Query(strSql.ToString());
        }

        #endregion

        #region 模块操作
        /// <summary>
        /// 判断模块是否存在
        /// </summary>
        /// <param name="ModuleName">模块名称</param>
        /// <returns></returns>
        public bool ModuleExists(string ModuleTag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from RGP_Modules");
            strSql.Append(" where ModuleTag=@ModuleTag");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = ModuleTag;

            return SqlServerHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新时判断模块是否存在
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <param name="ModuleName">模块名称</param>
        /// <returns></returns>
        public bool UpdateExists(int ModuleID, string ModuleTag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from RGP_Modules");
            strSql.Append(" where ModuleID<>@ModuleID and ModuleTag=@ModuleTag");
            SqlParameter[] parameters = {
                    new SqlParameter("@ModuleID", SqlDbType.Int,4),
					new SqlParameter("@ModuleTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = ModuleID;
            parameters[1].Value = ModuleTag;

            return SqlServerHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一个模块
        /// </summary>
        /// <param name="model">模块实体类</param>
        /// <returns></returns>
        public int CreateModule(RedGlovePermission.Model.RGP_Modules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into RGP_Modules(");
            strSql.Append("ModuleTypeID,ModuleName,ModuleTag,ModuleURL,ModuleDisabled,ModuleOrder,ModuleDescription,IsMenu)");
            strSql.Append(" values (");
            strSql.Append("@ModuleTypeID,@ModuleName,@ModuleTag,@ModuleURL,@ModuleDisabled,@ModuleOrder,@ModuleDescription,@IsMenu)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTypeID", SqlDbType.Int,4),
					new SqlParameter("@ModuleName", SqlDbType.NVarChar,30),
					new SqlParameter("@ModuleTag", SqlDbType.NVarChar,50),
					new SqlParameter("@ModuleURL", SqlDbType.NVarChar,500),
					new SqlParameter("@ModuleDisabled", SqlDbType.Bit,1),
					new SqlParameter("@ModuleOrder", SqlDbType.Int,4),
					new SqlParameter("@ModuleDescription", SqlDbType.NVarChar,50),
                    new SqlParameter("@IsMenu", SqlDbType.Bit,1)};
            parameters[0].Value = model.ModuleTypeID;
            parameters[1].Value = model.ModuleName;
            parameters[2].Value = model.ModuleTag;
            parameters[3].Value = model.ModuleURL;
            parameters[4].Value = model.ModuleDisabled;
            parameters[5].Value = model.ModuleOrder;
            parameters[6].Value = model.ModuleDescription;
            parameters[7].Value = model.IsMenu;

            object obj = SqlServerHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">模块实体类</param>
        /// <returns></returns>
        public int UpdateModule(RedGlovePermission.Model.RGP_Modules model)
        {
            int ret = 0;
            if (!UpdateExists(model.ModuleID, model.ModuleTag))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update RGP_Modules set ");
                strSql.Append("ModuleTypeID=@ModuleTypeID,");
                strSql.Append("ModuleName=@ModuleName,");
                strSql.Append("ModuleTag=@ModuleTag,");
                strSql.Append("ModuleURL=@ModuleURL,");
                strSql.Append("ModuleDisabled=@ModuleDisabled,");
                strSql.Append("ModuleOrder=@ModuleOrder,");
                strSql.Append("ModuleDescription=@ModuleDescription");
                strSql.Append(" where ModuleID=@ModuleID ");
                SqlParameter[] parameters = {
					new SqlParameter("@ModuleID", SqlDbType.Int,4),
					new SqlParameter("@ModuleTypeID", SqlDbType.Int,4),
					new SqlParameter("@ModuleName", SqlDbType.NVarChar,30),
					new SqlParameter("@ModuleTag", SqlDbType.NVarChar,50),
					new SqlParameter("@ModuleURL", SqlDbType.NVarChar,500),
					new SqlParameter("@ModuleDisabled", SqlDbType.Bit,1),
					new SqlParameter("@ModuleOrder", SqlDbType.Int,4),
					new SqlParameter("@ModuleDescription", SqlDbType.NVarChar,50)};
                parameters[0].Value = model.ModuleID;
                parameters[1].Value = model.ModuleTypeID;
                parameters[2].Value = model.ModuleName;
                parameters[3].Value = model.ModuleTag;
                parameters[4].Value = model.ModuleURL;
                parameters[5].Value = model.ModuleDisabled;
                parameters[6].Value = model.ModuleOrder;
                parameters[7].Value = model.ModuleDescription;

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
        /// 删除模块，以及相应的权限
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public bool DeleteModule(int ModuleID)
        {
            ArrayList List = new ArrayList();
            List.Add("delete RGP_Modules where ModuleID=" + ModuleID.ToString());
            List.Add("delete RGP_ModuleAuthorityList where ModuleID=" + ModuleID.ToString());
            try
            {
                SqlServerHelper.ExecuteSqlTran(List);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个模块实体
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public RedGlovePermission.Model.RGP_Modules GetModuleModel(int ModuleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from RGP_Modules ");
            strSql.Append(" where ModuleID=@ModuleID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleID", SqlDbType.Int,4)};
            parameters[0].Value = ModuleID;

            RedGlovePermission.Model.RGP_Modules model = new RedGlovePermission.Model.RGP_Modules();
            DataSet ds = SqlServerHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ModuleID"].ToString() != "")
                {
                    model.ModuleID = int.Parse(ds.Tables[0].Rows[0]["ModuleID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ModuleTypeID"].ToString() != "")
                {
                    model.ModuleTypeID = int.Parse(ds.Tables[0].Rows[0]["ModuleTypeID"].ToString());
                }
                model.ModuleName = ds.Tables[0].Rows[0]["ModuleName"].ToString();
                model.ModuleTag = ds.Tables[0].Rows[0]["ModuleTag"].ToString();
                model.ModuleURL = ds.Tables[0].Rows[0]["ModuleURL"].ToString();
                if (ds.Tables[0].Rows[0]["ModuleDisabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ModuleDisabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["ModuleDisabled"].ToString().ToLower() == "true"))
                    {
                        model.ModuleDisabled = true;
                    }
                    else
                    {
                        model.ModuleDisabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ModuleOrder"].ToString() != "")
                {
                    model.ModuleOrder = int.Parse(ds.Tables[0].Rows[0]["ModuleOrder"].ToString());
                }
                model.ModuleDescription = ds.Tables[0].Rows[0]["ModuleDescription"].ToString();

                if (ds.Tables[0].Rows[0]["IsMenu"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsMenu"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsMenu"].ToString().ToLower() == "true"))
                    {
                        model.IsMenu = true;
                    }
                    else
                    {
                        model.IsMenu = false;
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
        /// 获得模块数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetModuleList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM RGP_Modules ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            strSql.Append(" order by ModuleOrder asc");

            return SqlServerHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取模块ID
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        /// <returns></returns>
        public int GetModuleID(string ModuleTag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ModuleID from RGP_Modules where ModuleTag=@ModuleTag");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = ModuleTag;

            object obj = SqlServerHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 模块是否关闭
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        /// <returns></returns>
        public bool IsModule(string ModuleTag)
        {
            bool ret = false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ModuleDisabled from RGP_Modules where ModuleTag=@ModuleTag");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = ModuleTag;

            object obj = SqlServerHelper.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                if ((obj.ToString() == "1") || (obj.ToString().ToLower() == "true"))
                    ret = true;
            }
            return ret;
        }

        #endregion

        #region 模块授权

        /// <summary>
        /// 增加模块权限
        /// </summary>
        /// <param name="list">权限列表</param>
        /// <returns></returns>
        public bool CreateAuthorityList(ArrayList list)
        {
            ArrayList AuthorityList = new ArrayList();

            for (int i = 0; i < list.Count; i++)
            {
                string[] val = list[i].ToString().Split('|');
                AuthorityList.Add("insert into RGP_ModuleAuthorityList(ModuleID,AuthorityTag) values (" + val[0] + ",'" + val[1] + "')");
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
        /// 更新模块权限
        /// </summary>
        public bool UpdateAuthorityList(ArrayList list)
        {
            ArrayList AuthorityList = new ArrayList();

            for (int i = 0; i < list.Count; i++)
            {
                string[] val = list[i].ToString().Split('|');
                if (val[2] == "0")//如果为0就删除权限
                {
                    //删除模块权限
                    AuthorityList.Add("delete RGP_ModuleAuthorityList where ModuleID=" + val[0] + " and AuthorityTag='" + val[1] + "'");
                    //删除角色所对应该模块标识的权限
                    AuthorityList.Add("delete RGP_RoleAuthorityList where ModuleID=" + val[0] + " and AuthorityTag='" + val[1] + "'");
                }
                else
                {
                    AuthorityList.Add("insert into RGP_ModuleAuthorityList(ModuleID,AuthorityTag) values (" + val[0] + ",'" + val[1] + "')");
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
        /// 删除指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public bool DeleteAuthority(int ModuleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete RGP_ModuleAuthorityList ");
            strSql.Append(" where ModuleID=@ModuleID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ModuleID", SqlDbType.Int,4)};
            parameters[0].Value = ModuleID;

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
        /// 获得指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public DataSet GetAuthorityList(int ModuleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from RGP_ModuleAuthorityList where ModuleID=" + ModuleID.ToString());

            return SqlServerHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public DataSet GetAuthorityNameList(int ModuleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT RGP_ModuleAuthorityList.*, RGP_AuthorityDir.AuthorityName FROM RGP_ModuleAuthorityList INNER JOIN RGP_AuthorityDir ON RGP_ModuleAuthorityList.AuthorityTag = RGP_AuthorityDir.AuthorityTag where ModuleID=" + ModuleID.ToString());

            return SqlServerHelper.Query(strSql.ToString());
        }

        #endregion
    }
}