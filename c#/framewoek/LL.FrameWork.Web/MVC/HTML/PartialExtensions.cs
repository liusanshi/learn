using System;
using System.Globalization;
using System.IO;

namespace LL.FrameWork.Web.MVC
{
    public static class PartialExtensions
    {
        /// <summary>
        /// 返回部分试图的执行 HTML结果
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewPath"></param>
        /// <returns></returns>
        public static string Partial(this HTMLHelper htmlHelper, string partialViewPath)
        {
            return PartialExtensions.Partial(htmlHelper, partialViewPath, null);
        }
        /// <summary>
        /// 返回部分试图的执行 HTML结果
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewPath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Partial(this HTMLHelper htmlHelper, string partialViewPath, object model)
        {
            if (htmlHelper.ViewContext == null)
                throw new InvalidOperationException(string.Format("页面:{0} 的ViewContext 为null"));
            using (StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture))
            {
                htmlHelper.ViewContext.Model = model;
                new CachingUserControlView(htmlHelper.ViewContext, partialViewPath).Render(htmlHelper.ViewContext, stringWriter);
                return stringWriter.ToString();
            }
        }
    }
}
