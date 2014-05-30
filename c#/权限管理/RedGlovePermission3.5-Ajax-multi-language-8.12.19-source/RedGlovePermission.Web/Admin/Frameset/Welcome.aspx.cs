using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedGlovePermission.Web.Admin.Frameset
{
    public partial class Welcome : CommonPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Default.aspx");
            }
            else
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                if (Request.QueryString["icke"] != null)
                {
                    HttpCookie style = new HttpCookie("UIStyle", Request.QueryString["icke"].ToString());
                    style.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(style);
                }
            }
        }
    }
}
