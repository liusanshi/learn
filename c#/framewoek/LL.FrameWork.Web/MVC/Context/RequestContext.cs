using System;
using System.Collections.Generic;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 请求的上下文
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// HttpRequest
        /// </summary>
        public HttpRequest HttpRequest { get; set; }
    }
}
