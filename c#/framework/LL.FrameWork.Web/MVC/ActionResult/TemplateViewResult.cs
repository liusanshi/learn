using System;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 动作结果为模板视图
    /// </summary>
    public class TemplateViewResult : ActionResult
    {
        /// <summary>
        /// 页面路径
        /// </summary>
        public string VirtualPath { get; private set; }
        /// <summary>
        /// 创建 PageResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="tempData"></param>
        public TemplateViewResult(string virtualPath, ViewDataDictionary viewData)
            : this(virtualPath, viewData, null)
        {
        }
        /// <summary>
        /// 创建 PageResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="model"></param>
        public TemplateViewResult(string virtualPath, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            this.VirtualPath = virtualPath;
            _tempData = tempData;
            _viewData = viewData;
        }

        private TempDataDictionary _tempData = null;
        private ViewDataDictionary _viewData = null;
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
        /// 视图中的数据
        /// </summary>
        public ViewDataDictionary ViewData
        {
            get
            {
                return _viewData;
            }
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

            var viewContext = ViewContext.CreateViewContext(context, ViewData, TempData);
            viewContext.TemplateViewType = GetTemplateViewType();
            viewContext.TemplatePath = VirtualPath;

            context.HttpContext.Response.ContentType = "text/html";
            TemplateViewExecutor.Render(viewContext);
        }
    }
}
