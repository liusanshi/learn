namespace LL.Framework.Web.MVC
{
    using System;

    /// <summary>
    /// 异常筛选器使用的上下文
    /// </summary>
    public class ExceptionContext : ControllerContext
    {
        private ActionResult _result;
        /// <summary>
        /// 筛选到的异常
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
        /// 创建ExceptionContext对象
        /// </summary>
        protected ExceptionContext()
        {
        }
        /// <summary>
        /// 创建ExceptionContext对象
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="exception"></param>
        public ExceptionContext(ControllerContext controllerContext, Exception exception)
            : base(controllerContext)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            this.Exception = exception;
        }
    }
}
