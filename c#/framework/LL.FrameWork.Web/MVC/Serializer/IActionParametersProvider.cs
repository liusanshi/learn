using System;
using System.Collections.Generic;
using System.Web;

namespace LL.FrameWork.Web.MVC.Serializer
{
	internal interface IActionParametersProvider
	{
		IDictionary<string, object> GetParameters(HttpRequest request, ActionDescriptor action);

	}
}
