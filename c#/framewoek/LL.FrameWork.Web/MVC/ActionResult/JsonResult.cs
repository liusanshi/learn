using System;
using System.Text;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public class JsonResult : ActionResult
    {
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
        public object Data
        {
            get;
            set;
        }
        public JsonRequestBehavior JsonRequestBehavior
        {
            get;
            set;
        }
        public JsonResult()
        {
            this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet 
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("访问被拒绝!");
            }
            HttpResponse response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                response.Write(this.Data.ToJson());
            }
        }
    }

    public enum JsonRequestBehavior
    {
        /// <summary>
        /// 允许获取
        /// </summary>
        AllowGet,
        /// <summary>
        /// 拒绝获取
        /// </summary>
        DenyGet
    }
}
