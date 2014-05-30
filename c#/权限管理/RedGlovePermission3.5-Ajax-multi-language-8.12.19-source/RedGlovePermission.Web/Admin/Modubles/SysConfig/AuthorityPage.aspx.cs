using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：权限数据管理
 * 作者：Nick.Yan
 * 日期:2008-02-11
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.SysConfig
{
    public partial class AuthorityPage : CommonPage
    {
        RedGlovePermission.BLL.RGP_AuthorityDir bll = new RedGlovePermission.BLL.RGP_AuthorityDir();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                //初始化模块权限
                UserHandle.InitModule("Mod_Authorized");

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
            DataSet ds = bll.GetAuthorityList("", "order by AuthorityOrder asc");

            if (ds.Tables[0].Rows.Count == 0)
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Msg_norecord");
            else
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Lab_gy") + ds.Tables[0].Rows.Count + ResourceManager.GetString("Pub_Lab_tjl");

            AuthorityLists.DataSource = ds;
            AuthorityLists.DataBind();
        }

        /// <summary>
        /// 分页
        /// </summary>
        protected void AuthorityLists_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AuthorityLists.PageIndex = e.NewPageIndex;
            BindOrder(); //重新绑定GridView数据的函数 
        }

        /// <summary>
        /// 退出编辑状态
        /// </summary>
        protected void AuthorityLists_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            AuthorityLists.EditIndex = -1;
            BindOrder();
        }

        /// <summary>
        /// 执行事件（删除操作）
        /// </summary>
        protected void AuthorityLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "Del":
                    if (bll.DeleteAuthority(int.Parse(e.CommandArgument.ToString())))
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
        /// 变更到编辑状态
        /// </summary>
        protected void AuthorityLists_RowEditing(object sender, GridViewEditEventArgs e)
        {
            AuthorityLists.EditIndex = e.NewEditIndex;
            BindOrder();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        protected void AuthorityLists_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            RedGlovePermission.Model.RGP_AuthorityDir model = new RedGlovePermission.Model.RGP_AuthorityDir();
            model.AuthorityID = int.Parse(AuthorityLists.DataKeys[e.RowIndex].Values[0].ToString());
            model.AuthorityName = ((TextBox)AuthorityLists.Rows[e.RowIndex].FindControl("txt_name")).Text.Trim();
            model.AuthorityTag = ((TextBox)AuthorityLists.Rows[e.RowIndex].FindControl("txt_tag")).Text.Trim();
            model.AuthorityDescription = ((TextBox)AuthorityLists.Rows[e.RowIndex].FindControl("txt_Description")).Text.Trim();
            model.AuthorityOrder = int.Parse(((TextBox)AuthorityLists.Rows[e.RowIndex].FindControl("txt_order")).Text);

            if (!bll.UpdateAuthority(model))
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "')", true);
            }
            //返回浏览狀態
            AuthorityLists.EditIndex = -1;
            BindOrder();
        }

        /// <summary>
        /// 数据绑定到表格
        /// </summary>
        protected void AuthorityLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)//判定当前的行是否属于datarow类型的行
            {
                //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#ffffcd',this.style.fontWeight='';");
                //当鼠标离开的时候 将背景颜色还原的以前的颜色
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");

                LinkButton btnDel = (LinkButton)e.Row.FindControl("btn_del");

                btnDel.Attributes.Add("onclick", "return confirm('" + ResourceManager.GetString("Pub_Msg_confirm") + "')");
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (txt_Name.Text.Trim() != "" || txt_Tag.Text.Trim() != "" || txt_order.Text.Trim() != "" || Lib.TypeParse.IsUnsign(txt_order.Text.Trim()))
            {
                RedGlovePermission.Model.RGP_AuthorityDir model = new RedGlovePermission.Model.RGP_AuthorityDir();

                model.AuthorityName = txt_Name.Text.Trim();
                model.AuthorityTag = txt_Tag.Text.Trim();
                model.AuthorityDescription = txt_Description.Text.Trim();
                model.AuthorityOrder = int.Parse(txt_order.Text.Trim());

                if (!bll.Exists(txt_Tag.Text.Trim()))
                {
                    if (bll.CreateAuthority(model))
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_true") + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_false") + "')", true);
                    }
                    txt_Name.Text = "";
                    txt_Tag.Text = "";
                    txt_Description.Text = "";
                    txt_order.Text = "";
                    BindOrder();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_Iscaption") + "')", true);
                }
            }
        }

        protected void AuthorityLists_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;

                #region 语言
                tcHeader[0].Text = ResourceManager.GetString("AuthorityPage_Lab_0");
                tcHeader[1].Text = ResourceManager.GetString("AuthorityPage_Lab_1");
                tcHeader[2].Text = ResourceManager.GetString("AuthorityPage_Lab_2");
                tcHeader[3].Text = ResourceManager.GetString("AuthorityPage_Lab_3");
                tcHeader[4].Text = ResourceManager.GetString("AuthorityPage_Lab_4");
                tcHeader[5].Text = ResourceManager.GetString("Pub_Lbtn_update");
                tcHeader[6].Text = ResourceManager.GetString("Pub_Lbtn_delete");
                #endregion
            }
        }
    }
}
