using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LL.Framework.Web.MVC
{
    public class RedirectResult : ActionResult
    {
        public bool Permanent
        {
            get;
            private set;
        }
        public string Url
        {
            get;
            private set;
        }
        public RedirectResult(string url)
            : this(url, false)
        {
        }
        public RedirectResult(string url, bool permanent)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("参数：url为null或者空", "url");
            }
            this.Permanent = permanent;
            this.Url = url;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            //if (context.IsChildAction)
            //{
            //    throw new InvalidOperationException(MvcResources.RedirectAction_CannotRedirectInChildAction);
            //}
            if (this.Permanent)
            {
                context.HttpContext.Response.RedirectPermanent(Url, true);
                return;
            }
            context.HttpContext.Response.Redirect(Url, true);
        }
    }
}
