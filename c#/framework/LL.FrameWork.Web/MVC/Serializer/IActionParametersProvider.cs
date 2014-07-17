using System;
using System.Collections.Generic;
using System.Web;

namespace LL.Framework.Web.MVC.Serializer
{
	internal interface IActionParametersProvider
	{
		IDictionary<string, object> GetParameters(HttpRequest request, ActionDescriptor action);

	}
}
