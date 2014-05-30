using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：用户数据管理
 * 作者：Nick.Yan
 * 日期: 2008-11-13
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.Users
{
    public partial class ListUsers : CommonPage
    {
        RedGlovePermission.BLL.Users bll = new RedGlovePermission.BLL.Users();

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
                    BindGroup();
                    BindOrder();
                }
            }
        }

        /// <summary>
        /// 绑定用户组数据
        /// </summary>
        protected void BindGroup()
        {
            RedGlovePermission.BLL.RGP_Groups GroupBll = new RedGlovePermission.BLL.RGP_Groups();

            DataSet ds = GroupBll.GetGroupList("", "order by GroupOrder asc");          

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string s = ds.Tables[0].Rows[i]["GroupName"].ToString();
                if (ResourceManager.GetString(s) != "")
                {
                    GroupList.Items.Add(new ListItem(ResourceManager.GetString(s), ds.Tables[0].Rows[i]["GroupID"].ToString()));
                }
                else
                {
                    GroupList.Items.Add(new ListItem(s, ds.Tables[0].Rows[i]["GroupID"].ToString()));
                }
            }

            GroupList.Items.Insert(0, new ListItem(ResourceManager.GetString("Pub_Lab_showall"), "all"));
        }

        /// <summary>
        /// 绑定用户数据
        /// </summary>
        protected void BindOrder()
        {
            string strWhere = "(dbo.Users.RoleID <> 0)";
            if (GroupList.SelectedValue != "all")
            {
                strWhere = strWhere + " and dbo.Users.UserGroup=" + GroupList.SelectedValue;
            }
            DataSet ds = bll.GetUserList(strWhere, "ORDER BY dbo.Users.CreateTime DESC");

            if (ds.Tables[0].Rows.Count == 0)
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Msg_norecord");
            else
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Lab_gy") + ds.Tables[0].Rows.Count + ResourceManager.GetString("Pub_Lab_tjl");

            UserList.DataSource = ds;
            UserList.DataBind();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        protected void UserList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "Del":
                    if (bll.DeleteUser(int.Parse(e.CommandArgument.ToString())))
                    {
                        BindOrder();
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_delete_true") + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_delete_false") + "');", true);
                    }
                    break;
            }
        }

        /// <summary>
        /// 数据绑定到表格
        /// </summary>
        protected void UserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)//判定当前的行是否属于datarow类型的行
            {
                //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#ffffcd',this.style.fontWeight='';");
                //当鼠标离开的时候 将背景颜色还原的以前的颜色
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");

                LinkButton btnDel = ((LinkButton)e.Row.FindControl("btn_del"));

                btnDel.Attributes.Add("onclick", "return confirm('" + ResourceManager.GetString("Pub_Msg_confirm") + "')");
            }
        }

        /// <summary>
        /// 根据分类排序
        /// </summary>
        protected void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrder();
        }

        protected void UserList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;

                #region 语言
                tcHeader[0].Text = ResourceManager.GetString("ListUsers_Lab_0");
                tcHeader[1].Text = ResourceManager.GetString("ListUsers_Lab_1");
                tcHeader[2].Text = ResourceManager.GetString("ListUsers_Lab_2");
                tcHeader[3].Text = ResourceManager.GetString("ListUsers_Lab_3");
                tcHeader[4].Text = ResourceManager.GetString("ListUsers_Lab_4");
                tcHeader[5].Text = ResourceManager.GetString("ListUsers_Lab_5");
                tcHeader[6].Text = ResourceManager.GetString("ListUsers_Lab_6");
                tcHeader[7].Text = ResourceManager.GetString("Pub_Lbtn_update");
                tcHeader[8].Text = ResourceManager.GetString("Pub_Lbtn_delete");
                #endregion
            }
        }
    }
}
