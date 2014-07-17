using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Web.MVC
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
        public CacheTemplateViewResult(string virtualPath)
            : this(virtualPath, null)
        {
        }
        /// <summary>
        /// 创建 CacheTemplateViewResult 实例
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="getModel"></param>
        public CacheTemplateViewResult(string virtualPath, Func<object> getModel)
            : base(virtualPath, null)
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

            var viewContext = CacheViewContext.CreateCacheViewContext(context, GetModel, null);
            viewContext.TemplatePath = VirtualPath;
            context.HttpContext.Response.ContentType = "text/html";
            TemplateViewExecutor.UCCacheRender(viewContext);
        }
    }
}
