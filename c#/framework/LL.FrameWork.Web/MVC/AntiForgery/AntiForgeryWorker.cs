using System;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 防伪工作者
    /// </summary>
    internal class AntiForgeryWorker
    {
        /// <summary>
        /// 创建 AntiForgeryWorker 对象
        /// </summary>
        public AntiForgeryWorker()
        {
            this.Serializer = new AntiForgeryDataSerializer();
        }
        /// <summary>
        /// 防伪标识序列化器
        /// </summary>
        internal AntiForgeryDataSerializer Serializer
        {
            get;
            set;
        }
        private static HttpAntiForgeryException CreateValidationException()
        {
            return new HttpAntiForgeryException("防伪标识验证失败！");
        }
        /// <summary>
        /// 给页面打上防伪标识
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="salt"></param>
        /// <param name="domain"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public String GetHtml(HttpContext httpContext, string salt, string domain, string path)
        {
            string antiForgeryTokenAndSetCookie = this.GetAntiForgeryTokenAndSetCookie(httpContext, salt, domain, path);
            string antiForgeryTokenName = AntiForgeryData.GetAntiForgeryTokenName(null);
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.Attributes["type"] = "hidden";
            tagBuilder.Attributes["name"] = antiForgeryTokenName;
            tagBuilder.Attributes["value"] = antiForgeryTokenAndSetCookie;
            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }
        /// <summary>
        /// 获取、设置防伪标识
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="salt"></param>
        /// <param name="domain"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetAntiForgeryTokenAndSetCookie(HttpContext httpContext, string salt, string domain, string path)
        {
            string antiForgeryTokenName = AntiForgeryData.GetAntiForgeryTokenName(httpContext.Request.ApplicationPath);
            AntiForgeryData antiForgeryData = null;
            HttpCookie httpCookie = httpContext.Request.Cookies[antiForgeryTokenName];
            if (httpCookie != null)
            {
                try
                {
                    antiForgeryData = this.Serializer.Deserialize(httpCookie.Value);
                }
                catch
                {
                }
            }
            if (antiForgeryData == null)
            {
                antiForgeryData = AntiForgeryData.NewToken();
                string value = this.Serializer.Serialize(antiForgeryData);
                HttpCookie httpCookie2 = new HttpCookie(antiForgeryTokenName, value)
                {
                    HttpOnly = true,
                    Domain = domain
                };
                if (!string.IsNullOrEmpty(path))
                {
                    httpCookie2.Path = path;
                }
                httpContext.Response.Cookies.Set(httpCookie2);
            }
            AntiForgeryData token = new AntiForgeryData(antiForgeryData)
            {
                Salt = salt,
                Username = AntiForgeryData.GetUsername(httpContext.User)
            };
            return this.Serializer.Serialize(token);
        }
        /// <summary>
        /// 验证请求
        /// </summary>
        /// <param name="context"></param>
        /// <param name="salt"></param>
        public void Validate(HttpContext context, string salt)
        {
            string formTokenName = AntiForgeryData.GetAntiForgeryTokenName(null);
            string cookieTokenName = AntiForgeryData.GetAntiForgeryTokenName(context.Request.ApplicationPath);
            HttpCookie httpCookie = context.Request.Cookies[cookieTokenName];
            if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
            {
                throw AntiForgeryWorker.CreateValidationException();
            }
            AntiForgeryData cookieAntiForgeryData = this.Serializer.Deserialize(httpCookie.Value);//从客户端cookie传来的防伪标识
            string text = context.Request.Form[formTokenName];
            if (string.IsNullOrEmpty(text))
            {
                throw AntiForgeryWorker.CreateValidationException();
            }
            AntiForgeryData formAntiForgeryData = this.Serializer.Deserialize(text);//从表单里面传递过来的防伪标识
            if (!string.Equals(cookieAntiForgeryData.Value, formAntiForgeryData.Value, StringComparison.Ordinal))
            {
                throw AntiForgeryWorker.CreateValidationException();
            }
            string username = AntiForgeryData.GetUsername(context.User);//当前的用户名
            if (!string.Equals(formAntiForgeryData.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                throw AntiForgeryWorker.CreateValidationException();
            }
            if (!string.Equals(salt ?? string.Empty, formAntiForgeryData.Salt, StringComparison.Ordinal))
            {
                throw AntiForgeryWorker.CreateValidationException();
            }
        }
    }
}
