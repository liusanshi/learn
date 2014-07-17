using System;

namespace LL.Framework.Web.MVC
{
    public interface IActionInvoker
    {
        bool InvokeAction(ControllerContext controllerContext, string actionName);
    }
}
