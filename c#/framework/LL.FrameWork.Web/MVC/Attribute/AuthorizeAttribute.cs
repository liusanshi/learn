using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LL.Framework.Web.MVC
{
	/// <summary>
	/// 用于验证用户身份的修饰属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeAttribute : ActionFilterAttribute
	{
		private string _user;
		private string[] _users;
		private string _role;
		private string[] _roles;

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

		public virtual bool AuthenticateRequest(HttpContext context)
		{
			if( context.Request.IsAuthenticated == false )
				return false;

			if( _users != null && 
				_users.Contains(context.User.Identity.Name, StringComparer.OrdinalIgnoreCase) == false )
				return false;

			if( _roles != null && _roles.Any(context.User.IsInRole) == false )
				return false;

			return true;
		}

        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            IEnumerable<string> source =
                from piece in original.Split(new char[]
				{
					','
				})
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed;
            return source.ToArray<string>();
        }
	}
}
