using System;
using System.Reflection;

namespace LL.Framework.Core.Reflection
{
    public static class FastReflectionExtensions
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="Target"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object FastInvoke(this MethodInfo method, object Target, params object[] args)
        {
            return FastReflectionCaches.MethodReflectionCache.Get(method).Invoke(Target, args);
        }
        /// <summary>
        /// 调用构造函数
        /// </summary>
        /// <param name="constrcutor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object FastCreate(this ConstructorInfo constrcutor, params object[] args)
        {
            return FastReflectionCaches.ConstructorReflectionCache.Get(constrcutor).Invoke(args);
        }

        /// <summary>
        /// 调用Get属性
        /// </summary>
        /// <param name="property"></param>
        /// <param name="Target"></param>
        /// <returns></returns>
        public static object FastGetValue(this PropertyInfo property, object Target)
        {
            return FastReflectionCaches.ProertyReflectionCache.Get(property).Get(Target);
        }
        /// <summary>
        /// 调用Set属性
        /// </summary>
        /// <param name="property"></param>
        /// <param name="Target"></param>
        /// <param name="Value"></param>
        public static void FastSetValue(this PropertyInfo property, object Target, object Value)
        {
            FastReflectionCaches.ProertyReflectionCache.Get(property).Set(Target, Value);
        }
        /// <summary>
        /// 获取成员变量
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Target"></param>
        /// <returns></returns>
        public static object FastGetValue(this FieldInfo field, object Target)
        {
            return FastReflectionCaches.FieldReflectionCache.Get(field).Get(Target);
        }
        /// <summary>
        /// 设置成员变量
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Target"></param>
        /// <param name="Value"></param>
        public static void FastSetValue(this FieldInfo field, object Target, object Value)
        {
            FastReflectionCaches.FieldReflectionCache.Get(field).Set(Target, Value);
        }
    }
}
