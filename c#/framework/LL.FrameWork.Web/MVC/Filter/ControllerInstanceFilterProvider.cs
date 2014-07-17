using System;
using System.Collections.Generic;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 控制器实例的筛选器的提供者
    /// 控制器也是筛选器的情况下使用
    /// </summary>
    public class ControllerInstanceFilterProvider : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (controllerContext.Controller != null)
            {
                yield return new Filter(controllerContext.Controller, FilterScope.First, new int?(Int32.MinValue));
            }
            yield break;
        }
    }
}
