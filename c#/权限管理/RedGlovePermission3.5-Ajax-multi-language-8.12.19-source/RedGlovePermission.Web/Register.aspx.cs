using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedGlovePermission.Web
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_reg_Click(object sender, EventArgs e)
        {
            RedGlovePermission.BLL.Users bll = new RedGlovePermission.BLL.Users();
            RedGlovePermission.Model.Users model = new RedGlovePermission.Model.Users();
            model.UserName = txt_username.Text.Trim();
            model.Password = RedGlovePermission.Lib.SecurityEncryption.MD5(txt_password2.Text.Trim(), 32);
            model.Question = txt_question.Text.Trim();
            model.Answer = txt_answer.Text.Trim();
            if (RGP_Value.IsVerifyUser)
            {
                model.UserGroup = RGP_Value.initGroupID;
                model.RoleID = RGP_Value.InitRoleID;
            }
            model.IsLimit = false;

            switch(bll.CreateUser(model))
            {
                case 1:
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('注册成功！')", true);
                    break;
                case 2:
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('用户已经存在，请更换后重试!')", true);
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('注册失败！')", true);
                    break;
            }
        }
    }
}
