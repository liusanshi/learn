using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Core.Infrastructure.IOC
{
    public interface IContainer
    {
        /// <summary>
        /// 决定哪个服务
        /// </summary>
        /// <param name="TServices"></param>
        /// <returns></returns>
        object Resolve(Type TServices);

        /// <summary>
        /// 决定哪个服务
        /// </summary>
        /// <param name="TServices"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        object Resolve(Type TServices, string name);
        
        /// <summary>
        /// 决定哪个服务
        /// </summary>
        /// <typeparam name="TServices"></typeparam>
        /// <returns></returns>
        TServices Resolve<TServices>();

        /// <summary>
        /// 决定哪个服务
        /// </summary>
        /// <typeparam name="TServices"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        TServices Resolve<TServices>(string name);
    }
}
