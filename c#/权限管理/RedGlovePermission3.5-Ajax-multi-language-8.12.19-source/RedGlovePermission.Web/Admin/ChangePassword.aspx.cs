using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：用戶密碼更改
 * 作者：Nick.Yan
 * 日期:2008-02-11
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin
{
    public partial class ChangePassword : CommonPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    btn_save.Attributes.Add("onclick", "return checksubmit()");

                    #region 语言加入
                    btn_save.Text = ResourceManager.GetString("ChangePassword_Btn_1");
                    #endregion
                }
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (OldPassword.Text != "" && NewPassword.Text != "" && RePassword.Text != "" && (NewPassword.Text == RePassword.Text))
            {
                RedGlovePermission.BLL.Users bll = new RedGlovePermission.BLL.Users();
                int id = SessionBox.GetUserSession().LoginId;
                if (bll.VerifyPassword(id,RedGlovePermission.Lib.SecurityEncryption.MD5(OldPassword.Text.Trim(), 32)))
                {
                    if (bll.ChangePassword(id, RedGlovePermission.Lib.SecurityEncryption.MD5(RePassword.Text.Trim(), 32)))
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "", "alert('" + ResourceManager.GetString("Pub_Msg_update_true") + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "", "alert('" + ResourceManager.GetString("ChangePassword_Msg_1") + "');", true);
                }
            }
        }
    }
}
