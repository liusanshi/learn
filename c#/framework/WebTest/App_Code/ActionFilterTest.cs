using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LL.FrameWork.Web.MVC;

namespace WebTest
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ActionFilterTestAttribute : ActionFilterAttribute
    {

    }
}