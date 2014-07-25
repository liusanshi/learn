namespace LL.Framework.Web.MVC
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 动作筛选-执行之后的上下文
    /// </summary>
    public class ActionExecutedContext : ControllerContext
    {
        private ActionResult _result;
        /// <summary>
        /// 动作描述者
        /// </summary>
        public virtual ActionDescriptor ActionDescriptor
        {
            get;
            set;
        }
        /// <summary>
        /// 是否取消
        /// </summary>
        public virtual bool Canceled
        {
            get;
            set;
        }
        /// <summary>
        /// 动作执行完成发生的异常
        /// </summary>
        public virtual Exception Exception
        {
            get;
            set;
        }
        /// <summary>
        /// 是否接住异常
        /// </summary>
        public bool ExceptionHandled
        {
            get;
            set;
        }
        /// <summary>
        /// 动作执行的结果
        /// </summary>
        public ActionResult Result
        {
            get
            {
                return this._result ?? EmptyResult.Instance;
            }
            set
            {
                this._result = value;
            }
        }
        /// <summary>
        /// 创建ActionExecutedContext对象
        /// </summary>
        protected ActionExecutedContext()
        {
        }
        /// <summary>
        /// 创建ActionExecutedContext对象
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actiondescriptor"></param>
        /// <param name="canceled"></param>
        /// <param name="exception"></param>
        public ActionExecutedContext(ControllerContext controllerContext, ActionDescriptor actiondescriptor, bool canceled, Exception exception)
            : base(controllerContext)
        {
            if (actiondescriptor == null)
            {
                throw new ArgumentNullException("actiondescriptor");
            }
            this.ActionDescriptor = actiondescriptor;
            this.Canceled = canceled;
            this.Exception = exception;
        }
    }
}
