using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 用于描述一个Action可以处理哪些请求路径。
    /// 注意：这个Attribute可以多次使用，表示可以处理多个请求路径。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PageUrlAttribute : Attribute, IUniquelyIdentifiable
    {
        protected PageUrlAttribute() { }
        /// <summary>
        /// 用于描述一个Action可以处理哪些请求路径。
        /// </summary>
        /// <param name="url"></param>
        public PageUrlAttribute(string url) : this(url, "GET") { }
        /// <summary>
        /// 用于描述一个Action可以处理哪些请求路径。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="verb"></param>
        public PageUrlAttribute(string url, string verb)
        {
            Url = url;
            Verb = verb;
        }

        /// <summary>
        /// 指示可以处理的请求路径。比如："/abc.aspx" 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 允许哪些访问动词，与web.config中的httpHanlder的配置意义一致。
        /// </summary>
        public string Verb { get; set; }

        /// <summary>
        /// 获取唯一标识
        /// </summary>
        public string UniqueId
        {
            get { return ConvertToUniqueId(Url, Verb); }
        }

        /// <summary>
        /// 将url、verb转换为唯一标识
        /// </summary>
        /// <param name="url"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        internal static string ConvertToUniqueId(string url, string verb)
        {
            return string.Format("{0}+{1}", url, verb); 
        }
    }
}
