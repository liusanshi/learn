using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public class ControllerContext
    {
        private HttpContext _httpcontext;
        private Route _routeData;
        private RequestContext _requestContext;
        /// <summary>
        /// 当前请求的 Controller
        /// </summary>
        public virtual ControllerBase Controller
        {
            get;
            set;
        }
        /// <summary>
        /// 当前请求的上下文
        /// </summary>
        public HttpContext HttpContext
        {
            get
            {
                if (_httpcontext == null)
                {
                    _httpcontext = HttpContext.Current;
                }
                return _httpcontext;
            }
            set
            {
                _httpcontext = value;
            }
        }
        /// <summary>
        /// 路由数据
        /// </summary>
        public Route RouteData
        {
            get
            {
                if (_routeData == null)
                {
                    this._routeData = ((this._requestContext != null) ? this._requestContext.RouteData : new Route());
                }
                return _routeData;
            }
            set
            {
                _routeData = value;
            }
        }
        public RequestContext RequestContext
        {
            get
            {
                if (this._requestContext == null)
                {
                    this._requestContext = new RequestContext(HttpContext, RouteData);
                }
                return this._requestContext;
            }
            set
            {
                this._requestContext = value;
            }
        }

        public ControllerContext() { }
        public ControllerContext(RequestContext requestContext, ControllerBase controller)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            this.Controller = controller;
            this.RequestContext = requestContext;
        }
        protected ControllerContext(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            this.Controller = controllerContext.Controller;
            this.HttpContext = controllerContext.HttpContext;
            this.RequestContext = controllerContext.RequestContext;
        }
    }
}
