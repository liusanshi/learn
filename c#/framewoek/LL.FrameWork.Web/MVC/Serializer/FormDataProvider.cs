using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;

using LL.FrameWork.Core.Reflection;
using LL.FrameWork.Impl.Infrastructure.Adapter.EmitMapperImpl;
using LL.FrameWork.Web.MVC;

namespace LL.FrameWork.Web.MVC.Serializer
{
    internal class FormDataProvider : IActionParametersProvider
    {
        public IDictionary<string, object> GetParameters(HttpRequest request, ActionDescriptor action)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (action == null)
                throw new ArgumentNullException("action");

            ParameterDescriptor[] ActionParam = action.GetParameters();
            IDictionary<string, object> parameters = new Dictionary<string, object>(ActionParam.Length);

            for (int i = 0; i < ActionParam.Length; i++)
            {
                ParameterDescriptor p = ActionParam[i];

                if (p.ParameterInfo.IsOut)
                    continue;

                if (p.ParameterType == ReflectionHelper.VoidType)
                    continue;

                if (p.ParameterType == typeof(NameValueCollection))
                {
                    if (string.Compare(p.ParameterName, "Form", StringComparison.OrdinalIgnoreCase) == 0)
                        parameters[p.ParameterName] = request.Form;
                    else if (string.Compare(p.ParameterName, "QueryString", StringComparison.OrdinalIgnoreCase) == 0)
                        parameters[p.ParameterName] = request.QueryString;
                    else if (string.Compare(p.ParameterName, "Headers", StringComparison.OrdinalIgnoreCase) == 0)
                        parameters[p.ParameterName] = request.Headers;
                    else if (string.Compare(p.ParameterName, "ServerVariables", StringComparison.OrdinalIgnoreCase) == 0)
                        parameters[p.ParameterName] = request.ServerVariables;
                }
                else
                {
                    Type paramterType = p.ParameterType.GetRealType();

                    // 如果参数是可支持的类型，则直接从HttpRequest中读取并赋值
                    if (paramterType.IsSupportableType())
                    {
                        object val = GetValueByNameAndTypeFrommRequest(request, p);
                        if (val != null)
                            parameters[p.ParameterName] = val;
                        else
                        {
                            if (p.ParameterType.IsValueType && p.ParameterType.IsNullableType() == false)
                                throw new ArgumentException("未能找到指定的参数值：" + p.ParameterName);
                        }
                    }
                    else
                    {
                        // 自定义的类型。首先创建实例，然后给所有成员赋值。
                        // 注意：这里不支持嵌套类型的自定义类型。
                        //object item = Activator.CreateInstance(paramterType);
                        object item = paramterType.GetConstructor(Type.EmptyTypes).FastCreate();
                        FillModel(request, item, p);
                        parameters[p.ParameterName] = item;
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// 根据HttpRequest填充一个数据实体。
        /// 这里不支持嵌套类型的数据实体，且要求各数据成员都是简单的数据类型。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        private static void FillModel(HttpRequest request, object model, ParameterDescriptor parameterDescriptor)
        {
            ModelDescriptor descripton = MetadataHelper.GetModelDescripton(model.GetType());
            var func = ParameterDescriptor.GetPropertyFilter(parameterDescriptor);

            object val = null;
            foreach (DataMember field in descripton.Fields)
            {
                if (field.Ignore)
                    continue;

                if (!func(field.Name))//在参数上面标记了排除
                    continue;

                // 这里的实现方式不支持嵌套类型的数据实体。
                // 如果有这方面的需求，可以将这里改成递归的嵌套调用。
                val = GetValueByNameAndTypeFrommRequest(request, parameterDescriptor);
                if (val != null)
                    field.SetValue(model, val);
            }
        }


        /// <summary>
        /// 读取一个HTTP参数值。这里只读取QueryString以及Form
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string[] GetHttpValues(HttpRequest request, string name)
        {
            string[] val = request.QueryString.GetValues(name);
            if (val == null)
                val = request.Form.GetValues(name);

            return val;
        }
        
        private static object GetValueByNameAndTypeFrommRequest(HttpRequest request, ParameterDescriptor parameterDescriptor)
        {
            MethodInfo stringImplicit = null;
            Type type = parameterDescriptor.ParameterType.GetRealType();
            string name = parameterDescriptor.ParameterName;

            // 检查是否为不支持的参数类型
            if (type.IsSupportableType() == false)
            {
                // 检查是否可以做隐式类型转换
                stringImplicit = ModelHelper.GetStringImplicit(type);

                if (stringImplicit == null)
                    return null;
            }

            string[] val = GetValueFromHttpRequest(request, name, parameterDescriptor.BindingInfo.Prefix);

            if (type == typeof(string[]))
                return val;

            if (val == null || val.Length == 0)
                return null;

            // 还原ASP.NET的默认数据格式
            string str = val.Length == 1 ? val[0] : string.Join(",", val);

            // 可以做隐式类型转换
            if (stringImplicit != null)
                return stringImplicit.FastInvoke(null, str.Trim());


            return ModelHelper.SafeChangeType(str.Trim(), type);
        }

        /// <summary>
        /// 在请求中获取值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="name"></param>
        /// <param name="Prefix">前缀</param>
        /// <returns></returns>
        private static string[] GetValueFromHttpRequest(HttpRequest request, string name, string Prefix)
        {
            string[] val = GetHttpValues(request, Prefix + name);
            if (val == null)
            {
                val = GetHttpValues(request, name);
            }
            return val;
        }
    }
}
