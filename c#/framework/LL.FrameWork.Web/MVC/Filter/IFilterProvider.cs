using System.Collections.Generic;

namespace LL.FrameWork.Web.MVC
{
    public interface IFilterProvider
    {
        IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor);
    }
}