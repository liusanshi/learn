using System;
using System.Collections;
using System.Data;

namespace RedGlovePermission.IDAL
{
    public interface IRGP_Roles
	{

        #region 角色管理

        /// <summary>
        /// 判断角色是否存在
        /// </summary>
        /// <param name="RoleName">角色名称</param>
        /// <param name="RoleGroupID">角色分组ID</param>
        /// <returns></returns>
        bool RoleExists(string RoleName, int RoleGroupID);

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="model">角色实体类</param>
        /// <returns></returns>
        int CreateRole(RedGlovePermission.Model.RGP_Roles model);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model">角色实体类</param>
        /// <returns></returns>
        bool UpdateRole(RedGlovePermission.Model.RGP_Roles model);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns></returns>
        int DeleteRole(int RoleID);

        /// <summary>
        /// 获取角色实体
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns></returns>
        RedGlovePermission.Model.RGP_Roles GetRoleModel(int RoleID);

        /// <summary>
        /// 获得角色数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        DataSet GetRoleList(string strWhere, string strOrder);

        #endregion

        #region 角色授权

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        bool RoleAuthorityExists(RedGlovePermission.Model.RGP_RoleAuthorityList model);

        /// <summary>
        /// 修改角色模块权限
        /// </summary>
        bool UpdateRoleAuthority(ArrayList list);

        /// <summary>
        /// 修改用户模块权限
        /// </summary>
        bool UpdateUserAuthority(ArrayList list);

        /// <summary>
        /// 读取角色的模块权限
        /// </summary>
        DataSet GetRoleAuthorityList(int RoleID, int ModuleID);

        /// <summary>
        /// 读取用户的模块权限
        /// </summary>
        DataSet GetUserAuthorityList(int UserID, int ModuleID);

        #endregion
    }
}

