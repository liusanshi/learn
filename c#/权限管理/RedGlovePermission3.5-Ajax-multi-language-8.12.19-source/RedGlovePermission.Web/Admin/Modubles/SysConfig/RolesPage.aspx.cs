using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：角色数据管理
 * 作者：Nick.Yan
 * 日期:2008-11-13
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.SysConfig
{
    public partial class RolesPage : CommonPage
    {
        RedGlovePermission.BLL.RGP_Roles bll = new RedGlovePermission.BLL.RGP_Roles();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                //初始化模块权限
                UserHandle.InitModule("Mod_Roles");

                if (!IsPostBack)
                {
                    btn_add.Attributes.Add("onclick", "return CheckAdd()");//加入验证

                    #region 语言加入
                    btn_add.Text = ResourceManager.GetString("Pub_Btn_add");
                    //绑定组信息
                    RedGlovePermission.BLL.RGP_Groups Groupbll = new RedGlovePermission.BLL.RGP_Groups();
                    DataSet groupds = Groupbll.GetGroupList("", "order by GroupOrder asc");

                    for (int i = 0; i < groupds.Tables[0].Rows.Count; i++)
                    {
                        string s = groupds.Tables[0].Rows[i]["GroupName"].ToString();
                        if (ResourceManager.GetString(s) != "")
                        {
                            GroupList.Items.Add(new ListItem(ResourceManager.GetString(s),groupds.Tables[0].Rows[i]["GroupID"].ToString()));
                        }
                        else
                        {
                            GroupList.Items.Add(new ListItem(s, groupds.Tables[0].Rows[i]["GroupID"].ToString()));
                        }
                    }

                    #endregion

                    BindOrder();
                }
            }
        }

        /// <summary>
        /// 将数据绑定到DataSet
        /// </summary>
        public void BindOrder()
        {
            DataSet ds = bll.GetRoleList("RoleGroupID=" + GroupList.SelectedValue, "");

            if (ds.Tables[0].Rows.Count == 0)
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Msg_norecord");
            else
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Lab_gy") + ds.Tables[0].Rows.Count + ResourceManager.GetString("Pub_Lab_tjl");

            RolesLists.DataSource = ds;
            RolesLists.DataBind();
        }

        /// <summary>
        /// 分页
        /// </summary> 
        protected void RolesLists_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            RolesLists.PageIndex = e.NewPageIndex;
            BindOrder(); //重新绑定GridView数据的函数 
        }

        /// <summary>
        /// 退出编辑状态
        /// </summary>
        protected void RolesLists_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RolesLists.EditIndex = -1;
            BindOrder();
        }

        /// <summary>
        /// 执行事件（删除操作）
        /// </summary>
        protected void RolesLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Del")
            {
                int ret = bll.DeleteRole(int.Parse(e.CommandArgument.ToString()));
                switch (ret)
                {
                    case 1:
                        BindOrder();
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_delete_true") + "');", true);
                        break;
                    case 2:
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_IsRoseUser") + "');", true);
                        break;
                    default:
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_delete_false") + "');", true);
                        break;
                }

            }
        }

        /// <summary>
        /// 变更到编辑状态
        /// </summary>
        protected void RolesLists_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RolesLists.EditIndex = e.NewEditIndex;
            BindOrder();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        protected void RolesLists_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            RedGlovePermission.Model.RGP_Roles model = new RedGlovePermission.Model.RGP_Roles();
            model.RoleID = int.Parse(RolesLists.DataKeys[e.RowIndex].Values[0].ToString());
            model.RoleName = ((TextBox)RolesLists.Rows[e.RowIndex].FindControl("txt_name")).Text.Trim();
            model.RoleDescription = ((TextBox)RolesLists.Rows[e.RowIndex].FindControl("txt_Description")).Text.Trim();
            model.RoleGroupID = int.Parse(((DropDownList)RolesLists.Rows[e.RowIndex].FindControl("GroupID")).SelectedValue);
            if (!bll.UpdateRole(model))
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "')", true);
            }
            //返回浏览狀態
            RolesLists.EditIndex = -1;
            BindOrder();
        }

        /// <summary>
        /// 数据绑定到表格
        /// </summary>
        protected void RolesLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)//判定当前的行是否属于datarow类型的行
            {
                RedGlovePermission.BLL.RGP_Groups Groupbll = new RedGlovePermission.BLL.RGP_Groups();
                DataSet Groups1 = Groupbll.GetGroupList("", "order by GroupOrder asc");
                //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#ffffcd',this.style.fontWeight='';");
                //当鼠标离开的时候 将背景颜色还原的以前的颜色
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");

                LinkButton btnDel = ((LinkButton)e.Row.FindControl("btn_del"));
                Label Lab_GroupID = ((Label)e.Row.FindControl("Lab_GroupID"));
                Label hid_GroupID = ((Label)e.Row.FindControl("hid_GroupID"));
                for (int i = 0; i < Groups1.Tables[0].Rows.Count; i++)
                {
                    if (Groups1.Tables[0].Rows[i]["GroupID"].ToString() == hid_GroupID.Text)
                    {
                        Lab_GroupID.Text = ResourceManager.GetString(Groups1.Tables[0].Rows[i]["GroupName"].ToString());
                    }
                }

                btnDel.Attributes.Add("onclick", "return confirm('" + ResourceManager.GetString("Pub_Msg_confirm") + "')");
            }

            if ((e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                RedGlovePermission.BLL.RGP_Groups Groupbll = new RedGlovePermission.BLL.RGP_Groups();
                DataSet Groups2 = Groupbll.GetGroupList("", "order by GroupOrder asc");

                Label hid_GroupID = ((Label)e.Row.FindControl("hid_GroupID"));
                DropDownList GroupID = ((DropDownList)e.Row.FindControl("GroupID"));

                for (int i = 0; i < Groups2.Tables[0].Rows.Count; i++)
                {
                    string s = Groups2.Tables[0].Rows[i]["GroupName"].ToString();
                    if (ResourceManager.GetString(s) != "")
                    {
                        GroupID.Items.Add(new ListItem(ResourceManager.GetString(s), Groups2.Tables[0].Rows[i]["GroupID"].ToString()));
                    }
                    else
                    {
                        GroupID.Items.Add(new ListItem(s, Groups2.Tables[0].Rows[i]["GroupID"].ToString()));
                    }
                }

                GroupID.SelectedValue = hid_GroupID.Text;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (txt_Name.Text.Trim() != "")
            {
                RedGlovePermission.Model.RGP_Roles model = new RedGlovePermission.Model.RGP_Roles();

                model.RoleName = txt_Name.Text.Trim();
                model.RoleDescription = txt_Description.Text.Trim();
                model.RoleGroupID = int.Parse(GroupList.SelectedValue);

                switch (bll.CreateRole(model))
                {
                    case 1:
                        txt_Name.Text = "";
                        txt_Description.Text = "";
                        BindOrder();
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_true") + "')", true);
                        break;
                    case 2:
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_Isgroup") + "')", true);
                        break;
                    default:
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_false") + "')", true);
                        break;
                }                
            }
        }

        /// <summary>
        /// 选择分类
        /// </summary>
        protected void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrder();
        }

        protected void RolesLists_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;

                #region 语言
                tcHeader[0].Text = ResourceManager.GetString("RolesPage_Lab_0");
                tcHeader[1].Text = ResourceManager.GetString("RolesPage_Lab_1");
                tcHeader[2].Text = ResourceManager.GetString("RolesPage_Lab_3");
                tcHeader[3].Text = ResourceManager.GetString("RolesPage_Lab_5");
                tcHeader[4].Text = ResourceManager.GetString("Pub_Lbtn_update");
                tcHeader[5].Text = ResourceManager.GetString("Pub_Lbtn_delete");
                #endregion
            }
        }
    }
}
