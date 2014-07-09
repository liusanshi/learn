using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Globalization;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 请求的上下文
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// 创建RequestContext对象
        /// </summary>
        public RequestContext() { }
        /// <summary>
        /// 创建RequestContext对象
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="route"></param>
        public RequestContext(HttpContext httpContext, Route route)
        {
            HttpContext = httpContext;
            RouteData = route;
        }

        public HttpContext HttpContext { get; set; }

        /// <summary>
        /// HttpRequest
        /// </summary>
        public HttpRequest HttpRequest
        {
            get
            {
                if (HttpContext != null)
                {
                    return HttpContext.Request;
                }
                return null;
            }
        }

        /// <summary>
        /// 路由数据
        /// </summary>
        public Route RouteData { get; set; }
    }

    /// <summary>
    /// 路由元数据
    /// </summary>
    public class Route
    {
        private readonly static Route _Instance = new Route();

        /// <summary>
        /// 路由元数据实例
        /// </summary>
        public static Route Instance { get { return _Instance; } }

        /// <summary>
        /// 创建RouteData
        /// </summary>
        public Route() : this(Instance) { }
        /// <summary>
        /// 创建RouteData
        /// </summary>
        /// <param name="url"></param>
        public Route(string url) : this(url, null) { }
        /// <summary>
        /// 创建RouteData
        /// </summary>
        /// <param name="url"></param>
        /// <param name="routeData"></param>
        public Route(string url, Dictionary<string, object> routeData)
        {
            this.Url = url;
            _routeData = routeData;
        }
        /// <summary>
        /// 创建RouteData
        /// </summary>
        /// <param name="routeData"></param>
        public Route(Route routeData)
        {
            this.Url = routeData.Url;
            _routeData = routeData._routeData;
        }

        private Dictionary<string, object> _routeData = null;
        /// <summary>
        /// 路由数据
        /// </summary>
        public Dictionary<string, object> RouteData
        {
            get
            {
                if (_routeData == null)
                {
                    _routeData = new Dictionary<string, object>();
                }
                return _routeData;
            }
        }
        /// <summary>
        /// 路由时的url
        /// </summary>
        public string Url { get; internal set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public IEnumerable<string> Namespaces
        {
            get
            {
                return (IEnumerable<string>)GetValue(RouteData, "Namespaces", Enumerable.Empty<string>());
            }
            set
            {
                RouteData["Namespaces"] = value;
            }
        }
        /// <summary>
        /// 使用默认的命名空间(或没有命名空间)限定返回区域
        /// </summary>
        public bool UseNamespaceFallback
        {
            get
            {
                return (bool)GetValue(RouteData, "UseNamespaceFallback", false);
            }
            set
            {
                RouteData["UseNamespaceFallback"] = value;
            }
        }

        /// <summary>
        /// 获取控制器
        /// </summary>
        public string Controller
        {
            get
            {
                return Convert.ToString(GetValue(RouteData, "Controller", ""));
            }
            set
            {
                RouteData["Controller"] = value;
            }
        }
        /// <summary>
        /// 获取动作
        /// </summary>
        public string Action
        {
            get
            {
                return Convert.ToString(GetValue(RouteData, "Action", ""));
            }
            set
            {
                RouteData["Action"] = value;
            }
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public string GetRequiredString(string valueName)
        {
            var value = Convert.ToString(GetValue(RouteData, valueName, ""));
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "请求的值:{0}不存在或者为空", new object[]
			{
				valueName
			}));
        }

        /// <summary>
        /// 添加命名空间
        /// </summary>
        /// <param name="nameSpace"></param>
        public void AddNS(string nameSpace)
        {
            Namespaces = Namespaces.Concat(new string[] { nameSpace });
        }
        /// <summary>
        /// 添加命名空间
        /// </summary>
        /// <param name="nameSpaces"></param>
        public void AddNS(IEnumerable<string> nameSpaces)
        {
            Namespaces = Namespaces.Concat(nameSpaces);
        }

        /// <summary>
        /// 在字典中查找指定的值查找指定的值，如果没有返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            if (dict == null) return defaultValue;
            TValue val = default(TValue);
            if (dict.TryGetValue(key, out val))
            {
                return val;
            }
            return defaultValue;
        }
    }
}
