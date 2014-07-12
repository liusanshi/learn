using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public abstract class BaseMvcHandlerFactory : IHttpHandlerFactory
    {
        public abstract RequestContext CreateRequestContext(HttpContext context, string path);

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            // 说明：这里不使用virtualPath变量，因为不同的配置，这个变量的值会不一样。
            // 例如：/Ajax/*/*.aspx 和 /Ajax/*
            // 为了映射HTTP处理器，下面直接使用context.Request.Path

            string vPath = UrlHelper.GetRealVirtualPath(context, context.Request.Path);

            // 根据请求路径，定位到要执行的Action
            RequestContext requestContext = CreateRequestContext(context, vPath);
            requestContext.RouteData.Url = url;

            return new MvcHandlerBuilder().Create(requestContext);
        }

        /// <summary>
        /// 创建RouteData
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected Route CreateRouteData(string controller, string action)
        {
            var r = new Route("");
            r.Controller = controller;
            r.Action = action;
            return r;
        }

        public virtual void ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
