using System;

namespace LL.FrameWork.Web.MVC
{
    public class AuthorizationContext : ControllerContext
    {
        /// <summary>
        /// 动作的表述
        /// </summary>
        public virtual ActionDescriptor ActionDescriptor
        {
            get;
            set;
        }
        /// <summary>
        /// 动作执行结果
        /// </summary>
        public ActionResult Result
        {
            get;
            set;
        }
        public AuthorizationContext()
        {
        }
        public AuthorizationContext(ControllerContext controllerContext, ActionDescriptor actiondescription)
            : base(controllerContext)
        {
            if (actiondescription == null)
            {
                throw new ArgumentNullException("actiondescription");
            }
            this.ActionDescriptor = actiondescription;
        }
    }
}
