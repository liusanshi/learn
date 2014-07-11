using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 动作结果为模板视图
    /// </summary>
    public class TemplateViewResult : ActionResult
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
        public TemplateViewResult(string virtualPath)
            : this(virtualPath, null)
        {
        }
        /// <summary>
        /// 创建 PageResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="model"></param>
        public TemplateViewResult(string virtualPath, object model)
        {
            this.VirtualPath = virtualPath;
            this.Model = model;
        }

        private TempDataDictionary _tempData = null;
        /// <summary>
        /// 视图中的临时数据
        /// </summary>
        public TempDataDictionary TempData
        {
            get
            {
                if (_tempData == null)
                {
                    _tempData = new TempDataDictionary();
                }
                return _tempData;
            }
        }
        /// <summary>
        /// 创建视图的上下文
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private ViewContext CreateViewContext(ControllerContext context)
        {
            var vc = new ViewContext(context, Model, TempData, context.HttpContext.Response.Output)
            {
                TemplateViewType = GetTemplateViewType()
            };
            if (vc.TemplateViewType == TemplateViewType.Page && string.IsNullOrEmpty(VirtualPath))
                vc.TemplatePath = context.HttpContext.Request.FilePath;
            else
                vc.TemplatePath = VirtualPath;
            return vc;
        }
        /// <summary>
        /// 获取模板类型
        /// </summary>
        /// <returns></returns>
        private TemplateViewType GetTemplateViewType()
        {
            if (string.IsNullOrEmpty(VirtualPath)) return TemplateViewType.Page;
            if (VirtualPath.EndsWith(".ascx", StringComparison.OrdinalIgnoreCase)) return TemplateViewType.UserControl;
            return TemplateViewType.Page;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var viewContext = CreateViewContext(context);
            context.HttpContext.Response.ContentType = "text/html";

            TemplateViewExecutor.Render(viewContext);

            //string html = PageExecutor.Render(context.HttpContext, VirtualPath, Model);
            //context.HttpContext.Response.Write(html);

            //if (context == null)
            //{
            //    throw new ArgumentNullException("context");
            //}

            //context.HttpContext.Response.ContentType = "text/html";
            //string html = UcExecutor.Render(VirtualPath, Model);
            //context.HttpContext.Response.Write(html);
        }
    }
}
