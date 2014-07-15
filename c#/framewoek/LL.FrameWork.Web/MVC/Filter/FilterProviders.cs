using System;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 筛选器提供者
    /// 提供所有的筛选器
    /// 全局的筛选器、特性筛选器、控制器实例筛选器
    /// </summary>
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
