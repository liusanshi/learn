using System;
using System.Collections.Generic;

namespace LL.FrameWork.Web.MVC
{
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
