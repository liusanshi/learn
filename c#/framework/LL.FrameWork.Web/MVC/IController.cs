using System;
using System.Web;

namespace LL.Framework.Web.MVC
{
    public interface IController : IDisposable
    {
        void Execute(RequestContext requestContext);
    }
}
