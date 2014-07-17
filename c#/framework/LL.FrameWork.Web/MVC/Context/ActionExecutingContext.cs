using System;
using System.Collections.Generic;

namespace LL.Framework.Web.MVC
{
    public class ActionExecutingContext : ControllerContext
    {
        public virtual ActionDescriptor ActionDescriptor
        {
            get;
            set;
        }
        public virtual IDictionary<string, object> ActionParameters
        {
            get;
            set;
        }
        public ActionResult Result
        {
            get;
            set;
        }
        public ActionExecutingContext()
        {
        }
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
