using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public class ControllerContext
    {
        private HttpContext _httpcontext;
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
        public ControllerContext() { }
        public ControllerContext(ControllerBase controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            this.Controller = controller;
        }
        public ControllerContext(HttpContext httpcontext, ControllerBase controller)
        {
            if (httpcontext == null)
            {
                throw new ArgumentNullException("httpcontext");
            }
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            this.HttpContext = httpcontext;
            this.Controller = controller;
        }
        protected ControllerContext(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            this.Controller = controllerContext.Controller;
            this.HttpContext = controllerContext.HttpContext;
        }
    }
}
