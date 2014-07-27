using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 用于验证用户身份的修饰属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 创建 AuthorizeAttribute 实例
        /// 用于验证用户身份的修饰属性
        /// </summary>
        public AuthorizeAttribute() { }

        private readonly object _typeId = new object();
        /// <summary>
        /// 未通过验证的请求的状态码
        /// </summary>
        private const int UnauthorizedRequestState = 401;
        private string _user;
        private string[] _users;
        private string _role;
        private string[] _roles;

        /// <summary>
        /// 获取该 AuthorizeAttribute 的唯一标识符。
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }

        /// <summary>
        /// 允许访问的用户列表，用逗号分隔。
        /// </summary>
        public string Users
        {
            get { return _user; }
            set
            {
                _user = value;
                _users = value.SplitTrim(StringExtensions.CommaSeparatorArray);
            }
        }

        /// <summary>
        /// 允许访问的角色列表，用逗号分隔。
        /// </summary>
        public string Roles
        {
            get { return _role; }
            set
            {
                _role = value;
                _roles = value.SplitTrim(StringExtensions.CommaSeparatorArray);
            }
        }
        /// <summary>
        /// 获取所有允许访问的用户列表
        /// </summary>
        protected string[] AllUsers
        {
            get
            {
                if (_users == null)
                {
                    _users = new string[0];
                }
                return _users;
            }
        }
        /// <summary>
        /// 获取所有允许访问的角色列表
        /// </summary>
        protected string[] AllRoles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new string[0];
                }
                return _roles;
            }
        }

        /// <summary>
        /// 验证核心
        /// 验证通过返回 true 否则返回 false
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="allowAnonymous">是否允许匿名访问</param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(HttpContext httpContext, bool allowAnonymous)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (allowAnonymous) return true;
            if (!httpContext.Request.IsAuthenticated)
                return false;

            IPrincipal user = httpContext.User;
            return (this.AllUsers.Length <= 0 || this.AllUsers.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
                && (this.AllRoles.Length <= 0 || this.AllRoles.Any(user.IsInRole));
        }
        /// <summary>
        /// 缓存验证的结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <param name="validationStatus"></param>
        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(context, data);
        }
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (this.AuthorizeCore(filterContext.HttpContext, filterContext.AllowAnonymous))
            {
                HttpCachePolicy cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), filterContext.AllowAnonymous);
                return;
            }
            this.HandleUnauthorizedRequest(filterContext);
        }
        /// <summary>
        /// 获取未通过验证的求情
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpStatusCodeResult(UnauthorizedRequestState);//返回401错误，未通过验证的请求
        }
        /// <summary>
        /// 缓存验证信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContext httpContext, object state)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!this.AuthorizeCore(httpContext, (bool)state))
            {
                return HttpValidationStatus.IgnoreThisRequest;
            }
            return HttpValidationStatus.Valid;
        }
    }
}
