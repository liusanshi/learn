using System;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 全局筛选器
    /// </summary>
    public static class GlobalFilters
    {
        public static GlobalFilterCollection Filters
        {
            get;
            private set;
        }
        static GlobalFilters()
        {
            GlobalFilters.Filters = new GlobalFilterCollection();
        }
    }
}
