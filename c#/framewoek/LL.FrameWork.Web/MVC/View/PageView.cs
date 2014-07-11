using System;
using System.Globalization;
using System.IO;

using LL.FrameWork.Core.Infrastructure.IOC;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 页面视图
    /// </summary>
    public class PageView : DynamicView
    {
        protected PageView(ControllerContext controllerContext, string viewPath, string masterPath)
            : this(controllerContext, viewPath, masterPath, null)
        {
        }
        protected PageView(ControllerContext controllerContext, string viewPath, string masterPath, IViewPageActivator viewPageActivator)
            : this(controllerContext, viewPath, masterPath, viewPageActivator, null)
        {
        }
        public PageView(ControllerContext controllerContext, string viewPath, string masterPath, IViewPageActivator viewPageActivator, IDependencyResolver dependencyResolver)
            : base(controllerContext, viewPath, viewPageActivator, dependencyResolver)
        {
            MasterPath = masterPath;
        }

        /// <summary>
        /// 母版页路径
        /// </summary>
        public string MasterPath
        {
            get;
            private set;
        }
        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            ViewPageBae viewPage = instance as ViewPageBae;
            if (viewPage != null)
            {
                this.RenderViewPage(viewContext, viewPage);
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "视图:{0}必须派生于ViewPageBae, ViewPageBae<TModel>", new object[]
			    {
				    base.ViewPath
			    }));
            }
        }

        private void RenderViewPage(ViewContext context, ViewPageBae page)
        {
            if (!string.IsNullOrEmpty(this.MasterPath))
            {
                page.MasterPageFile = this.MasterPath;
            }
            page.ViewContext = context;
            page.RenderView(context);
        }
    }
}
