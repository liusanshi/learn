using System;

namespace LL.FrameWork.Web.MVC
{
    public class HttpStatusCodeResult : ActionResult
    {
        public int StatusCode
        {
            get;
            private set;
        }
        public string StatusDescription
        {
            get;
            private set;
        }
        public HttpStatusCodeResult(int statusCode)
            : this(statusCode, null)
        {
        }
        public HttpStatusCodeResult(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.HttpContext.Response.StatusCode = this.StatusCode;
            if (this.StatusDescription != null)
            {
                context.HttpContext.Response.StatusDescription = this.StatusDescription;
            }
        }
    }
}
