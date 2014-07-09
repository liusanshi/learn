using System;
using System.Reflection;

namespace LL.FrameWork.Web.MVC
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ActionNameSelectorAttribute : Attribute
    {
        public abstract bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo);
    }
}
