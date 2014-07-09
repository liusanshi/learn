using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Web.MVC
{
	/// <summary>
	/// 用于描述一个Action可以处理哪些请求路径。
	/// 注意：这个Attribute可以多次使用，表示可以处理多个请求路径。
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PageUrlAttribute : ActionNameSelectorAttribute
	{
        public PageUrlAttribute() { }

        public PageUrlAttribute(string url) { Url = url; }

		/// <summary>
		/// 指示可以处理的请求路径。比如："/abc.aspx" 
		/// </summary>
		public string Url { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, System.Reflection.MethodInfo methodInfo)
        {
            throw new NotImplementedException();
        }
    }
}
