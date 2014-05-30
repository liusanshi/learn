using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using RedGlovePermission.Lib;
using RedGlovePermission.Model;
using RedGlovePermission.DALFactory;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.BLL
{
	/// <summary>
	/// 业务逻辑类RGP_Roles 的摘要说明。
	/// </summary>
	public class RGP_Roles
	{
        private readonly IRGP_Roles dal = DataAccess.CreateRGP_Roles();

		public RGP_Roles()
		{ }

        #region 角色管理

        /// <summary>
        /// 判断角色是否存在
        /// </summary>
        /// <param name="RoleName">角色名称</param>
        /// <param name="RoleGroupID">角色分组ID</param>
        /// <returns></returns>
        public bool RoleExists(string RoleName, int RoleGroupID)
        {
            return dal.RoleExists(RoleName, RoleGroupID);
        }

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="model">角色实体类</param>
        /// <returns></returns>
        public int CreateRole(RedGlovePermission.Model.RGP_Roles model)
        {
            return dal.CreateRole(model);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model">角色实体类</param>
        /// <returns></returns>
        public bool UpdateRole(RedGlovePermission.Model.RGP_Roles model)
        {
            return dal.UpdateRole(model);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns></returns>
        public int DeleteRole(int RoleID)
        {
            return dal.DeleteRole(RoleID);
        }

        /// <summary>
        /// 获取角色实体
        /// </summary>
        /// <param name="RoleID">角色ID</param>
        /// <returns></returns>
        public RedGlovePermission.Model.RGP_Roles GetRoleModel(int RoleID)
        {
            return dal.GetRoleModel(RoleID);
        }

        /// <summary>
        /// 获得角色数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetRoleList(string strWhere, string strOrder)
        {
            return dal.GetRoleList(strWhere, strOrder);
        }

        #endregion

        #region 角色授权

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        public bool RoleAuthorityExists(RedGlovePermission.Model.RGP_RoleAuthorityList model)
        {
            return dal.RoleAuthorityExists(model);
        }

        /// <summary>
        /// 修改角色模块权限
        /// </summary>
        public bool UpdateRoleAuthority(ArrayList list)
        {
            return dal.UpdateRoleAuthority(list);
        }

        /// <summary>
        /// 修改用户模块权限
        /// </summary>
        public bool UpdateUserAuthority(ArrayList list)
        {
            return dal.UpdateUserAuthority(list);
        }

        /// <summary>
        /// 读取角色的模块权限
        /// </summary>
        public DataSet GetRoleAuthorityList(int RoleID, int ModuleID)
        {
            return dal.GetRoleAuthorityList(RoleID, ModuleID);
        }

        /// <summary>
        /// 读取用户的模块权限
        /// </summary>
        public DataSet GetUserAuthorityList(int UserID, int ModuleID)
        {
            return dal.GetUserAuthorityList(UserID, ModuleID);
        }

        #endregion
    }
}

