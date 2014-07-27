namespace LL.Framework.Web.MVC
{
    using System;
    using System.Web;
    
    /// <summary>
    /// 验证防伪标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 创建 ValidateAntiForgeryTokenAttribute 对象
        /// </summary>
        public ValidateAntiForgeryTokenAttribute()
            : this(new Action<HttpContext, string>(AntiForgery.Validate))
        {
        }
        /// <summary>
        /// 创建 ValidateAntiForgeryTokenAttribute 对象
        /// </summary>
        /// <param name="validateAction"></param>
        internal ValidateAntiForgeryTokenAttribute(Action<HttpContext, string> validateAction)
        {
            this.ValidateAction = validateAction;
        }

        private string _salt;
        /// <summary>
        /// 添加的用于混淆的字符串(俗称加盐)
        /// </summary>
        public string Salt
        {
            get
            {
                return this._salt ?? string.Empty;
            }
            set
            {
                this._salt = value;
            }
        }
        /// <summary>
        /// 验证动作
        /// </summary>
        internal Action<HttpContext, string> ValidateAction
        {
            get;
            private set;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            this.ValidateAction(filterContext.HttpContext, this.Salt);
        }
    }
}
