using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace LL.Framework.Web.MVC
{
    public sealed class MvcPageHandlerFactory : IHttpHandlerFactory
    {
        internal sealed class AspnetPageHandlerFactory : PageHandlerFactory { }

        /// <summary>
        /// 尝试根据当前请求，获取一个有效的Action，并返回ActionHandler
        /// 此方法可以在HttpModule中使用，用于替代httpHandler的映射配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IHttpHandler TryGetHandler(HttpContext context)
        {
            string vPath = UrlHelper.GetRealVirtualPath(context, context.Request.FilePath);

            var data = PageUrl2ControllerTypeCache.DefaultPageUrl2ControllerTypeCache
                .GetDescriptor(PageUrlAttribute.ConvertToUniqueId(vPath, context.Request.HttpMethod));
            if (data == null)
                return null;

            RequestContext requestContext = new RequestContext(context, new Route(vPath)
            {
                UsePageUrlRoute = true,
                PageUrlData = data
            });
            return new MvcHandlerBuilder().Create(requestContext);
        }

        private AspnetPageHandlerFactory _msPageHandlerFactory;

        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context,
                            string requestType, string virtualPath, string physicalPath)
        {
            // 说明：这里不使用virtualPath变量，因为不同的配置，这个变量的值会不一样。
            // 例如：/mvc/*/*.aspx 和 /mvc/*
            // 为了映射HTTP处理器，下面直接使用context.Request.Path

            string requestPath = context.Request.Path;
            string vPath = UrlHelper.GetRealVirtualPath(context, requestPath);

            var data = PageUrl2ControllerTypeCache.DefaultPageUrl2ControllerTypeCache
                .GetDescriptor(PageUrlAttribute.ConvertToUniqueId(vPath, context.Request.HttpMethod));

            // 如果没有找到合适的Action，并且请求的是一个ASPX页面，则按ASP.NET默认的方式来继续处理
            if (data == null && requestPath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                if (_msPageHandlerFactory == null)
                    _msPageHandlerFactory = new AspnetPageHandlerFactory();

                // 调用ASP.NET默认的Page处理器工厂来处理
                return _msPageHandlerFactory.GetHandler(context, requestType, requestPath, physicalPath);
            }

            RequestContext requestContext = new RequestContext(context, new Route(vPath)
            {
                UsePageUrlRoute = true,
                PageUrlData = data,
                Controller = data.Item1.Name,
                Action = data.Item2.Name
            });
            return new MvcHandlerBuilder().Create(requestContext);
        }

        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
