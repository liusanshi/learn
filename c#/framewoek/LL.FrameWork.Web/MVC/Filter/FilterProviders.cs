using System;

namespace LL.FrameWork.Web.MVC
{
    public static class FilterProviders
    {
        public static FilterProviderCollection Providers
        {
            get;
            private set;
        }
        static FilterProviders()
        {
            FilterProviders.Providers = new FilterProviderCollection();
            FilterProviders.Providers.Add(GlobalFilters.Filters);
            FilterProviders.Providers.Add(new FilterAttributeFilterProvider());
            FilterProviders.Providers.Add(new ControllerInstanceFilterProvider());
        }
    }
}
