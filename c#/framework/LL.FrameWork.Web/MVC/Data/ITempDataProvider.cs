using System;
using System.Collections.Generic;

namespace LL.FrameWork.Web.MVC
{
    public interface ITempDataProvider
    {
        IDictionary<string, object> LoadTempData(ControllerContext controllerContext);
        void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values);
    }
}
