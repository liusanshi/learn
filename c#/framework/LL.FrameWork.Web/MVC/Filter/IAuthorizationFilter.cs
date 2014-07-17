using System;

namespace LL.Framework.Web.MVC
{
    public interface IAuthorizationFilter
    {
        void OnAuthorization(AuthorizationContext filterContext);
    }
}
