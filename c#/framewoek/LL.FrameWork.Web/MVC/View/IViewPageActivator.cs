using System;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 视图创建者
    /// </summary>
    public interface IViewPageActivator
    {
        object Create(ControllerContext controllerContext, Type type);
    }
}
