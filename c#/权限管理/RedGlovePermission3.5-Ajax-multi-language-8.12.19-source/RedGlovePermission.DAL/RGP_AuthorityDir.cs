using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using RedGlovePermission.DBUtility;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.SQLServerDAL
{
	/// <summary>
	/// 权限管理数据访问类
	/// </summary>
	public class RGP_AuthorityDir:IRGP_AuthorityDir
	{
		public RGP_AuthorityDir()
		{}

		/// <summary>
        /// 判断权限标识否存在
		/// </summary>
        /// <param name="AuthorityTag">权限标识</param>
        /// <returns></returns>
        public bool Exists(string AuthorityTag)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from RGP_AuthorityDir");
            strSql.Append(" where AuthorityTag=@AuthorityTag ");
			SqlParameter[] parameters = {
					new SqlParameter("@AuthorityTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = AuthorityTag;

			return SqlServerHelper.Exists(strSql.ToString(),parameters);
		}

		/// <summary>
        /// 增加一个权限
		/// </summary>
		/// <param name="model">权限实体类</param>
		/// <returns></returns>
        public bool CreateAuthority(RedGlovePermission.Model.RGP_AuthorityDir model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into RGP_AuthorityDir(");
            strSql.Append("AuthorityName,AuthorityTag,AuthorityDescription,AuthorityOrder)");
			strSql.Append(" values (");
            strSql.Append("@AuthorityName,@AuthorityTag,@AuthorityDescription,@AuthorityOrder)");
			SqlParameter[] parameters = {
					new SqlParameter("@AuthorityName", SqlDbType.NVarChar,30),
					new SqlParameter("@AuthorityTag", SqlDbType.NVarChar,50),
					new SqlParameter("@AuthorityDescription", SqlDbType.NVarChar,50),
                    new SqlParameter("@AuthorityOrder", SqlDbType.Int,4)};
			parameters[0].Value = model.AuthorityName;
			parameters[1].Value = model.AuthorityTag;
			parameters[2].Value = model.AuthorityDescription;
            parameters[3].Value = model.AuthorityOrder;

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
        /// 更新指定的权限
		/// </summary>
        /// <param name="model">权限实体类</param>
        /// <returns></returns>
        public bool UpdateAuthority(RedGlovePermission.Model.RGP_AuthorityDir model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update RGP_AuthorityDir set ");
			strSql.Append("AuthorityName=@AuthorityName,");
			strSql.Append("AuthorityTag=@AuthorityTag,");
			strSql.Append("AuthorityDescription=@AuthorityDescription,");
			strSql.Append("AuthorityOrder=@AuthorityOrder");
			strSql.Append(" where AuthorityID=@AuthorityID ");
			SqlParameter[] parameters = {
					new SqlParameter("@AuthorityID", SqlDbType.Int,4),
					new SqlParameter("@AuthorityName", SqlDbType.NVarChar,30),
					new SqlParameter("@AuthorityTag", SqlDbType.NVarChar,50),
					new SqlParameter("@AuthorityDescription", SqlDbType.NVarChar,50),
					new SqlParameter("@AuthorityOrder", SqlDbType.Int,4)};
			parameters[0].Value = model.AuthorityID;
			parameters[1].Value = model.AuthorityName;
			parameters[2].Value = model.AuthorityTag;
			parameters[3].Value = model.AuthorityDescription;
			parameters[4].Value = model.AuthorityOrder;

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
        /// 删除一个权限
		/// </summary>
        /// <param name="AuthorityID">权限ID</param>
        /// <returns></returns>
        public bool DeleteAuthority(int AuthorityID)
		{			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete RGP_AuthorityDir ");
			strSql.Append(" where AuthorityID=@AuthorityID ");
			SqlParameter[] parameters = {
					new SqlParameter("@AuthorityID", SqlDbType.Int,4)};
			parameters[0].Value = AuthorityID;

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
        /// 得到一个权限实体
		/// </summary>
		/// <param name="AuthorityID">权限ID</param>
		/// <returns></returns>
        public RedGlovePermission.Model.RGP_AuthorityDir GetAuthorityModel(int AuthorityID)
		{			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select top 1 AuthorityID,AuthorityName,AuthorityTag,AuthorityDescription,AuthorityOrder from RGP_AuthorityDir ");
			strSql.Append(" where AuthorityID=@AuthorityID ");
			SqlParameter[] parameters = {
					new SqlParameter("@AuthorityID", SqlDbType.Int,4)};
			parameters[0].Value = AuthorityID;

			RedGlovePermission.Model.RGP_AuthorityDir model=new RedGlovePermission.Model.RGP_AuthorityDir();
			DataSet ds=SqlServerHelper.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["AuthorityID"].ToString()!="")
				{
					model.AuthorityID=int.Parse(ds.Tables[0].Rows[0]["AuthorityID"].ToString());
				}
				model.AuthorityName=ds.Tables[0].Rows[0]["AuthorityName"].ToString();
				model.AuthorityTag=ds.Tables[0].Rows[0]["AuthorityTag"].ToString();
				model.AuthorityDescription=ds.Tables[0].Rows[0]["AuthorityDescription"].ToString();
				if(ds.Tables[0].Rows[0]["AuthorityOrder"].ToString()!="")
				{
					model.AuthorityOrder=int.Parse(ds.Tables[0].Rows[0]["AuthorityOrder"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
        /// 获得权限数据列表
		/// </summary>
		/// <param name="strWhere">Where条件</param>
		/// <param name="strOrder">排序条件</param>
		/// <returns></returns>
        public DataSet GetAuthorityList(string strWhere,string strOrder)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select * FROM RGP_AuthorityDir ");

			if(strWhere.Trim() != "")
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

