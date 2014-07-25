using System;
using System.Text.RegularExpressions;

namespace LL.Framework.Web.MVC
{
    public class ServiceHandlerFactory : BaseMvcHandlerFactory
    {
        private static readonly Regex s_urlRegex
            = new Regex(@"/(?<type>\w{3,})/((?<namespace>[\.\w]+)\.)?(?<name>\w+)/(?<method>\w+)\.(?<extname>[a-zA-Z]+)", RegexOptions.Compiled);

        /*
			可以解析以下格式的URL：（前一个表示包含命名空间的格式）

			/service/Fish.AA.Demo/GetMd5.aspx
		    /service/Demo/GetMd5.aspx
		*/

        public override RequestContext CreateRequestContext(System.Web.HttpContext context, string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            Match match = s_urlRegex.Match(path);
            if (match.Success == false)
            {
                return new RequestContext(context);
            }
            else
            {
                string serivceType = match.Groups["type"].Value;
                string nspace = match.Groups["namespace"].Value;
                string className = match.Groups["name"].Value;
                string extname = match.Groups["extname"].Value;

                return new RequestContext(context,
                    CreateRouteData(GetControllerName(serivceType, nspace, className, extname), match.Groups["method"].Value)
                    );
            }
        }
        /// <summary>
        /// 获取控制器的名称
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="nspace"></param>
        /// <param name="className"></param>
        /// <param name="extname"></param>
        /// <returns></returns>
        public virtual string GetControllerName(string serviceType, string nspace, string className, string extname)
        {
            return nspace
                + (nspace.Length > 0 ? "." : string.Empty)
                + className + "Service";
        }
    }
}
