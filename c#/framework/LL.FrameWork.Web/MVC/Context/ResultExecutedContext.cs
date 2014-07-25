namespace LL.Framework.Web.MVC
{
    using System;

    /// <summary>
    /// 动作执行之后的上下文
    /// </summary>
    public class ResultExecutedContext : ControllerContext
    {
        /// <summary>
        /// 是否已经取消
        /// </summary>
        public virtual bool Canceled
        {
            get;
            set;
        }
        /// <summary>
        /// 执行发生的异常
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
        /// 动作执行结果
        /// </summary>
        public virtual ActionResult Result
        {
            get;
            set;
        }
        /// <summary>
        /// 创建ResultExecutedContext对象
        /// </summary>
        protected ResultExecutedContext()
        {
        }
        /// <summary>
        /// 创建ResultExecutedContext对象
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="result"></param>
        /// <param name="canceled"></param>
        /// <param name="exception"></param>
        public ResultExecutedContext(ControllerContext controllerContext, ActionResult result, bool canceled, Exception exception)
            : base(controllerContext)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            this.Result = result;
            this.Canceled = canceled;
            this.Exception = exception;
        }
    }
}
