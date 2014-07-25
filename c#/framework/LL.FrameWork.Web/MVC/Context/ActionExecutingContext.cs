namespace LL.Framework.Web.MVC
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 动作筛选-执行之前的上下文
    /// </summary>
    public class ActionExecutingContext : ControllerContext
    {
        /// <summary>
        /// 动作描述者
        /// </summary>
        public virtual ActionDescriptor ActionDescriptor
        {
            get;
            set;
        }
        /// <summary>
        /// 动作描述的参数
        /// </summary>
        public virtual IDictionary<string, object> ActionParameters
        {
            get;
            set;
        }
        /// <summary>
        /// 动作执行的结果
        /// </summary>
        public ActionResult Result
        {
            get;
            set;
        }
        /// <summary>
        /// 创建ActionExecutingContext对象
        /// </summary>
        protected ActionExecutingContext()
        {
        }
        /// <summary>
        /// 创建ActionExecutingContext对象
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actiondescriptor"></param>
        /// <param name="actionParameters"></param>
        public ActionExecutingContext(ControllerContext controllerContext, ActionDescriptor actiondescriptor, IDictionary<string, object> actionParameters)
            : base(controllerContext)
        {
            if (actiondescriptor == null)
            {
                throw new ArgumentNullException("actiondescriptor");
            }
            if (actionParameters == null)
            {
                throw new ArgumentNullException("actionParameters");
            }
            this.ActionDescriptor = actiondescriptor;
            this.ActionParameters = actionParameters;
        }
    }
}
