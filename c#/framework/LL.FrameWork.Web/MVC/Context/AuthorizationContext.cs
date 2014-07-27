namespace LL.Framework.Web.MVC
{
    using System;

    /// <summary>
    /// 授权验证的上下文
    /// </summary>
    public class AuthorizationContext : ControllerContext
    {
        private bool? _allowAnonymous;

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

        /// <summary>
        /// 是否允许匿名用户访问
        /// </summary>
        public bool AllowAnonymous
        {
            get
            {
                if (_allowAnonymous == null)
                {
                    var AllowAnonymousType = typeof(AllowAnonymousAttribute);
                    _allowAnonymous = ActionDescriptor.IsDefined(AllowAnonymousType, true)
                        || base.Controller.GetType().IsDefined(AllowAnonymousType, true);
                }
                return _allowAnonymous.Value;
            }
        }

        /// <summary>
        /// 创建 授权验证的上下文 对象
        /// </summary>
        protected AuthorizationContext()
        {
        }
        /// <summary>
        /// 创建 授权验证的上下文 对象
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actiondescription"></param>
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
