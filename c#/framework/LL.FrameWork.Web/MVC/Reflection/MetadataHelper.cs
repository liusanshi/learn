using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Text;
using System.Web;
using System.Xml;

using LL.Framework.Core.Reflection;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 元数据获取帮助类
    /// </summary>
    internal static class MetadataHelper
	{
        private static Hashtable s_modelTable = Hashtable.Synchronized(
											new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

		/// <summary>
		/// 返回一个实体类型的描述信息（全部属性及字段）。
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ModelDescriptor GetModelDescriptor(Type type)
		{
			if( type == null )
				throw new ArgumentNullException("type");
			
			string key = type.FullName;
			ModelDescriptor mm = (ModelDescriptor)s_modelTable[key];

			if( mm == null ) {
				List<DataMember> list = new List<DataMember>();

				(from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				 select new PropertyMember(p)).ToList().ForEach(x=>list.Add(x));

				(from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
				 select new FieldMember(f)).ToList().ForEach(x => list.Add(x));

				mm = new ModelDescriptor { Fields = list.ToArray() };
				s_modelTable[key] = mm;
			}
			return mm;
		}

		/// <summary>
		/// 返回一个实体类型的指定名称的数据成员
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DataMember GetMemberByName(Type type, string name, bool ifNotFoundThrowException)
		{
			ModelDescriptor description = GetModelDescriptor(type);
			foreach( DataMember member in description.Fields )
				if( member.Name == name )
					return member;

			if( ifNotFoundThrowException )
				throw new ArgumentOutOfRangeException(
						string.Format("指定的成员 {0} 在类型 {1} 中并不存在。", name, type.ToString()));

			return null;
		}
	}
}
