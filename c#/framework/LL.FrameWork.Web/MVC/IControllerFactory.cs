using System;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// IController 创建工厂
    /// </summary>
    public interface IControllerFactory
    {
        /// <summary>
        /// 创建控制器的实例
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        IController CreateController(RequestContext requestContext, string controllerName);
        /// <summary>
        /// 获取控制的Session 模式
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        SessionMode GetControllerSessionBehavior(RequestContext requestContext, string controllerName);
        /// <summary>
        /// 重置控制器
        /// </summary>
        /// <param name="controller"></param>
        void ReleaseController(IController controller);
    }
}
