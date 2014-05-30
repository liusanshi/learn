using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedGlovePermission.Web.Admin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SessionBox.RemoveUserSession();
            //SessionBox.RemoveModuleList();
            //SessionBox.RemoveAuthority();
            Session.Abandon();
            Response.Write("<script language=javascript>window.parent.location.href='Login.aspx';</script>");
        }
    }
}
