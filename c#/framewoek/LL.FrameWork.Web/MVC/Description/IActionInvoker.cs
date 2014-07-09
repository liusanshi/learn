using System;

namespace LL.FrameWork.Web.MVC
{
    public interface IActionInvoker
    {
        bool InvokeAction(ControllerContext controllerContext, string actionName);
    }
}
