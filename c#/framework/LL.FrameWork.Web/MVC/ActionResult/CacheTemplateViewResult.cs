using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 动作结果为缓存模板视图 Output指令
    /// </summary>
    public class CacheTemplateViewResult : TemplateViewResult
    {
        /// <summary>
        /// 创建 CacheTemplateViewResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        public CacheTemplateViewResult(string virtualPath, ViewDataDictionary viewData)
            : this(virtualPath, null, viewData, null)
        {
        }
        /// <summary>
        /// 创建 CacheTemplateViewResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="getModel"></param>
        /// <param name="viewData"></param>
        /// <param name="tempData"></param>
        public CacheTemplateViewResult(string virtualPath, Func<object> getModel, ViewDataDictionary viewData, TempDataDictionary tempData)
            : base(virtualPath, viewData, tempData)
        {
            if (getModel == null)
            {
                GetModel = () => null;
            }
            else
            {
                this.GetModel = getModel;
            }
        }

        private Func<object> GetModel;
        private bool isCalac = false;
        private object model = null;

        /// <summary>
        /// 视图的model
        /// </summary>
        public override object Model
        {
            get
            {
                if (!isCalac)
                {
                    model = GetModel();
                    isCalac = true;
                }
                return model;
            }
            set
            {
                model = value;
                isCalac = true;
            }
        }

        /// <summary>
        /// 执行动作结果
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var viewContext = CacheViewContext.CreateCacheViewContext(context, GetModel, ViewData, TempData);
            viewContext.TemplatePath = VirtualPath;
            context.HttpContext.Response.ContentType = "text/html";
            TemplateViewExecutor.UCCacheRender(viewContext);
        }
    }
}
