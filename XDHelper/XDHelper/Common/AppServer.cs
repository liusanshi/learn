using System;
using System.Collections.Generic;
using System.Web;

namespace XDHelper.Common
{
    /// <summary>
    /// 应用所有的服务
    /// </summary>
    public static class AppServer
    {
        private static TokenManager _tokenManager = null;

        static AppServer()
        {
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// token 管理器
        /// </summary>
        public static TokenManager TokenManager
        {
            get { return _tokenManager; }
        }
    }
}