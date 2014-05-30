using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：用户详细信息显示
 * 作者：Nick.Yan
 * 日期: 2008-11-19
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.Users
{
    public partial class ViewUserPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Default.aspx");
            }
            else
            {
                if (Request.QueryString["uid"] != null)
                {
                    GetUser(int.Parse(Request.QueryString["uid"].ToString()));
                }
            }
        }

        protected void GetUser(int id)
        {
            RedGlovePermission.BLL.Users bll = new RedGlovePermission.BLL.Users();
            RedGlovePermission.Model.Users model= new RedGlovePermission.Model.Users();
            model = bll.GetUserModel(id);
            lab_name.Text = model.UserName;
            Lab_group.Text= model.UserGroup.ToString();
            Lab_role.Text=model.RoleID.ToString();
            Lab_state.Text=model.Status.ToString();
            Lab_time1.Text=model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            Lab_time2.Text = model.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
