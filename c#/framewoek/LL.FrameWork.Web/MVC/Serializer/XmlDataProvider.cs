using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

using LL.FrameWork.Core.Reflection;

namespace LL.FrameWork.Web.MVC.Serializer
{
    internal class XmlDataProvider : IActionParametersProvider
    {
        public IDictionary<string, object> GetParameters(HttpRequest request, ActionDescriptor action)
        {
            var Parameters = action.GetParameters();

            if (Parameters.Length == 1)
            {
                object value = GetObjectFromRequest(request, Parameters[0].ParameterType.GetRealType());
                return new Dictionary<string, object> { { Parameters[0].ParameterName, value } };
            }
            else
            {
                return GetMultiObjectsFormRequest(request, action);
            }
        }


        private object GetObjectFromRequest(HttpRequest request, Type destType)
        {
            XmlSerializer mySerializer = new XmlSerializer(destType);

            request.InputStream.Position = 0;
            StreamReader sr = new StreamReader(request.InputStream, request.ContentEncoding);
            return mySerializer.Deserialize(sr);
        }

        private IDictionary<string, object> GetMultiObjectsFormRequest(HttpRequest request, ActionDescriptor action)
        {
            string xml = request.ReadInputStream();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode root = doc.LastChild;

            //if( root.ChildNodes.Count != action.Parameters.Length )
            //    throw new ArgumentException("客户端提交的数据项与服务端的参数项的数量不匹配。");

            var Parameters = action.GetParameters();
            Dictionary<string, object> parameters = new Dictionary<string, object>(Parameters.Length);

            for (int i = 0; i < Parameters.Length; i++)
            {
                var parameterDescriptor = Parameters[i];
                string name = parameterDescriptor.ParameterName;
                XmlNode node = (from n in root.ChildNodes.Cast<XmlNode>()
                                where string.Compare(n.Name, parameterDescriptor.BindingInfo.Prefix + name, StringComparison.OrdinalIgnoreCase) == 0
                                select n).FirstOrDefault();

                if (node != null)
                {
                    object parameter = null;
                    Type destType = parameterDescriptor.ParameterType.GetRealType();

                    if (destType.IsSupportableType())
                        parameter = ModelHelper.SafeChangeType(node.InnerText, destType);
                    else
                        parameter = XmlDeserialize(node.OuterXml, destType, request.ContentEncoding);

                    if (parameter == null) parameter = parameterDescriptor.DefaultValue;
                    parameters[name] = parameter;
                }
            }

            return parameters;
        }


        /// <summary>
        /// 将xml序列化为对象
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="destType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static object XmlDeserialize(string xml, Type destType, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentNullException("xml");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(destType);
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xml)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return mySerializer.Deserialize(sr);
                }
            }
        }

    }
}
