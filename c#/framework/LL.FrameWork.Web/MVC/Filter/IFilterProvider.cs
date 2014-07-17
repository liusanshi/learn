using System.Collections.Generic;

namespace LL.Framework.Web.MVC
{
    public interface IFilterProvider
    {
        IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor);
    }
}