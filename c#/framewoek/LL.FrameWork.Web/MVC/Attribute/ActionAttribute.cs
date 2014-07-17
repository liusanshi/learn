using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Web.MVC
{
	/// <summary>
	/// 将一个方法标记为一个Action
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ActionAttribute : ActionMethodSelectorAttribute
	{
		/// <summary>
		/// 允许哪些访问动词，与web.config中的httpHanlder的配置意义一致。
		/// </summary>
		public string Verb { get; set; }
        
		public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(Verb) || Verb == "*")
            {
                return true;
            }
            else
            {
                string[] verbArray = Verb.SplitTrim(StringExtensions.CommaSeparatorArray);

                return verbArray.Contains(controllerContext.HttpContext.Request.HttpMethod, StringComparer.OrdinalIgnoreCase);
            }
        }
    }


	
}
