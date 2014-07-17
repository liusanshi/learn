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
        public object Model
        {
            get;
            set;
        }
        public DataRequestBehavior JsonRequestBehavior
        {
            get;
            set;
        }
        public JsonResult(object model)
            : this(model, DataRequestBehavior.DenyGet)
        { }
        public JsonResult(object model, DataRequestBehavior jsonRequestBehavior)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            this.JsonRequestBehavior = jsonRequestBehavior;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.JsonRequestBehavior == DataRequestBehavior.DenyGet 
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
            if (this.Model != null)
            {
                response.Write(this.Model.ToJson());
            }
        }
    }

    public enum DataRequestBehavior
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
