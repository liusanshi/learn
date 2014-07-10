using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 用户控件作为视图
    /// </summary>
    public class UcResult : ActionResult
    {
        /// <summary>
        /// 用户控件的路径
        /// </summary>
        public string VirtualPath { get; private set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Model { get; private set; }
        /// <summary>
        /// 创建一个UcResult 对象
        /// </summary>
        /// <param name="virtualPath"></param>
        public UcResult(string virtualPath)
            : this(virtualPath, null)
        {
        }
        /// <summary>
        /// 创建一个UcResult 对象
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="model"></param>
        public UcResult(string virtualPath, object model)
        {
            if (string.IsNullOrEmpty(virtualPath))
                throw new ArgumentNullException("virtualPath");

            this.VirtualPath = virtualPath;
            this.Model = model;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            context.HttpContext.Response.ContentType = "text/html";
            string html = UcExecutor.Render(VirtualPath, Model);
            context.HttpContext.Response.Write(html);
        }
    }
}
