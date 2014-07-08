using System;
using System.Text;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public class ContentResult : ActionResult
    {
        public string Content
        {
            get;
            set;
        }
        public Encoding ContentEncoding
        {
            get;
            set;
        }
        public string ContentType
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
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            else
            {
                response.ContentEncoding = Encoding.UTF8; //默认是UTF8
            }
            if (this.Content != null)
            {
                response.Write(this.Content);
            }
        }
    }
}
