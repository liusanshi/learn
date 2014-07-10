using System;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;


namespace LL.FrameWork.Web.MVC
{
    public static class PageExecutor
    {
        private static void SetPageModel(IHttpHandler handler, object model)
        {
            if (handler == null)
                return;

            if (model != null)
            {
                MyBasePage page = handler as MyBasePage;
                if (page != null)
                    page.SetModel(model);
            }
        }


        /// <summary>
        /// 用指定的页面路径以及视图数据呈现结果，最后返回生成的HTML代码。
        /// 页面应从MyPageView&lt;T&gt;继承
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="pageVirtualPath">Page的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <returns>生成的HTML代码</returns>
        public static string Render(HttpContext context, string pageVirtualPath, object model)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (string.IsNullOrEmpty(pageVirtualPath))
                throw new ArgumentNullException("pageVirtualPath");


            Page handler = BuildManager.CreateInstanceFromVirtualPath(
                                            pageVirtualPath, typeof(object)) as Page;
            if (handler == null)
                throw new InvalidOperationException(
                    string.Format("指定的路径 {0} 不是一个有效的页面。", pageVirtualPath));


            SetPageModel(handler, model);

            StringWriter output = new StringWriter();
            context.Server.Execute(handler, output, false);
            return output.ToString();
        }
    }
}
