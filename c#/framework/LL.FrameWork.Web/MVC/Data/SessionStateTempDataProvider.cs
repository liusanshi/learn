using System;
using System.Collections.Generic;
using System.Web.SessionState;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// Session保存临时数据 提供者
    /// </summary>
    public class SessionStateTempDataProvider : ITempDataProvider
    {
        internal const string TempDataSessionStateKey = "__ControllerTempData";
        /// <summary>
        /// 加载临时数据
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        public virtual IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            HttpSessionState session = controllerContext.HttpContext.Session;
            if (session != null)
            {
                Dictionary<string, object> dictionary = session[TempDataSessionStateKey] as Dictionary<string, object>;
                if (dictionary != null)
                {
                    session.Remove(TempDataSessionStateKey);
                    return dictionary;
                }
            }
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 保存临时数据
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="values"></param>
        public virtual void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            HttpSessionState session = controllerContext.HttpContext.Session;
            bool hasData = values != null && values.Count > 0;
            if (session == null)
            {
                if (hasData)
                {
                    throw new InvalidOperationException("Session为null");
                }
            }
            else
            {
                if (hasData)
                {
                    session[TempDataSessionStateKey] = values;
                }
                else if (session[TempDataSessionStateKey] != null)
                {
                    session.Remove(TempDataSessionStateKey);
                }
            }
        }
    }
}
