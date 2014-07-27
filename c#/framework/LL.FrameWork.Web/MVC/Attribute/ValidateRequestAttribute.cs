using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 验证请求的输入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ValidateRequestAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 是否启用请求输入的验证
        /// </summary>
        public bool EnableValidation
		{
			get;
			private set;
		}
        /// <summary>
        /// 标识是否验证请求的输入
        /// </summary>
        /// <param name="enableValidation"></param>
        public ValidateRequestAttribute(bool enableValidation)
		{
			this.EnableValidation = enableValidation;
		}
		public virtual void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}
			filterContext.Controller.ValidateRequest = this.EnableValidation;
		}
    }
}
