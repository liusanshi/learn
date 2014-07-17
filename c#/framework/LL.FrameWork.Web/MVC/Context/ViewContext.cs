using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 视图上下文
    /// </summary>
    public class ViewContext : ControllerContext
    {
        private string templatePath;
        protected ViewContext() : base() { }
        /// <summary>
        /// 创建 ViewContext 实例
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="model"></param>
        /// <param name="tempData"></param>
        /// <param name="writer"></param>
        public ViewContext(ControllerContext controllerContext, object model, TempDataDictionary tempData, TextWriter writer)
            : base(controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            //if (model == null)
            //{
            //    throw new ArgumentNullException("model");
            //}
            if (tempData == null)
            {
                throw new ArgumentNullException("tempData");
            }
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            this.Model = model;
            this.Writer = writer;
            this.TempData = tempData;
        }

        /// <summary>
        /// 视图数据
        /// </summary>
        public virtual object Model { get; set; }
        /// <summary>
        /// 视图结果的输入入口
        /// </summary>
        public TextWriter Writer { get; set; }
        /// <summary>
        /// 视图上面的临时数据
        /// </summary>
        public TempDataDictionary TempData { get; set; }
        /// <summary>
        /// 模板的类型
        /// </summary>
        public TemplateViewType TemplateViewType { get; set; }
        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath
        {
            get
            {
                if (TemplateViewType == MVC.TemplateViewType.Page && string.IsNullOrEmpty(templatePath))
                {
                    templatePath = HttpContext.Request.FilePath;
                }
                return templatePath;
            }
            set { templatePath = value; }
        }

        /// <summary>
        /// 创建ViewContext
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="model"></param>
        /// <param name="tempData"></param>
        /// <returns></returns>
        internal static ViewContext CreateViewContext(ControllerContext controllerContext, object model, TempDataDictionary tempData)
        {
            return new ViewContext(controllerContext, model,
                tempData ?? new TempDataDictionary(),
                controllerContext.HttpContext.Response.Output);
        }
    }

    /// <summary>
    /// 模板视图类型
    /// </summary>
    public enum TemplateViewType
    {
        /// <summary>
        /// page页面
        /// </summary>
        Page,
        /// <summary>
        /// UserControl
        /// </summary>
        UserControl
    }
}
