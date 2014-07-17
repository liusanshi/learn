using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Web;

using LL.Framework.Core.Reflection;

namespace LL.Framework.Web.MVC.Serializer
{
    internal class JsonDataProvider : IActionParametersProvider
    {
        // ASP.NET 4.0 为下面二个方法增加了重载版本，所以必须指定更多的匹配条件。
        private static readonly MethodInfo s_methodDeserialize
                = typeof(JavaScriptSerializer).GetMethod("Deserialize",
                            BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(string) }, null);

        private static readonly MethodInfo s_methodConvertToType
                = typeof(JavaScriptSerializer).GetMethod("ConvertToType",
                            BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(object) }, null);

        JavaScriptSerializer jss = new JavaScriptSerializer();

        public IDictionary<string, object> GetParameters(HttpRequest request, ActionDescriptor action)
        {
            string input = request.ReadInputStream();
            var Parameters = action.GetParameters();

            if (Parameters.Length == 1)
            {
                object value = GetObjectFromString(input, Parameters[0].ParameterType.GetRealType());
                return new Dictionary<string, object> { { Parameters[0].ParameterName, value } };
            }
            else
                return GetMultiObjectsFormString(input, action);
        }


        private object GetObjectFromString(string input, Type destType)
        {
            MethodInfo deserialize = s_methodDeserialize.MakeGenericMethod(destType);

            return deserialize.FastInvoke(jss, new object[] { input });
        }

        private IDictionary<string, object> GetMultiObjectsFormString(string input, ActionDescriptor action)
        {
            Dictionary<string, object> dict = jss.DeserializeObject(input) as Dictionary<string, object>;

            //if( dict.Count != action.Parameters.Length )
            //    throw new ArgumentException("客户端提交的数据项与服务端的参数项的数量不匹配。");

            var Parameters = action.GetParameters();
            Dictionary<string, object> parameters = new Dictionary<string, object>(Parameters.Length);

            for (int i = 0; i < Parameters.Length; i++)
            {
                var parameterDescriptor = Parameters[i];
                string name = parameterDescriptor.ParameterName;
                object value = (from kv in dict
                                where string.Compare(kv.Key, parameterDescriptor.BindingInfo.Prefix + name, StringComparison.OrdinalIgnoreCase) == 0
                                select kv.Value).FirstOrDefault();

                if (value != null)
                {
                    Type destType = parameterDescriptor.ParameterType.GetRealType();

                    MethodInfo method = s_methodConvertToType.MakeGenericMethod(destType);
                    object parameter = method.FastInvoke(jss, new object[] { PropertyFilter(value, parameterDescriptor) });
                    parameters[name] = parameter;
                }
                else
                {
                    parameters[name] = parameterDescriptor.DefaultValue;
                }
            }
            return parameters;
        }
        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parameterDescriptor"></param>
        /// <returns></returns>
        private object PropertyFilter(object val, ParameterDescriptor parameterDescriptor)
        {
            IDictionary<string, object> dict = val as IDictionary<string, object>;
            if (dict == null)
            {
                return val;
            }
            var filter = ParameterDescriptor.GetPropertyFilter(parameterDescriptor);
            foreach (var item in dict.Keys)
            {
                if (!filter(item))
                {
                    dict[item] = null;
                }
            }
            return dict;
        }
    }
}
