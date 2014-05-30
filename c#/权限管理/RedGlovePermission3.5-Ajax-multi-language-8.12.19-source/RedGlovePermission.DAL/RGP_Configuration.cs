using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using RedGlovePermission.DBUtility;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.SQLServerDAL
{
    public class RGP_Configuration:IRGP_Configuration
    {
        public RGP_Configuration() { }

        /// <summary>
        /// 判断配置项是否存在
        /// </summary>
        /// <param name="ItemName">配置项名称</param>
        /// <returns></returns>
        public bool Exists(string ItemName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from RGP_Configuration");
            strSql.Append(" where ItemName=@ItemName ");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemName", SqlDbType.NVarChar,50)};
            parameters[0].Value = ItemName;

            return SqlServerHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 创建新配置
        /// </summary>
        /// <param name="ItemName">配置名称</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        public bool CreateItem(string ItemName,string ItemValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into RGP_Configuration(");
            strSql.Append("ItemName,ItemValue)");
            strSql.Append(" values (");
            strSql.Append("@ItemName,@ItemValue)");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemName", SqlDbType.NVarChar,50),
					new SqlParameter("@ModuleTypeDescription", SqlDbType.NVarChar)};
            parameters[0].Value = ItemName;
            parameters[1].Value = ItemValue;

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
        /// 更新配置
        /// </summary>
        /// <param name="id">配置项ID</param>
        /// <param name="ItemName">配置名称</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        public bool UpdateItem(int id,string ItemName, string ItemValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RGP_Configuration set ");
            strSql.Append("ItemName=@ItemName,");
            strSql.Append("ItemValue=@ItemValue,");
            strSql.Append(" where ItemID=@ItemID");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemID", SqlDbType.Int,4),
					new SqlParameter("@ItemName", SqlDbType.NVarChar,50),
					new SqlParameter("@ItemValue",SqlDbType.NVarChar)};
            parameters[0].Value = id;
            parameters[1].Value = ItemName;
            parameters[2].Value = ItemValue;

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
        /// 更新配置值
        /// </summary>
        /// <param name="id">配置项ID</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        public bool UpdateItem(int id, string ItemValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RGP_Configuration set ");
            strSql.Append("ItemValue=@ItemValue ");
            strSql.Append("where ItemID=@ItemID");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemID", SqlDbType.Int,4),
					new SqlParameter("@ItemValue",SqlDbType.NVarChar)};
            parameters[0].Value = id;
            parameters[1].Value = ItemValue;

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
        /// 删除配置项
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public bool DeleteItem(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete RGP_Configuration ");
            strSql.Append(" where ItemID=@ItemID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemID", SqlDbType.Int,4)};
            parameters[0].Value = id;

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
        /// 获取配置项的址
        /// </summary>
        /// <param name="ItemName">配置项名称</param>
        /// <returns></returns>
        public string GetItemValue(string ItemName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemValue FROM RGP_Configuration where ItemName=@ItemName");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemName", SqlDbType.NVarChar,50)};
            parameters[0].Value = ItemName;

            object obj = SqlServerHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }

        /// <summary>
        /// 获取配置的列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetItemList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM RGP_Configuration ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            return SqlServerHelper.Query(strSql.ToString());
        }
    }
}
