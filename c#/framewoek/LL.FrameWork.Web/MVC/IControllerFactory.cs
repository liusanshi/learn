using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// IController 创建工厂
    /// </summary>
    public interface IControllerFactory
    {
        IController CreateController(RequestContext requestContext, string controllerName);
        SessionMode GetControllerSessionBehavior(RequestContext requestContext, string controllerName);
        void ReleaseController(IController controller);
    }
}
