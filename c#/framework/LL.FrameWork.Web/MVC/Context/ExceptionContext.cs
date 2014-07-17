namespace LL.Framework.Web.MVC
{
    using System;

    public class ExceptionContext : ControllerContext
    {
        private ActionResult _result;
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
        public ExceptionContext()
        {
        }
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
