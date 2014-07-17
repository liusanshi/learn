using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public class JavaScriptResult : ActionResult
    {
        public string Script
        {
            get;
            set;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponse response = context.HttpContext.Response;
            response.ContentType = "application/x-javascript";
            if (this.Script != null)
            {
                response.Write(this.Script);
            }
        }
    }
}
