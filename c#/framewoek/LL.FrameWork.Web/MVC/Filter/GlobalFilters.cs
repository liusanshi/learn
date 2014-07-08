using System;

namespace LL.FrameWork.Web.MVC
{
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
