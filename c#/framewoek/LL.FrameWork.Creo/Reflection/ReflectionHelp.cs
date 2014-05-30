using System;
using System.Text;
using System.Reflection;

namespace LL.FrameWork.Core.Reflection
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public static class ReflectionHelp
    {
        /// <summary>
        /// 字符串处理缓存
        /// </summary>
        private static readonly StringBuilder Stringbuff = new StringBuilder();

        /// <summary>
        /// 获取成员的标识
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        internal static string GetMemberSignName(MemberInfo mi)
        {
            Stringbuff.Length = 0;
            Stringbuff.Append(mi.DeclaringType.FullName).Append("@");
            switch (mi.MemberType)
            {
                case MemberTypes.Constructor:
                    Stringbuff.Append(mi.DeclaringType.Name);
                    GetParamsSignName(Stringbuff, ((ConstructorInfo)mi).GetParameters());
                    break;
                case MemberTypes.Method:
                    Stringbuff.Append(mi.Name);
                    GetParamsSignName(Stringbuff, ((MethodInfo)mi).GetParameters());
                    break;
                default:
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.Field:
                case MemberTypes.Property:
                case MemberTypes.TypeInfo:
                    Stringbuff.Append(mi.Name);
                    break;
            }
            return Stringbuff.ToString();
        }

        private static void GetParamsSignName(StringBuilder sb, params ParameterInfo[] args)
        {
            if (args == null || args.Length == 0) return;
            foreach (var item in args)
            {
                sb.AppendFormat("-{0}", item.ParameterType.Name);
            }
        }

        /// <summary>
        /// 空类型
        /// </summary>
        internal readonly static Type VoidType = typeof(void);
        /// <summary>
        /// object类型
        /// </summary>
        internal readonly static Type ObjectType = typeof(object);
        /// <summary>
        /// object[]类型
        /// </summary>
        internal readonly static Type ObjArrayType = typeof(object[]);

        /// <summary>
        /// 默认的方法标记
        /// </summary>
        internal readonly static MethodAttributes DefaultMethodAttributes 
            = MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot;
    }
}
