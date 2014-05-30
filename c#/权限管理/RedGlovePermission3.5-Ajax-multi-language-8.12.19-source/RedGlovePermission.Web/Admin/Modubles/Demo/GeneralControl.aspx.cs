using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：权限控制演示
 * 作者：Nick.Yan
 * 日期: 2008-11-13
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.Demo
{
    public partial class GeneralControl : CommonPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                UserHandle.InitModule("Mod_Control");//初始化模块权限

                //浏览权限
                if (UserHandle.ValidationHandle(RGP_Tag.Browse))
                    Label1.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label1.Text = ResourceManager.GetString("Pub_State_close");

                //新增权限
                if (UserHandle.ValidationHandle(RGP_Tag.Add))
                    Label2.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label2.Text = ResourceManager.GetString("Pub_State_close");

                //编辑权限
                if (UserHandle.ValidationHandle(RGP_Tag.Edit))
                    Label3.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label3.Text = ResourceManager.GetString("Pub_State_close");

                //删除权限
                if (UserHandle.ValidationHandle(RGP_Tag.Delete))
                    Label4.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label4.Text = ResourceManager.GetString("Pub_State_close");

                //搜索权限
                if (UserHandle.ValidationHandle(RGP_Tag.Search))
                    Label5.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label5.Text = ResourceManager.GetString("Pub_State_close");

                //审核权限
                if (UserHandle.ValidationHandle(RGP_Tag.Verify))
                    Label6.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label6.Text = ResourceManager.GetString("Pub_State_close");

                //移动权限
                if (UserHandle.ValidationHandle(RGP_Tag.Move))
                    Label7.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label7.Text = ResourceManager.GetString("Pub_State_close");

                //打印权限
                if (UserHandle.ValidationHandle(RGP_Tag.Print))
                    Label8.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label8.Text = ResourceManager.GetString("Pub_State_close");

                //下载权限
                if (UserHandle.ValidationHandle(RGP_Tag.Download))
                    Label9.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label9.Text = ResourceManager.GetString("Pub_State_close");

                //备份权限
                if (UserHandle.ValidationHandle(RGP_Tag.back))
                    Label10.Text = ResourceManager.GetString("Pub_State_open");
                else
                    Label10.Text = ResourceManager.GetString("Pub_State_close");
            }
        }
    }
}
