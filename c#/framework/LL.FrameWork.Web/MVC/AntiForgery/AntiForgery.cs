namespace LL.Framework.Web.MVC
{
    using System;
    using System.Web;

    /// <summary>
    /// 防伪
    /// </summary>
    public static class AntiForgery
    {
        /// <summary>
        /// 防伪工作者
        /// </summary>
        private static readonly AntiForgeryWorker _worker = new AntiForgeryWorker();
        /// <summary>
        /// 给页面打上防伪标识
        /// </summary>
        /// <returns></returns>
        public static String GetHtml()
        {
            return AntiForgery.GetHtml(HttpContext.Current, null, null, null);
        }
        /// <summary>
        /// 给页面打上防伪标识
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="salt"></param>
        /// <param name="domain"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String GetHtml(HttpContext httpContext, string salt, string domain, string path)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            return AntiForgery._worker.GetHtml(httpContext, salt, domain, path);
        }
        /// <summary>
        /// 验证防伪标识
        /// </summary>
        public static void Validate()
        {
            AntiForgery.Validate(HttpContext.Current, null);
        }
        /// <summary>
        /// 验证防伪标识
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="salt"></param>
        public static void Validate(HttpContext httpContext, string salt)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            AntiForgery._worker.Validate(httpContext, salt);
        }
    }
}
