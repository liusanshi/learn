using System;
using System.Reflection;
using System.Collections.Generic;

#if Castle  
using Castle.DynamicProxy;
#endif

namespace LL.FrameWork.Core.Reflection
{
    /// <summary>
    /// 反射缓存接口
    /// </summary>
    public interface IFastReflectionCache<TKey, TValue>
    {
        TValue Get(TKey key);
    }
    /// <summary>
    /// 缓存基类
    /// DynamicMethod 来创建
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue>
    {
        #if Castle  
        protected ProxyGenerator Generator;
        public FastReflectionCache(ProxyGenerator generator)
        {
            Generator = generator;
        }
        #endif
        private Dictionary<TKey, TValue> dic_cache = new Dictionary<TKey, TValue>();

        public FastReflectionCache() { }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            TValue value = default(TValue);
            TValue result;
            if (dic_cache.TryGetValue(key, out value))
            {
                result = value;
            }
            else
            {
                lock (key)
                {
                    if (dic_cache.TryGetValue(key, out value))
                    {
                        result = value;
                    }
                    else
                    {
                        result = Create(key);
                        dic_cache[key] = result;
                    }
                }
            }
            return result;
        }

        TValue IFastReflectionCache<TKey, TValue>.Get(TKey key)
        {
            return this.Get(key);
        }

        /// <summary>
        /// 创建类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract TValue Create(TKey key);
    }

    /// <summary>
    /// 所有缓存
    /// </summary>
    public static class FastReflectionCaches
    {
        static IFastReflectionCache<ConstructorInfo, IConstructorInvoker> _ConstructorReflectionCache;
        static IFastReflectionCache<MethodInfo, IMethodInvoker> _MethodReflectionCache;
        static IFastReflectionCache<FieldInfo, IFieldAccessor> _FieldReflectionCache;
        static IFastReflectionCache<PropertyInfo, IPropertyAccessor> _ProertyReflectionCache;

        static FastReflectionCaches()
        {
#if Castle
            
#endif
            _ConstructorReflectionCache = new ConstructorReflectionCache();
            _MethodReflectionCache = new MethodReflectionCache();
            _FieldReflectionCache = new FieldReflectionCache();
            _ProertyReflectionCache = new PropertyReflectionCache();
        }

        /// <summary>
        /// 属性访问对象
        /// </summary>
        public static IFastReflectionCache<PropertyInfo, IPropertyAccessor> ProertyReflectionCache
        {
            get { return _ProertyReflectionCache; }
        }
        /// <summary>
        /// 字段访问对象
        /// </summary>
        public static IFastReflectionCache<FieldInfo, IFieldAccessor> FieldReflectionCache
        {
            get { return _FieldReflectionCache; }
        }
        /// <summary>
        /// 方法访问对象
        /// </summary>
        public static IFastReflectionCache<MethodInfo, IMethodInvoker> MethodReflectionCache
        {
            get { return _MethodReflectionCache; }
        }
        /// <summary>
        /// 构造器访问对象
        /// </summary>
        public static IFastReflectionCache<ConstructorInfo, IConstructorInvoker> ConstructorReflectionCache
        {
            get { return _ConstructorReflectionCache; }
        }
    }
}
