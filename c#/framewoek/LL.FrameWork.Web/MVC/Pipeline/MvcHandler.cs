using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Diagnostics;
using System.Globalization;
using System.Web.SessionState;

namespace LL.Framework.Web.MVC
{
    internal class MvcHandler : IHttpHandler
    {
        /// <summary>
        /// MvcVersion
        /// </summary>
        internal static readonly string MvcVersion = FileVersionInfo.GetVersionInfo(typeof(MvcHandler).Assembly.Location).FileVersion;
        /// <summary>
        /// MvcVersionHeaderName
        /// </summary>
        public static readonly string MvcVersionHeaderName = "X-AspNetMvc2-Version";

        /// <summary>
        /// 创建一个 MvcHandler
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerFactory"></param>
        internal MvcHandler(RequestContext requestContext, IControllerFactory controllerFactory)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            if (controllerFactory == null)
            {
                throw new ArgumentNullException("controllerFactory");
            }
            this.RequestContext = requestContext;
            this.ControllerFactory = controllerFactory;
        }

        /// <summary>
        /// 请求的上下文
        /// </summary>
        public RequestContext RequestContext
        {
            get;
            internal set;
        }
        /// <summary>
        /// 控制器创建工厂
        /// </summary>
        public IControllerFactory ControllerFactory
        {
            get;
            internal set;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            IController controller;
            this.ProcessRequestInit(context, out controller);
            try
            {
                controller.Execute(RequestContext);
            }
            finally
            {
                ControllerFactory.ReleaseController(controller);
            }
        }

        void IHttpHandler.ProcessRequest(HttpContext httpContext)
        {
            this.ProcessRequest(httpContext);
        }

        /// <summary>
        /// 在输出里面添加版本信息
        /// </summary>
        /// <param name="context"></param>
        private static void AddVersionHeader(HttpContext context)
        {
            context.Response.AppendHeader(MvcVersionHeaderName, MvcVersion);
        }

        private void ProcessRequestInit(HttpContext context, out IController controller)
        {
            AddVersionHeader(context);
            string requiredString = this.RequestContext.RouteData.Controller;
            controller = ControllerFactory.CreateController(this.RequestContext, requiredString);
            if (controller == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "IControllerFactory:{0}不能创建控制器：{1}", new object[]{
			            ControllerFactory.GetType(),
			            requiredString
		            }));
            }
        }
    }

    internal class RequiresSessionActionHandler : MvcHandler, IRequiresSessionState
    {
        public RequiresSessionActionHandler(RequestContext requestContext, IControllerFactory controllerFactory)
            : base(requestContext, controllerFactory)
        { }
    }

    internal class ReadOnlySessionActionHandler : MvcHandler, IRequiresSessionState, IReadOnlySessionState
    {
        public ReadOnlySessionActionHandler(RequestContext requestContext, IControllerFactory controllerFactory)
            : base(requestContext, controllerFactory)
        { }
    }

    /// <summary>
    /// MvcHandler创建生成器
    /// </summary>
    internal class MvcHandlerBuilder
    {
        private ControllerBuilder _controllerBuilder;
        /// <summary>
        /// 控制器的生成器
        /// </summary>
        internal ControllerBuilder ControllerBuilder
        {
            get
            {
                if (this._controllerBuilder == null)
                {
                    this._controllerBuilder = ControllerBuilder.Current;
                }
                return this._controllerBuilder;
            }
            set
            {
                this._controllerBuilder = value;
            }
        }
        /// <summary>
        /// 创建 MvcHandler
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        internal MvcHandler Create(RequestContext requestContext)
        {
            string requiredString = requestContext.RouteData.Controller;
            if (string.IsNullOrEmpty(requiredString))
            {
                throw new HttpException(404, string.Format(CultureInfo.CurrentCulture, "没有找到路径:{0}对应的控制器", new object[]
				{
					requestContext.HttpRequest.Path
				}));
            }
            var factory = ControllerBuilder.GetControllerFactory();
            SessionMode mode = factory.GetControllerSessionBehavior(requestContext, requiredString);
            switch (mode)
            {
                default:
                case SessionMode.Default:
                    return new MvcHandler(requestContext, factory);
                case SessionMode.NotSupport:
                    return new MvcHandler(requestContext, factory);
                case SessionMode.Support:
                    return new RequiresSessionActionHandler(requestContext, factory);
                case SessionMode.ReadOnly:
                    return new ReadOnlySessionActionHandler(requestContext, factory);
            }
        }
    }
}
