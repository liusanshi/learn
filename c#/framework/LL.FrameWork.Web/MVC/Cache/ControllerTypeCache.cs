using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 类型缓存
    /// </summary>
    internal sealed class ControllerTypeCache
    {
        /// <summary>
        /// 默认的类型缓存对象
        /// </summary>
        internal readonly static ControllerTypeCache DefaultControllerTypeCache = new ControllerTypeCache();

        private Dictionary<string, ILookup<string, Type>> _cache;
        private object _lockObj = new object();
        /// <summary>
        /// 数量
        /// </summary>
        internal int Count
        {
            get
            {
                int num = 0;
                foreach (ILookup<string, Type> current in this._cache.Values)
                {
                    foreach (IGrouping<string, Type> current2 in current)
                    {
                        num += current2.Count<Type>();
                    }
                }
                return num;
            }
        }
        /// <summary>
        /// 所有类型
        /// </summary>
        internal IEnumerable<Type> AllTypes
        {
            get
            {
                foreach (ILookup<string, Type> current in this._cache.Values)
                {
                    foreach (IGrouping<string, Type> current2 in current)
                    {
                        foreach (Type item in current2)
                        {
                            yield return item;
                        }
                    }
                }
                yield break;
            }
        }

        /// <summary>
        /// 确认已经初始化
        /// </summary>
        public void EnsureInitialized()
        {
            if (this._cache == null)
            {
                lock (this._lockObj)
                {
                    if (this._cache == null)
                    {
                        var listType = FilterTypesInAssemblies(ControllerTypeCache.IsControllerType);

                        IEnumerable<IGrouping<string, Type>> source = listType.GroupBy((Type t) =>
                            t.Name.Substring(0, t.Name.Length - "Controller".Length), StringComparer.OrdinalIgnoreCase);

                        this._cache = source.ToDictionary((IGrouping<string, Type> g) => g.Key, 
                            (IGrouping<string, Type> g) =>
                                g.ToLookup((Type t) => t.Namespace ?? string.Empty, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
        }
        /// <summary>
        /// 根据名称获取类型
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public ICollection<Type> GetControllerTypes(string controllerName, HashSet<string> namespaces)
        {
            HashSet<Type> hashSet = new HashSet<Type>();
            ILookup<string, Type> lookup;
            if (this._cache.TryGetValue(controllerName, out lookup))
            {
                if (namespaces != null)
                {
                    using (HashSet<string>.Enumerator enumerator = namespaces.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            string current = enumerator.Current;
                            foreach (IGrouping<string, Type> current2 in lookup)
                            {
                                if (ControllerTypeCache.IsNamespaceMatch(current, current2.Key))
                                {
                                    hashSet.UnionWith(current2);
                                }
                            }
                        }
                        return hashSet;
                    }
                }
                foreach (IGrouping<string, Type> current3 in lookup)
                {
                    hashSet.UnionWith(current3);
                }
            }
            return hashSet;
        }



        /// <summary>
        /// 判断是否控制器
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static bool IsControllerType(Type t)
        {
            return t != null 
                && t.IsPublic && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) 
                && !t.IsAbstract && typeof(IController).IsAssignableFrom(t);
        }
        /// <summary>
        /// 命名空间是否匹配
        /// </summary>
        /// <param name="requestedNamespace"></param>
        /// <param name="targetNamespace"></param>
        /// <returns></returns>
        internal static bool IsNamespaceMatch(string requestedNamespace, string targetNamespace)
        {
            if (requestedNamespace == null)
            {
                return false;
            }
            if (requestedNamespace.Length == 0)
            {
                return true;
            }
            if (!requestedNamespace.EndsWith(".*", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(requestedNamespace, targetNamespace, StringComparison.OrdinalIgnoreCase);
            }
            requestedNamespace = requestedNamespace.Substring(0, requestedNamespace.Length - ".*".Length);
            return targetNamespace.StartsWith(requestedNamespace, StringComparison.OrdinalIgnoreCase) 
                && (requestedNamespace.Length == targetNamespace.Length || targetNamespace[requestedNamespace.Length] == '.');
        }

        /// <summary>
        /// 查找所有类型
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private static IEnumerable<Type> FilterTypesInAssemblies(Predicate<Type> predicate)
        {
            IEnumerable<Type> enumerable = Type.EmptyTypes;
            ICollection referencedAssemblies = BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in referencedAssemblies)
            {
                // 过滤以【System】开头的程序集，加快速度
                if (assembly.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase))
                    continue;

                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types;
                }
                enumerable = enumerable.Concat(types);
            }
            return
                from type in enumerable
                where TypeIsPublicClass(type) && predicate(type)
                select type;
        }

        /// <summary>
        /// 是否公开类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool TypeIsPublicClass(Type type)
        {
            return type != null && type.IsPublic && type.IsClass && !type.IsAbstract;
        }
    }
}
