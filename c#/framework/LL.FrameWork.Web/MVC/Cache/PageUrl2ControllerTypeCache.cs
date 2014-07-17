using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// PageUrl转换为ControllerType, MethodInfo
    /// </summary>
    internal class PageUrl2ControllerTypeCache : ReaderWriterCache<string, Tuple<Type, MethodInfo>>
    {
        /// <summary>
        /// 获取类型与方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Tuple<Type, MethodInfo> GetDescriptor(string url)
        {
            return base.FetchOrCreateItem(url, () => GetTypeByUrl(url));
        }

        /// <summary>
        /// 默认的缓存对象
        /// </summary>
        internal readonly static PageUrl2ControllerTypeCache DefaultPageUrl2ControllerTypeCache = new PageUrl2ControllerTypeCache();

        /// <summary>
        /// 类型缓存
        /// </summary>
        private ControllerTypeCache ControllerTypeCache
        {
            get
            {
                return ControllerTypeCache.DefaultControllerTypeCache;
            }
        }

        // 用于从类型查找Action的反射标记
        private static readonly BindingFlags ActionBindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        /// <summary>
        /// 根据url获取类型
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Tuple<Type, MethodInfo> GetTypeByUrl(string url)
        {
            ControllerTypeCache.EnsureInitialized();
            foreach (var type in ControllerTypeCache.AllTypes)
            {
                foreach (var method in type.GetMethods(ActionBindingFlags))
                {
                    foreach (var attr in ReflectedAttributeCache.GetPageUrlAttributes(method))
                    {
                        base.Cache[attr.Url] = new Tuple<Type, MethodInfo>(type, method);
                    }
                }
            }
            Tuple<Type, MethodInfo> result;
            base.Cache.TryGetValue(url, out result);
            return result;
        }
    }
}
