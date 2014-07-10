using System;
using System.Reflection;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 动作的别名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ActionNameSelectorAttribute : Attribute
    {
        public abstract bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo);
    }
}
