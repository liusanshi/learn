using System;

namespace LL.FrameWork.Web.MVC
{
    public interface IAuthorizationFilter
    {
        void OnAuthorization(AuthorizationContext filterContext);
    }
}
