using System;
using System.Collections.Generic;

namespace LL.FrameWork.Core.Infrastructure.IOC
{
    /// <summary>
    /// 解耦依赖 的接口
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="key">服务相关的key</param>
        /// <param name="serviceType">服务相关的类型</param>
        /// <returns></returns>
        object GetService(string key, Type serviceType);
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType">服务相关的类型</param>
        /// <returns></returns>
        object GetService(Type serviceType);

        IEnumerable<object> GetServices(Type serviceType);
    }
}
