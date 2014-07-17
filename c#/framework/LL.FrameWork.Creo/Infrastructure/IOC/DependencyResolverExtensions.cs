using System;
using System.Collections.Generic;
using System.Linq;

namespace LL.Framework.Core.Infrastructure.IOC
{
    public static class DependencyResolverExtensions
    {
        /// <summary>
        /// 泛型 获取指定类型的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static TService GetService<TService>(this IDependencyResolver resolver)
        {
            return (TService)((object)resolver.GetService(typeof(TService)));
        }
        /// <summary>
        /// 泛型 获取指定类型的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="resolver"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TService GetService<TService>(this IDependencyResolver resolver, string key)
        {
            return (TService)((object)resolver.GetService(key, typeof(TService)));
        }
        /// <summary>
        /// 泛型 获取指定类型的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static IEnumerable<TService> GetServices<TService>(this IDependencyResolver resolver)
        {
            return resolver.GetServices(typeof(TService)).Cast<TService>();
        }
    }
}
