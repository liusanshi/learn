using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Web.MVC
{
    public class ActionExecutedContext : ControllerContext
    {
        private ActionResult _result;
        public virtual ActionDescriptor ActionDescriptor
        {
            get;
            set;
        }
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
        public ActionExecutedContext()
        {
        }
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
