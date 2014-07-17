using System;

namespace LL.Framework.Web.MVC
{
    public class ResultExecutedContext : ControllerContext
    {
        public virtual bool Canceled
        {
            get;
            set;
        }
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
        public virtual ActionResult Result
        {
            get;
            set;
        }
        public ResultExecutedContext()
        {
        }
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
