using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LL.Framework.Web.MVC;

namespace DocumentManagement.Config
{
    public static class FilterConfig
    {
        /// <summary>
        /// 注册全局筛选器
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
        }
    }
}