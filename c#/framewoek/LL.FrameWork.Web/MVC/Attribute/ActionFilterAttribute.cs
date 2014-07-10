using System;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 动作筛选特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class ActionFilterAttribute : FilterAttribute, IActionFilter, IResultFilter
    {
        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
        public virtual void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
        public virtual void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
