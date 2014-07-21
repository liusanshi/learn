using System;
using System.Collections.Generic;

using LL.Framework.Permission.DTO;

namespace LL.Framework.Permission.Services
{
    /// <summary>
    /// 权限管理服务
    /// </summary>
    public interface IPermissionService : IDisposable
    {
        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool HasPermission(string pCode, string userId);
        /// <summary>
        /// 创建权限项
        /// </summary>
        /// <param name="appPermission"></param>
        /// <returns></returns>
        AppPermissionDTO AddNewAppPermission(AppPermissionDTO appPermission);
        /// <summary>
        /// 修改权限项
        /// </summary>
        /// <param name="appPermission"></param>
        void UpdateAppPermission(AppPermissionDTO appPermission);

        /// <summary>
        /// 根据应用查找权限
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        List<AppPermissionDTO> FindAppPermission(string appId);
    }
}
