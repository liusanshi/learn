using System;

using LL.Framework.Permission.DTO;

namespace LL.Framework.Permission.Services
{
    /// <summary>
    /// 应用的服务
    /// </summary>
    public interface IAppService : IDisposable
    {
        /// <summary>
        /// 新增App
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        AppDTO AddNewApp(AppDTO app);

        /// <summary>
        /// 修改App
        /// </summary>
        /// <param name="app"></param>
        void UpdateApp(AppDTO app);

        /// <summary>
        /// 删除App
        /// </summary>
        /// <param name="app"></param>
        void DeleteApp(AppDTO app);

        
    }
}
