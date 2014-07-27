using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// HTML元素生成器
    /// </summary>
    public class TagBuilder
    {
        /// <summary>
        /// 创建 TagBuilder 对象
        /// </summary>
        /// <param name="tagName"></param>
        public TagBuilder(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentException("参数不能为空或者null", "tagName");
            }
            this.TagName = tagName;
            this.Attributes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        }

        private string _innerHtml;
        /// <summary>
        /// 属性
        /// </summary>
        public IDictionary<string, string> Attributes
        {
            get;
            private set;
        }
        /// <summary>
        /// 内部HTML
        /// </summary>
        public string InnerHtml
        {
            get
            {
                return this._innerHtml ?? string.Empty;
            }
            set
            {
                this._innerHtml = value;
            }
        }
        /// <summary>
        /// 元素Tag名称 NodeName
        /// </summary>
        public string TagName
        {
            get;
            private set;
        }
        /// <summary>
        /// 添加样式
        /// </summary>
        /// <param name="value"></param>
        public void AddCssClass(string value)
        {
            string str;
            if (this.Attributes.TryGetValue("class", out str))
            {
                this.Attributes["class"] = value + " " + str;
                return;
            }
            this.Attributes["class"] = value;
        }
        /// <summary>
        /// 生成Id
        /// </summary>
        /// <param name="value"></param>
        public void GenerateId(string value)
        {
            if (!string.IsNullOrEmpty(value) && !this.Attributes.ContainsKey("id"))
            {
                this.Attributes["id"] = value;
            }
        }
        /// <summary>
        /// 追加属性
        /// </summary>
        /// <param name="sb"></param>
        private void AppendAttributes(StringBuilder sb)
        {
            foreach (KeyValuePair<string, string> current in this.Attributes)
            {
                string key = current.Key;
                if (!string.Equals(key, "id", StringComparison.Ordinal) || !string.IsNullOrEmpty(current.Value))
                {
                    string value = HttpUtility.HtmlAttributeEncode(current.Value);
                    sb.Append(' ').Append(key).Append("=\"").Append(value).Append('"');
                }
            }
        }
        /// <summary>
        /// 合并属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void MergeAttribute(string key, string value)
        {
            this.MergeAttribute(key, value, false);
        }
        /// <summary>
        /// 合并属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="replaceExisting">已经存在是否替换</param>
        public void MergeAttribute(string key, string value, bool replaceExisting)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("参数不能为空或者null", "key");
            }
            if (replaceExisting || !this.Attributes.ContainsKey(key))
            {
                this.Attributes[key] = value;
            }
        }
        /// <summary>
        /// 合并属性
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="attributes"></param>
        public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
        {
            bool replaceExisting = false;
            this.MergeAttributes<TKey, TValue>(attributes, replaceExisting);
        }
        /// <summary>
        /// 合并属性
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="attributes"></param>
        /// <param name="replaceExisting">已经存在是否替换</param>
        public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting)
        {
            if (attributes != null)
            {
                foreach (KeyValuePair<TKey, TValue> current in attributes)
                {
                    string key = Convert.ToString(current.Key, CultureInfo.InvariantCulture);
                    string value = Convert.ToString(current.Value, CultureInfo.InvariantCulture);
                    this.MergeAttribute(key, value, replaceExisting);
                }
            }
        }
        /// <summary>
        /// 设置标签内部的文本
        /// </summary>
        /// <param name="innerText"></param>
        public void SetInnerText(string innerText)
        {
            this.InnerHtml = HttpUtility.HtmlEncode(innerText);
        }
        //internal IHtmlString ToHtmlString(TagRenderMode renderMode)
        //{
        //    return new HtmlString(this.ToString(renderMode));
        //}
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(TagRenderMode.Normal);
        }
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="renderMode"></param>
        /// <returns></returns>
        public string ToString(TagRenderMode renderMode)
        {
            StringBuilder stringBuilder = new StringBuilder();
            switch (renderMode)
            {
                case TagRenderMode.StartTag:
                    stringBuilder.Append('<').Append(this.TagName);
                    this.AppendAttributes(stringBuilder);
                    stringBuilder.Append('>');
                    break;
                case TagRenderMode.EndTag:
                    stringBuilder.Append("</").Append(this.TagName).Append('>');
                    break;
                case TagRenderMode.SelfClosing:
                    stringBuilder.Append('<').Append(this.TagName);
                    this.AppendAttributes(stringBuilder);
                    stringBuilder.Append(" />");
                    break;
                default:
                    stringBuilder.Append('<').Append(this.TagName);
                    this.AppendAttributes(stringBuilder);
                    stringBuilder.Append('>').Append(this.InnerHtml).Append("</").Append(this.TagName).Append('>');
                    break;
            }
            return stringBuilder.ToString();
        }
    }
}
