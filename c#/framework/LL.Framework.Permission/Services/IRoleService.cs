using System;
using System.Collections.Generic;

using LL.Framework.Permission.DTO;

namespace LL.Framework.Permission.Services
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleService
    {
        RoleDTO AddNewRole(RoleDTO role);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        void UpdateRole(RoleDTO role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role"></param>
        void DeleteRole(RoleDTO role);

        /// <summary>
        /// 根据角色查找用户
        /// </summary>
        /// <param name="role"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        List<UserDTO> FindUsersByRole(RoleDTO role, string text);
    }
}
