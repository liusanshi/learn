namespace LL.Framework.Web.MVC
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// 请求的上下文
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// 创建RequestContext对象
        /// </summary>
        protected RequestContext() { }
        /// <summary>
        /// 创建RequestContext对象
        /// </summary>
        /// <param name="httpContext"></param>
        public RequestContext(HttpContext httpContext)
        {
            HttpContext = httpContext;
            RouteData = Route.Instance;
        }
        /// <summary>
        /// 创建RequestContext对象
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="route"></param>
        public RequestContext(HttpContext httpContext, Route route)
        {
            HttpContext = httpContext;
            RouteData = route;
        }

        public HttpContext HttpContext { get; set; }

        /// <summary>
        /// HttpRequest
        /// </summary>
        public HttpRequest HttpRequest
        {
            get
            {
                if (HttpContext != null)
                {
                    return HttpContext.Request;
                }
                return null;
            }
        }

        /// <summary>
        /// 路由数据
        /// </summary>
        public Route RouteData { get; set; }
    }
}
