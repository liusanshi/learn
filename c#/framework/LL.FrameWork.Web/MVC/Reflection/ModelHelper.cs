using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;

using LL.Framework.Core.Reflection;

namespace LL.Framework.Web.MVC
{
    internal static class ModelHelper
    {
        public static readonly bool IsDebugMode;

        static ModelHelper()
        {
            CompilationSection configSection =
                        ConfigurationManager.GetSection("system.web/compilation") as CompilationSection;
            if (configSection != null)
                IsDebugMode = configSection.Debug;
        }

        public static object SafeChangeType(string value, Type conversionType)
        {
            if (conversionType == typeof(string))
                return value;

            if (value == null || value.Length == 0)
            {
                // 空字符串根本不能做任何转换，所以直接返回null
                return null;
            }

            try
            {
                if (conversionType == typeof(Guid))
                    return new Guid(value);

                if (conversionType.IsEnum)
                    return Enum.Parse(conversionType, value);


                // 为了简单，直接调用 .net framework中的方法。
                // 如果转换失败，则会抛出异常。
                return Convert.ChangeType(value, conversionType);
            }
            catch
            {
                if (IsDebugMode)
                    throw;			// Debug 模式下抛异常
                else
                {
                    // Release模式下忽略异常（防止恶意用户错误输入）
                    return null;
                }
            }
        }


        /// <summary>
        /// 判断指定的类型是否能从String类型做隐式类型转换，如果可以，则返回相应的方法
        /// </summary>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static MethodInfo GetStringImplicit(Type conversionType)
        {
            MethodInfo m = conversionType.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public);

            if (m != null && m.IsStatic && m.IsSpecialName && m.ReturnType == conversionType)
            {
                ParameterInfo[] paras = m.GetParameters();
                if (paras.Length == 1 && paras[0].ParameterType == typeof(string))
                    return m;
            }

            return null;
        }
    }
}
