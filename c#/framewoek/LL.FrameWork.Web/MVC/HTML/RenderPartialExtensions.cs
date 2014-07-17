using System;
using System.Globalization;
using System.IO;

namespace LL.Framework.Web.MVC
{
    public static class RenderPartialExtensions
    {
        /// <summary>
        /// 直接呈现部分试图的执行结果
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewPath"></param>
        /// <returns></returns>
        public static void RenderPartial(this HTMLHelper htmlHelper, string partialViewPath)
        {
            RenderPartialExtensions.RenderPartial(htmlHelper, partialViewPath, null);
        }
        /// <summary>
        /// 直接呈现部分试图的执行结果
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="partialViewPath"></param>
        /// <param name="model"></param>
        public static void RenderPartial(this HTMLHelper htmlHelper, string partialViewPath, object model)
        {
            if (htmlHelper.ViewContext == null)
                throw new InvalidOperationException(string.Format("页面:{0} 的ViewContext 为null"));
            new CacheUserControlView(htmlHelper.ViewContext, partialViewPath).Render(htmlHelper.ViewContext, htmlHelper.ViewContext.Writer);
        }
    }
}
