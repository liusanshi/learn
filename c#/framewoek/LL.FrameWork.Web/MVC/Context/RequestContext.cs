using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 请求的上下文
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// HttpRequest
        /// </summary>
        public HttpRequest HttpRequest { get; set; }

        /// <summary>
        /// 路由数据
        /// </summary>
        public Dictionary<string, object> RouteData { get; set; }

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
