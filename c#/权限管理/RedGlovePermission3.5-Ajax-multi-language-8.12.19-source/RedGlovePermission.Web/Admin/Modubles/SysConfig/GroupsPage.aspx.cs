using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：分组数据管理
 * 作者：Nick.Yan
 * 日期:2008-11-13
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.SysConfig
{
    public partial class GroupsPage : CommonPage
    {
        RedGlovePermission.BLL.RGP_Groups bll = new RedGlovePermission.BLL.RGP_Groups();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                //初始化模块权限
                UserHandle.InitModule("Mod_Groups");
                if (!IsPostBack)
                {
                    btn_add.Attributes.Add("onclick", "return CheckAdd()");//加入验证
                    BindOrder();

                    #region 语言加入
                    btn_add.Text = ResourceManager.GetString("Pub_Btn_add");
                    #endregion
                }
            }
        }

        /// <summary>
        /// 将数据绑定到DataSet
        /// </summary>
        public void BindOrder()
        {
            DataSet ds = bll.GetGroupList("", "order by GroupOrder asc");

            if (ds.Tables[0].Rows.Count == 0)
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Msg_norecord");
            else
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Lab_gy") + ds.Tables[0].Rows.Count + ResourceManager.GetString("Pub_Lab_tjl");

            GroupsLists.DataSource = ds;
            GroupsLists.DataBind();
        }

        /// <summary>
        /// 分页
        /// </summary>
        protected void GroupsLists_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GroupsLists.PageIndex = e.NewPageIndex;
            BindOrder(); //重新绑定GridView数据的函数 
        }

        /// <summary>
        /// 退出编辑状态
        /// </summary>
        protected void GroupsLists_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GroupsLists.EditIndex = -1;
            BindOrder();
        }

        /// <summary>
        /// 执行事件（删除操作）
        /// </summary>
        protected void GroupsLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Del")
            {
                int ret = bll.DeleteGroup(int.Parse(e.CommandArgument.ToString()));
                switch (ret)
                {
                    case 1:
                        BindOrder();
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_delete_true") + "');", true);
                        break;
                    case 2:
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_IsGroupUser") + "');", true);
                        break;
                    case 3:
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_IsGroupRole") + "');", true);
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
        protected void GroupsLists_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GroupsLists.EditIndex = e.NewEditIndex;
            BindOrder();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        protected void GroupsLists_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            RedGlovePermission.Model.RGP_Groups model = new RedGlovePermission.Model.RGP_Groups();
            model.GroupID = int.Parse(GroupsLists.DataKeys[e.RowIndex].Values[0].ToString());
            model.GroupName = ((TextBox)GroupsLists.Rows[e.RowIndex].FindControl("txt_name")).Text.Trim();
            model.GroupDescription = ((TextBox)GroupsLists.Rows[e.RowIndex].FindControl("txt_Description")).Text.Trim();
            model.GroupOrder = int.Parse(((TextBox)GroupsLists.Rows[e.RowIndex].FindControl("txt_order")).Text);

            if (!bll.UpdateGroup(model))
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "')", true);
            }
            //返回浏览狀態
            GroupsLists.EditIndex = -1;
            BindOrder();
        }

        /// <summary>
        /// 数据绑定到表格
        /// </summary>
        protected void GroupsLists_RowDataBound(object sender, GridViewRowEventArgs e)
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
        /// 添加数据
        /// </summary>
        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (txt_Name.Text.Trim() != "" || txt_order.Text.Trim() != "" || Lib.TypeParse.IsUnsign(txt_order.Text.Trim()))
            {
                RedGlovePermission.Model.RGP_Groups model = new RedGlovePermission.Model.RGP_Groups();

                model.GroupName = txt_Name.Text.Trim();
                model.GroupDescription = txt_Description.Text.Trim();
                model.GroupOrder = int.Parse(txt_order.Text.Trim());

                if (!bll.Exists(txt_Name.Text.Trim()))
                {
                    if (bll.CreateGroup(model))
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_true") + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_false") + "')", true);
                    }
                    txt_Name.Text = "";
                    txt_Description.Text = "";
                    txt_order.Text = "";
                    BindOrder();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_Isgroup") + "')", true);
                }
            }
        }

        protected void GroupsLists_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;

                #region 语言
                tcHeader[0].Text = ResourceManager.GetString("AuthorityPage_Lab_0");
                tcHeader[1].Text = ResourceManager.GetString("AuthorityPage_Lab_1");
                tcHeader[2].Text = ResourceManager.GetString("AuthorityPage_Lab_3");
                tcHeader[3].Text = ResourceManager.GetString("AuthorityPage_Lab_4");
                tcHeader[4].Text = ResourceManager.GetString("Pub_Lbtn_update");
                tcHeader[5].Text = ResourceManager.GetString("Pub_Lbtn_delete");
                #endregion
            }
        }
    }
}
