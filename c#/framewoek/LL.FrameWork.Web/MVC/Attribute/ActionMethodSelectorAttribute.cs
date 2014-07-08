using System;
using System.Reflection;

namespace LL.FrameWork.Web.MVC
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ActionMethodSelectorAttribute : Attribute
    {
        public abstract bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo);
    }
}
