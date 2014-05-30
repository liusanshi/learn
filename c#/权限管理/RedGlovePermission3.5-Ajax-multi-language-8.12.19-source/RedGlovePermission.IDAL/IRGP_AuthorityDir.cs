using System;
using System.Data;

namespace RedGlovePermission.IDAL
{
    public interface IRGP_AuthorityDir
	{		
        /// <summary>
        /// 判断权限标识否存在
        /// </summary>
        /// <param name="AuthorityTag">权限标识</param>
        /// <returns></returns>
        bool Exists(string AuthorityTag);

        /// <summary>
        /// 增加一个权限
        /// </summary>
        /// <param name="model">权限实体类</param>
        /// <returns></returns>
        bool CreateAuthority(RedGlovePermission.Model.RGP_AuthorityDir model);

        /// <summary>
        /// 更新指定的权限
        /// </summary>
        /// <param name="model">权限实体类</param>
        /// <returns></returns>
        bool UpdateAuthority(RedGlovePermission.Model.RGP_AuthorityDir model);

        /// <summary>
        /// 删除一个权限
        /// </summary>
        /// <param name="AuthorityID">权限ID</param>
        /// <returns></returns>
        bool DeleteAuthority(int AuthorityID);

        /// <summary>
        /// 得到一个权限实体
        /// </summary>
        /// <param name="AuthorityID">权限ID</param>
        /// <returns></returns>
        RedGlovePermission.Model.RGP_AuthorityDir GetAuthorityModel(int AuthorityID);

        /// <summary>
        /// 获得权限数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        DataSet GetAuthorityList(string strWhere, string strOrder);
	}
}

