using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 返回页面
    /// </summary>
    public class PageResult : ActionResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public object Model { get; set; }
        /// <summary>
        /// 页面路径
        /// </summary>
        public string VirtualPath { get; private set; }
        /// <summary>
        /// 创建 PageResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        public PageResult(string virtualPath)
            : this(virtualPath, null)
        {
        }
        /// <summary>
        /// 创建 PageResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="model"></param>
        public PageResult(string virtualPath, object model)
        {
            this.VirtualPath = virtualPath;
            this.Model = model;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(this.VirtualPath))
                this.VirtualPath = context.HttpContext.Request.FilePath;

            context.HttpContext.Response.ContentType = "text/html";
            string html = PageExecutor.Render(context.HttpContext, VirtualPath, Model);
            context.HttpContext.Response.Write(html);
        }
    }
}
