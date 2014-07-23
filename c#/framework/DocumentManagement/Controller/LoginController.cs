using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LL.Framework.Web.MVC;

namespace DocumentManagement.Controller
{
    public class LoginController : ControllerBase
    {
        [PageUrl("/Login.aspx")]
        [Action(Verb="get")]
        public TemplateViewResult LoginPage()
        {
            return View("/Login.aspx");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="psd"></param>
        public void Login(string usercode, string psd)
        {
 
        }
    }
}