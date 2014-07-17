using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// html 帮助类
    /// </summary>
    public class HTMLHelper
    {
        public HTMLHelper(ViewContext viewContext)
        {
            ViewContext = viewContext;
        }

        /// <summary>
        /// 视图上下文
        /// </summary>
        public ViewContext ViewContext { get; private set; }

        /// <summary>
        /// 将字符串最小限度地转换为 HTML 编码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string AttributeEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return HttpUtility.HtmlAttributeEncode(value);
        }
        /// <summary>
        /// 将字符串最小限度地转换为 HTML 编码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string AttributeEncode(object value)
        {
            return this.AttributeEncode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// 将对象的字符串表示形式转换为 HTML 编码的字符串，并返回编码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Encode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return HttpUtility.HtmlEncode(value);
        }
        /// <summary>
        /// 将对象的字符串表示形式转换为 HTML 编码的字符串，并返回编码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Encode(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return HttpUtility.HtmlEncode(value);
        }
    }
}
