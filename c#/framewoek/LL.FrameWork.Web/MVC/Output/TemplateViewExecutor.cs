using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

using LL.FrameWork.Core.Reflection;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 模板视图执行者
    /// </summary>
    public static class TemplateViewExecutor
    {
        /// <summary>
        /// Page页面的
        /// </summary>
        private static FieldInfo Page_request = typeof(Page).GetField("_request", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// 执行模板视图
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void Render(ViewContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            switch (context.TemplateViewType)
            {
                default:
                case TemplateViewType.Page:
                    PageRender(context);
                    break;
                case TemplateViewType.UserControl:
                    UCRender(context);
                    break;
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
        internal static void PageRender(ViewContext context)
        {
            string pageVirtualPath = context.TemplatePath;
            if (string.IsNullOrEmpty(pageVirtualPath))
                throw new ArgumentNullException("pageVirtualPath");


            Page handler = BuildManager.CreateInstanceFromVirtualPath(pageVirtualPath, typeof(object)) as Page;
            if (handler == null)
                throw new InvalidOperationException(
                    string.Format("指定的路径 {0} 不是一个有效的页面。", pageVirtualPath));

            ViewPageBae page = handler as ViewPageBae;
            if (page != null)
                page.ViewContext = context;

            context.HttpContext.Server.Execute(handler, context.Writer, false);
        }

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果，最后返回生成的HTML代码。
        /// 用户控件应从MyUserControlView&lt;T&gt;继承
        /// </summary>
        /// <param name="ucVirtualPath">用户控件的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <returns>生成的HTML代码</returns>
        internal static void UCRender(ViewContext context)
        {
            string ucVirtualPath = context.TemplatePath;
            if (string.IsNullOrEmpty(ucVirtualPath))
                throw new ArgumentNullException("ucVirtualPath");

            Page page = new Page();
            Control ctl = page.LoadControl(ucVirtualPath);
            if (ctl == null)
                throw new InvalidOperationException(
                    string.Format("指定的用户控件 {0} 没有找到。", ucVirtualPath));

            // 将用户控件放在Page容器中。
            page.Controls.Add(ctl);

            if (context.Model != null)
            {
                PartialCachingControl mycachectl = ctl as PartialCachingControl;//如果有 写outputcache指令
                Control temp = ctl;
                if (mycachectl != null)
                {
                    Page_request.FastSetValue(page, context.HttpContext.Request);//将当前的Request 写入当前页面
                    page.DesignerInitialize();
                    temp = mycachectl.CachedControl;
                }
                ViewUserControl myctl = temp as ViewUserControl;
                if (myctl != null)
                    myctl.ViewContext = context;
            }

            HtmlTextWriter write = new HtmlTextWriter(context.Writer, string.Empty);
            page.RenderControl(write);

            // 用下面的方法也可以的。目前是不行的
            //HttpContext.Current.Server.Execute(page, output, false);
        }
    }
}
