using System;
using System.Net;

namespace LL.Framework.Web.MVC
{
    public class HttpNotFoundResult : HttpStatusCodeResult
    {
        public HttpNotFoundResult()
            : this(null)
        {
        }
        public HttpNotFoundResult(string statusDescription)
            : base((int)HttpStatusCode.NotFound, statusDescription)
        {
        }
    }
}
