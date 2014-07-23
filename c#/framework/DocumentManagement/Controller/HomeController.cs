using System;
using System.Web;

using LL.Framework.Web.MVC;

namespace DocumentManagement.Controller
{
    public class HomeController : ControllerBase
    {
        [PageUrl("/Default.aspx")]
        public TemplateViewResult Index()
        {
            return View("/Default.aspx");
        }
    }
}