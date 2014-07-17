using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace LL.FrameWork.Web.MVC
{
    internal static class DescriptorUtil
	{
		private static void AppendPartToUniqueIdBuilder(StringBuilder builder, object part)
		{
			if (part == null)
			{
				builder.Append("[-1]");
				return;
			}
			string text = Convert.ToString(part, CultureInfo.InvariantCulture);
			builder.AppendFormat("[{0}]{1}", text.Length, text);
		}
        /// <summary>
        /// 根据参数创建唯一标识
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
		public static string CreateUniqueId(params object[] parts)
		{
			return DescriptorUtil.CreateUniqueId((IEnumerable<object>)parts);
		}
        /// <summary>
        /// 根据参数创建唯一标识
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
		public static string CreateUniqueId(IEnumerable<object> parts)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object current in parts)
			{
				MemberInfo memberInfo = current as MemberInfo;
				if (memberInfo != null)
				{
					DescriptorUtil.AppendPartToUniqueIdBuilder(stringBuilder, memberInfo.Module.ModuleVersionId);
					DescriptorUtil.AppendPartToUniqueIdBuilder(stringBuilder, memberInfo.MetadataToken.ToString());
				}
				else
				{
					IUniquelyIdentifiable uniquelyIdentifiable = current as IUniquelyIdentifiable;
					if (uniquelyIdentifiable != null)
					{
						DescriptorUtil.AppendPartToUniqueIdBuilder(stringBuilder, uniquelyIdentifiable.UniqueId);
					}
					else
					{
						DescriptorUtil.AppendPartToUniqueIdBuilder(stringBuilder, current);
					}
				}
			}
			return stringBuilder.ToString();
		}
        /// <summary>
        /// 延迟获取（不存在则创建）对象的描述
        /// </summary>
        /// <typeparam name="TReflection"></typeparam>
        /// <typeparam name="TDescriptor"></typeparam>
        /// <param name="cacheLocation"></param>
        /// <param name="initializer"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
		public static TDescriptor[] LazilyFetchOrCreateDescriptors<TReflection, TDescriptor>(ref TDescriptor[] cacheLocation, Func<TReflection[]> initializer, Func<TReflection, TDescriptor> converter)
		{
			TDescriptor[] array = Interlocked.CompareExchange<TDescriptor[]>(ref cacheLocation, null, null);
			if (array != null)
			{
				return array;
			}
			TReflection[] source = initializer();
			TDescriptor[] array2 = (
				from descriptor in source.Select(converter)
				where descriptor != null
				select descriptor).ToArray<TDescriptor>();
			TDescriptor[] array3 = Interlocked.CompareExchange<TDescriptor[]>(ref cacheLocation, array2, null);
			return array3 ?? array2;
		}
	}
}
