using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Web.MVC
{
    public static class ResponseWriter
    {
        /// <summary>
        /// 用指定的Page以及视图数据呈现结果（HTML），
        /// 然后将产生的HTML代码写入HttpContext.Current.Response
        /// 用户控件应从ViewPageBase&lt;T&gt;继承
        /// </summary>
        /// <param name="pageVirtualPath">Page的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <param name="flush">是否需要在输出html后调用Response.Flush()</param>
        public static void WritePage(this ControllerBase controller, string pageVirtualPath, object model, bool flush)
        {
            var context = controller.ControllerContext;
            if (context.HttpContext == null)
                return;

            var viewcontext = ViewContext.CreateViewContext(context, model, null);
            viewcontext.TemplatePath = pageVirtualPath;

            TemplateViewExecutor.PageRender(viewcontext);
            if (flush)
                context.HttpContext.Response.Flush();
        }

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果（HTML），
        /// 然后将产生的HTML代码写入HttpContext.Current.Response
        /// 用户控件应从MyUserControlView&lt;T&gt;继承
        /// </summary>
        /// <param name="ucVirtualPath">用户控件的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <param name="flush">是否需要在输出html后调用Response.Flush()</param>
        public static void WriteUserControl(this ControllerBase controller, string ucVirtualPath, object model, bool flush)
        {
            var context = controller.ControllerContext;
            if (context.HttpContext == null)
                return;

            var viewcontext = ViewContext.CreateViewContext(context, model, null);
            viewcontext.TemplatePath = ucVirtualPath;

            TemplateViewExecutor.UCRender(viewcontext);
            if (flush)
                context.HttpContext.Response.Flush();
        }

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果（HTML），
        /// 并生成一段代码，用于将生成的HTML替换哪个DOM节点，
        /// 然后将所有产生的HTML代码写入HttpContext.Current.Response
        /// 用户控件应从ViewUserControlBase&lt;T&gt;继承
        /// </summary>
        /// <param name="ucVirtualPath">用户控件的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <param name="targetDomId">需要将输出内容替换哪个DOM节点的内容</param>
        /// <param name="flush">是否需要在输出html后调用Response.Flush()</param>
        public static void WriteUserControl(this ControllerBase controller, string ucVirtualPath, object model, string targetDomId, bool flush)
        {
            var context = controller.ControllerContext;
            if (context.HttpContext == null)
                return;

            if (string.IsNullOrEmpty(context.HttpContext.Response.ContentType))
                context.HttpContext.Response.ContentType = "text/html";

            var viewcontext = ViewContext.CreateViewContext(context, model, null);
            viewcontext.TemplatePath = ucVirtualPath;

            string html = TemplateViewExecutor.UCHTML(viewcontext);

            string tempId = "div-" + Guid.NewGuid().ToString();

            context.HttpContext.Response.Write(string.Format("<div id=\"{0}\" style=\"display: none\">", tempId));

            context.HttpContext.Response.Write(html);

            context.HttpContext.Response.Write(string.Format("</div><script type=\"text/javascript\">document.getElementById(\"{0}\").innerHTML = document.getElementById(\"{1}\").innerHTML;</script>",
                targetDomId, tempId));

            if (flush)
                context.HttpContext.Response.Flush();
        }

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果（HTML），
        /// 然后将产生的HTML代码写入HttpContext.Current.Response
        /// 用户控件应从ViewUserControlBase&lt;T&gt;继承
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ucVirtualPath"></param>
        /// <param name="getModel"></param>
        /// <param name="flush"></param>
        public static void WriteCacheUserControl(this ControllerBase controller, string ucVirtualPath, Func<object> getModel, bool flush)
        {
            var context = controller.ControllerContext;
            if (context.HttpContext == null)
                return;

            var viewcontext = CacheViewContext.CreateCacheViewContext(context, getModel, null);
            viewcontext.TemplatePath = ucVirtualPath;

            TemplateViewExecutor.UCCacheRender(viewcontext);
            if (flush)
                context.HttpContext.Response.Flush();
        }

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果（HTML），
        /// 并生成一段代码，用于将生成的HTML替换哪个DOM节点，
        /// 然后将所有产生的HTML代码写入HttpContext.Current.Response
        /// 用户控件应从ViewUserControlBase&lt;T&gt;继承
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ucVirtualPath"></param>
        /// <param name="getModel"></param>
        /// <param name="targetDomId"></param>
        /// <param name="flush"></param>
        public static void WriteCacheUserControl(this ControllerBase controller, string ucVirtualPath, Func<object> getModel, string targetDomId, bool flush)
        {
            var context = controller.ControllerContext;
            if (context.HttpContext == null)
                return;

            if (string.IsNullOrEmpty(context.HttpContext.Response.ContentType))
                context.HttpContext.Response.ContentType = "text/html";

            var viewcontext = CacheViewContext.CreateCacheViewContext(context, getModel, null);
            viewcontext.TemplatePath = ucVirtualPath;

            string html = TemplateViewExecutor.UCCacheHTML(viewcontext);

            string tempId = "div-" + Guid.NewGuid().ToString();

            context.HttpContext.Response.Write(string.Format("<div id=\"{0}\" style=\"display: none\">", tempId));

            context.HttpContext.Response.Write(html);

            context.HttpContext.Response.Write(string.Format("</div><script type=\"text/javascript\">document.getElementById(\"{0}\").innerHTML = document.getElementById(\"{1}\").innerHTML;</script>",
                targetDomId, tempId));

            if (flush)
                context.HttpContext.Response.Flush();
        }
    }
}
