using System;
using System.Text;
using System.Web;

namespace LL.Framework.Web.MVC
{
    public class XmlResult : ActionResult
    {
        public object Model { get; private set; }
        /// <summary>
        /// xml数据请求
        /// </summary>
        public DataRequestBehavior XmlRequestBehavior
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
        /// <summary>
        /// 创建 XmlResult 对象
        /// </summary>
        /// <param name="model"></param>
        public XmlResult(object model)
            : this(model, DataRequestBehavior.DenyGet)
        { }
        /// <summary>
        /// 创建 XmlResult 对象
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xmlRequestBehavior"></param>
        public XmlResult(object model, DataRequestBehavior xmlRequestBehavior)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            Model = model;
            this.XmlRequestBehavior = xmlRequestBehavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.XmlRequestBehavior == DataRequestBehavior.DenyGet 
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
                response.ContentType = "application/xml";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Model != null)
            {
                response.Write(this.Model.ToXml());
            }
        }
    }
}
