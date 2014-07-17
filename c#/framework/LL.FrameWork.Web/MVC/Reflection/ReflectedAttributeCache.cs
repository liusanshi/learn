using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace LL.Framework.Web.MVC
{
    internal static class ReflectedAttributeCache
    {
        private static readonly ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionMethodSelectorAttribute>> _actionMethodSelectorAttributeCache = new ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionMethodSelectorAttribute>>();
        private static readonly ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionNameSelectorAttribute>> _actionNameSelectorAttributeCache = new ConcurrentDictionary<MethodInfo, ReadOnlyCollection<ActionNameSelectorAttribute>>();
        private static readonly ConcurrentDictionary<MethodInfo, ReadOnlyCollection<FilterAttribute>> _methodFilterAttributeCache = new ConcurrentDictionary<MethodInfo, ReadOnlyCollection<FilterAttribute>>();
        private static readonly ConcurrentDictionary<Type, ReadOnlyCollection<FilterAttribute>> _typeFilterAttributeCache = new ConcurrentDictionary<Type, ReadOnlyCollection<FilterAttribute>>();
        private static readonly ConcurrentDictionary<MethodInfo, ReadOnlyCollection<PageUrlAttribute>> _methodPageUrlAttributeCache = new ConcurrentDictionary<MethodInfo, ReadOnlyCollection<PageUrlAttribute>>();

        /// <summary>
        /// 获取筛选特性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICollection<FilterAttribute> GetTypeFilterAttributes(Type type)
        {
            return ReflectedAttributeCache.GetAttributes<Type, FilterAttribute>(ReflectedAttributeCache._typeFilterAttributeCache, type);
        }

        public static ICollection<FilterAttribute> GetMethodFilterAttributes(MethodInfo methodInfo)
        {
            return ReflectedAttributeCache.GetAttributes<MethodInfo, FilterAttribute>(ReflectedAttributeCache._methodFilterAttributeCache, methodInfo);
        }
        public static ICollection<ActionMethodSelectorAttribute> GetActionMethodSelectorAttributes(MethodInfo methodInfo)
        {
            return ReflectedAttributeCache.GetAttributes<MethodInfo, ActionMethodSelectorAttribute>(ReflectedAttributeCache._actionMethodSelectorAttributeCache, methodInfo);
        }
        public static ICollection<ActionNameSelectorAttribute> GetActionNameSelectorAttributes(MethodInfo methodInfo)
        {
            return ReflectedAttributeCache.GetAttributes<MethodInfo, ActionNameSelectorAttribute>(ReflectedAttributeCache._actionNameSelectorAttributeCache, methodInfo);
        }
        /// <summary>
        /// 获取PageUrlAttribute 属性
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static ICollection<PageUrlAttribute> GetPageUrlAttributes(MethodInfo methodInfo)
        {
            return ReflectedAttributeCache.GetAttributes<MethodInfo, PageUrlAttribute>(ReflectedAttributeCache._methodPageUrlAttributeCache, methodInfo);
        }

        private static ReadOnlyCollection<TAttribute> GetAttributes<TMemberInfo, TAttribute>(ConcurrentDictionary<TMemberInfo, 
            ReadOnlyCollection<TAttribute>> lookup, TMemberInfo memberInfo)
            where TMemberInfo : MemberInfo
            where TAttribute : Attribute
        {
            return lookup.GetOrAdd(memberInfo, delegate(TMemberInfo mi)
            {
                bool inherit = true;
                return new ReadOnlyCollection<TAttribute>((TAttribute[])memberInfo.GetCustomAttributes(typeof(TAttribute), inherit));
            });
        }
    }
}
