using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Compilation;

using LL.FrameWork.Core.Infrastructure.IOC;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 动态视图
    /// </summary>
    public abstract class DynamicView : IView
    {
        private ControllerContext _controllerContext;
        internal IViewPageActivator ViewPageActivator;
        /// <summary>
        /// 视图路径
        /// </summary>
        public string ViewPath
        {
            get;
            protected set;
        }
        protected DynamicView(ControllerContext controllerContext, string viewPath)
            : this(controllerContext, viewPath, null)
        {
        }
        protected DynamicView(ControllerContext controllerContext, string viewPath, IViewPageActivator viewPageActivator)
            : this(controllerContext, viewPath, viewPageActivator, null)
        {
        }
        internal DynamicView(ControllerContext controllerContext, string viewPath, IViewPageActivator viewPageActivator, IDependencyResolver dependencyResolver)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewPath))
            {
                throw new ArgumentException("值不能为空或者null", "viewPath");
            }
            this._controllerContext = controllerContext;
            this.ViewPath = viewPath;
            this.ViewPageActivator = (viewPageActivator ?? new DefaultViewPageActivator(dependencyResolver));
        }
        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="writer"></param>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            object obj = null;
            Type compiledType = BuildManager.GetCompiledType(this.ViewPath);
            if (compiledType != null)
            {
                obj = this.ViewPageActivator.Create(this._controllerContext, compiledType);
            }
            if (obj == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "视图:{0}没有创建成功", new object[]
				{
					this.ViewPath
				}));
            }
            this.RenderView(viewContext, writer, obj);
        }
        protected abstract void RenderView(ViewContext viewContext, TextWriter writer, object instance);
    }
}
