using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public interface IController : IDisposable
    {
        void Execute(RequestContext requestContext);
    }
}
