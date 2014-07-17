using System;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 创建IController对象的接口
    /// </summary>
    public interface IControllerActivator
    {
        IController Create(RequestContext requestContext, Type controllerType);
    }
}
