using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace RedGlovePermission.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Hashtable hOnline = (Hashtable)Application["Online"];
            if (hOnline[Session.SessionID] != null)
            {
                hOnline.Remove(Session.SessionID);
                Application.Lock();
                Application["Online"] = hOnline;
                Application.UnLock();
            } 
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}