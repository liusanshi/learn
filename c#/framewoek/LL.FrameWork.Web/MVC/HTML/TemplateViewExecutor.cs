using System;
using System.IO;
using System.Text;
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
        /// 页面应从ViewPageBase&lt;T&gt;继承
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="pageVirtualPath">Page的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <returns>生成的HTML代码</returns>
        internal static void PageRender(ViewContext context, string masterPath)
        {
            new PageView(context, context.TemplatePath, masterPath).Render(context, context.Writer);
        }
        /// <summary>
        /// 用指定的页面路径以及视图数据呈现结果，最后返回生成的HTML代码。
        /// 页面应从ViewPageBase&lt;T&gt;继承
        /// </summary>
        /// <param name="context"></param>
        internal static void PageRender(ViewContext context)
        {
            PageRender(context, "");
        }

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果，最后返回生成的HTML代码。
        /// 用户控件应从ViewUserControlBase&lt;T&gt;继承
        /// </summary>
        /// <param name="ucVirtualPath">用户控件的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <returns>生成的HTML代码</returns>
        internal static void UCRender(ViewContext context)
        {
            new UserControlView(context, context.TemplatePath).Render(context, context.Writer);
        }
        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果，最后返回生成的HTML代码。
        /// 用户控件应从ViewUserControlBase&lt;T&gt;继承
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static String UCHTML(ViewContext context)
        {
            StringBuilder buffer = new StringBuilder(100);
            using (StringWriter sw = new StringWriter(buffer))
            {
                new UserControlView(context, context.TemplatePath).Render(context, sw);
                return buffer.ToString();
            }
        }

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果，最后返回生成的HTML代码。使用了 OutputCache 指令的用户控件
        /// 用户控件应从ViewUserControlBase&lt;T&gt;继承
        /// </summary>
        /// <param name="context"></param>
        internal static void UCCacheRender(ViewContext context)
        {
            new CacheUserControlView(context, context.TemplatePath).Render(context, context.Writer);
        }
        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果，最后返回生成的HTML代码。使用了 OutputCache 指令的用户控件
        /// 用户控件应从ViewUserControlBase&lt;T&gt;继承
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static String UCCacheHTML(ViewContext context)
        {
            StringBuilder buffer = new StringBuilder(100);
            using (StringWriter sw = new StringWriter(buffer))
            {
                new CacheUserControlView(context, context.TemplatePath).Render(context, sw);
                return buffer.ToString();
            }
        }
    }
}
