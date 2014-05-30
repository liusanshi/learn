using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RedGlove.Core.Languages;

/**************************************
 * 模块：模块数据管理
 * 作者：Nick.Yan
 * 日期: 2008-11-13
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.SysConfig
{
    public partial class ModulesPage : CommonPage
    {
        RedGlovePermission.BLL.RGP_Modules bll = new RedGlovePermission.BLL.RGP_Modules();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                //初始化模块权限
                UserHandle.InitModule("Mod_Modules");
                if (!IsPostBack)
                {
                    btn_save.Attributes.Add("onclick", "return CheckAdd()");//加入验证
                    btn_update.Attributes.Add("onclick", "return CheckAdd()");//加入验证
                    BindTypeOrder();
                    BindOrder();

                    #region 语言加入
                    btn_add.Text = ResourceManager.GetString("Pub_Btn_add");
                    btn_update.Text = ResourceManager.GetString("Pub_Btn_update");
                    btn_save.Text = ResourceManager.GetString("Pub_Btn_save");
                    btn_cancel.Text = ResourceManager.GetString("Pub_Btn_cancel");

                    txt_state.Items[0].Text = ResourceManager.GetString("Pub_State_open");
                    txt_state.Items[1].Text = ResourceManager.GetString("Pub_State_close");

                    IsMenu.Items[0].Text = ResourceManager.GetString("Pub_State_visible");
                    IsMenu.Items[1].Text = ResourceManager.GetString("Pub_State_invisible");
                    #endregion
                }
            }
        }

        /// <summary>
        /// 将模块类型数据绑定到DataSet
        /// </summary>
        public void BindTypeOrder()
        {
            DataSet ds = bll.GetModuleTypeList("");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string s=ds.Tables[0].Rows[i]["ModuleTypeName"].ToString();
                if (ResourceManager.GetString(s) != "")
                {
                    ModuleType.Items.Add(new ListItem(ResourceManager.GetString(s), ds.Tables[0].Rows[i]["ModuleTypeID"].ToString()));
                }
                else
                {
                    ModuleType.Items.Add(new ListItem(s, ds.Tables[0].Rows[i]["ModuleTypeID"].ToString()));
                }
                
            }
            ModuleType.DataBind();
        }

        /// <summary>
        /// 将数据绑定到DataSet
        /// </summary>
        public void BindOrder()
        {
            DataSet ds = bll.GetModuleList("ModuleTypeID=" + ModuleType.SelectedValue);

            if (ds.Tables[0].Rows.Count == 0)
            {
                Glist.Visible = false;
            }
            else
            {
                Glist.Visible = true;
                GridViewMsg.InnerText = ResourceManager.GetString("Pub_Lab_gy") + ds.Tables[0].Rows.Count + ResourceManager.GetString("Pub_Lab_tjl");
            }

            ModuleList.DataSource = ds;
            ModuleList.DataBind();
        }

        /// <summary>
        /// 添加时初始化权限
        /// </summary>
        public void BindPermission()
        {
            StringBuilder strState = new StringBuilder();
            StringBuilder strTag = new StringBuilder();            

            RedGlovePermission.BLL.RGP_AuthorityDir Abll = new RedGlovePermission.BLL.RGP_AuthorityDir();
            DataSet ds = Abll.GetAuthorityList("", "order by AuthorityOrder asc");

             int rcount=ds.Tables[0].Rows.Count;
            AuthorityNum.Text = rcount.ToString();
            if (rcount == 0)
            {
                divstate.InnerHtml = ResourceManager.GetString("Pub_Msg_norecord");
            }
            else
            {
                strState.Append("<table width=\"200\" border=\"0\" cellpadding=\"0\" cellspacing=\"2\">");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if ((i + 1) % 2 != 0)
                    {
                        #region 左边
                        strState.Append("<tr><td width=\"50\"><span class=\"RoleTitle\">" + ResourceManager.GetString(ds.Tables[0].Rows[i]["AuthorityName"].ToString())
                            + "</span></td><td width=\"100\"><table id=\"Alist" + i.ToString()
                            + "\" border=\"0\"><tr><td><input id=\"Alist" + i.ToString()
                            + "_0\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"1\" />"
                            + "<label for=\"Alist" + i.ToString() + "_0\">" + ResourceManager.GetString("Pub_State_enabled") + "</label></td><td>"
                            + "<input id=\"Alist" + i.ToString() + "_1\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"0\" checked=\"checked\" />"
                            + "<label for=\"Alist" + i.ToString() + "_1\">" + ResourceManager.GetString("Pub_State_disable") + "</label></td></tr></table></td>");
                        
                        #endregion
                    }
                    else
                    {
                        #region 左边
                        strState.Append("<td width=\"50\"><span class=\"RoleTitle\">" + ResourceManager.GetString(ds.Tables[0].Rows[i]["AuthorityName"].ToString())
                            + "</span></td><td width=\"100\"><table id=\"Alist" + i.ToString()
                            + "\" border=\"0\"><tr><td><input id=\"Alist" + i.ToString()
                            + "_0\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"1\" />"
                            + "<label for=\"Alist" + i.ToString() + "_0\">" + ResourceManager.GetString("Pub_State_enabled") + "</label></td><td>"
                            + "<input id=\"Alist" + i.ToString() + "_1\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"0\" checked=\"checked\" />"
                            + "<label for=\"Alist" + i.ToString() + "_1\">" + ResourceManager.GetString("Pub_State_disable") + "</label></td></tr></table></td></tr>");

                        #endregion
                    }

                    //写入标识
                    strTag.Append("<input type=\"hidden\" name=\"Atag" + i.ToString() + "\" id=\"Atag" + i.ToString() + "\" value=\"" + ds.Tables[0].Rows[i]["AuthorityTag"] + "\" />");
                }

                if ((rcount) % 2 == 0)
                {
                    strState.Append("</table>");
                }
                else
                {
                    strState.Append("<td width=\"50\">&nbsp;</td><td width=\"100\">&nbsp;</td></tr></table>");
                }

                divstate.InnerHtml = strState.ToString() + strTag.ToString();
            }
        }

        /// <summary>
        /// 更新时初始化权限
        /// </summary>
        /// <param name="ModuleID"></param>
        public void BindPermissionUpdate(int ModuleID)
        {
            #region 模块数据绑定

            RedGlovePermission.Model.RGP_Modules model = new RedGlovePermission.Model.RGP_Modules();
            model = bll.GetModuleModel(ModuleID);
            M_ID.Text = model.ModuleID.ToString();
            ModuleType.SelectedValue = model.ModuleTypeID.ToString();
            txt_Name.Text = model.ModuleName;
            txt_tag.Text = model.ModuleTag;
            txt_url.Text = model.ModuleURL;
            if (model.ModuleDisabled)
            { txt_state.SelectedIndex = 0; }
            else
            { txt_state.SelectedIndex = 1; }

            txt_order.Text = model.ModuleOrder.ToString();
            txt_Description.Text = model.ModuleDescription;

            if (model.IsMenu)
            { IsMenu.SelectedIndex = 0; }
            else
            { IsMenu.SelectedIndex = 1; }

            #endregion

            #region 权限数据绑定

            StringBuilder strState = new StringBuilder();//状态
            StringBuilder strTag = new StringBuilder();//标识
            StringBuilder strVerify = new StringBuilder();//对比状态            

            RedGlovePermission.BLL.RGP_AuthorityDir Abll = new RedGlovePermission.BLL.RGP_AuthorityDir();
            DataSet MALDS = bll.GetAuthorityList(ModuleID);
            DataSet ds = Abll.GetAuthorityList("", "order by AuthorityOrder asc");

            int rcount=ds.Tables[0].Rows.Count;
            AuthorityNum.Text = rcount.ToString();
            if (rcount == 0)
            {
                divstate.InnerHtml = ResourceManager.GetString("Pub_Msg_norecord");
            }
            else
            {
                strVerify.Append("<input type=\"hidden\" name=\"verifystate\" id=\"verifystate\" value=\"");
                strState.Append("<table width=\"340\" border=\"0\" cellpadding=\"0\" cellspacing=\"2\">");
                for (int i = 0; i < rcount; i++)
                {
                    bool v = false;
                    for (int k = 0; k < MALDS.Tables[0].Rows.Count; k++)
                    {
                        if (MALDS.Tables[0].Rows[k]["AuthorityTag"].ToString() == ds.Tables[0].Rows[i]["AuthorityTag"].ToString())
                        {
                            v = true;
                            break;
                        }
                    }
                    if ((i + 1) % 2 != 0)
                    {
                        #region 左边
                        if (v)//是否为可以使用的权限
                        {
                            strState.Append("<tr><td width=\"50\"><span class=\"RoleTitle\">" + ResourceManager.GetString(ds.Tables[0].Rows[i]["AuthorityName"].ToString())
                                + "</span></td><td width=\"100\"><table id=\"Alist" + i.ToString()
                                + "\" border=\"0\"><tr><td><input id=\"Alist" + i.ToString()
                                + "_0\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"1\" checked=\"checked\" />"
                                + "<label for=\"Alist" + i.ToString() + "_0\">" + ResourceManager.GetString("Pub_State_enabled") + "</label></td><td>"
                                + "<input id=\"Alist" + i.ToString() + "_1\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"0\" />"
                                + "<label for=\"Alist" + i.ToString() + "_1\">" + ResourceManager.GetString("Pub_State_disable") + "</label></td></tr></table></td>");
                            strVerify.Append("1,");
                        }
                        else
                        {
                            strState.Append("<tr><td width=\"50\"><span class=\"RoleTitle\">" + ResourceManager.GetString(ds.Tables[0].Rows[i]["AuthorityName"].ToString())
                                + "</span></td><td width=\"100\"><table id=\"Alist" + i.ToString()
                                + "\" border=\"0\"><tr><td><input id=\"Alist" + i.ToString()
                                + "_0\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"1\" />"
                                + "<label for=\"Alist" + i.ToString() + "_0\">" + ResourceManager.GetString("Pub_State_enabled") + "</label></td><td>"
                                + "<input id=\"Alist" + i.ToString() + "_1\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"0\" checked=\"checked\" />"
                                + "<label for=\"Alist" + i.ToString() + "_1\">" + ResourceManager.GetString("Pub_State_disable") + "</label></td></tr></table></td>");
                            strVerify.Append("0,");
                        }
                        #endregion
                    }
                    else
                    {
                        #region 右边
                        if (v)//是否为可以使用的权限
                        {
                            strState.Append("<td width=\"50\"><span class=\"RoleTitle\">" + ResourceManager.GetString(ds.Tables[0].Rows[i]["AuthorityName"].ToString())
                                + "</span></td><td width=\"100\"><table id=\"Alist" + i.ToString()
                                + "\" border=\"0\"><tr><td><input id=\"Alist" + i.ToString()
                                + "_0\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"1\" checked=\"checked\" />"
                                + "<label for=\"Alist" + i.ToString() + "_0\">" + ResourceManager.GetString("Pub_State_enabled") + "</label></td><td>"
                                + "<input id=\"Alist" + i.ToString() + "_1\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"0\" />"
                                + "<label for=\"Alist" + i.ToString() + "_1\">" + ResourceManager.GetString("Pub_State_disable") + "</label></td></tr></table></td></tr>");
                            strVerify.Append("1,");
                        }
                        else
                        {
                            strState.Append("<td width=\"50\"><span class=\"RoleTitle\">" + ResourceManager.GetString(ds.Tables[0].Rows[i]["AuthorityName"].ToString())
                                + "</span></td><td width=\"100\"><table id=\"Alist" + i.ToString()
                                + "\" border=\"0\"><tr><td><input id=\"Alist" + i.ToString()
                                + "_0\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"1\" />"
                                + "<label for=\"Alist" + i.ToString() + "_0\">" + ResourceManager.GetString("Pub_State_enabled") + "</label></td><td>"
                                + "<input id=\"Alist" + i.ToString() + "_1\" type=\"radio\" name=\"Alist" + i.ToString() + "\" value=\"0\" checked=\"checked\" />"
                                + "<label for=\"Alist" + i.ToString() + "_1\">" + ResourceManager.GetString("Pub_State_disable") + "</label></td></tr></table></td></tr>");
                            strVerify.Append("0,");
                        }
                        #endregion
                    }
                    //写入标识
                    strTag.Append("<input type=\"hidden\" name=\"Atag" + i.ToString() + "\" id=\"Atag" + i.ToString() + "\" value=\"" + ds.Tables[0].Rows[i]["AuthorityTag"] + "\" />");
                }
                
                strVerify.Append("\" />");

                if ((rcount) % 2 == 0)
                {
                    strState.Append("</table>");
                }
                else
                {
                    strState.Append("<td width=\"50\">&nbsp;</td><td width=\"100\">&nbsp;</td></tr></table>");
                }

                divstate.InnerHtml = strState.ToString() + strTag.ToString() + strVerify.ToString();
            }
            #endregion
        }

        /// <summary>
        /// 分页
        /// </summary>
        protected void ModuleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ModuleList.PageIndex = e.NewPageIndex;
            BindOrder(); //重新绑定GridView数据的函数 
        }

        /// <summary>
        /// 执行事件,删除操作,查看/编辑模块信息
        /// </summary>
        protected void ModuleList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "Del":
                    if (bll.DeleteModule(int.Parse(e.CommandArgument.ToString())))
                    {
                        AddPanel.Visible = false;
                        BindOrder();
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_delete_true") + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_delete_false") + "');", true);
                    }
                    break;
                case "EditView":
                    BindPermissionUpdate(int.Parse(e.CommandArgument.ToString()));
                    btn_save.Visible = false;
                    btn_update.Visible = true;
                    AddPanel.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// 数据绑定到表格
        /// </summary>
        protected void ModuleList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        /// 选择分类
        /// </summary>
        protected void ModuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearTxt();
            AddPanel.Visible = false;
            BindOrder();
        }

        /// <summary>
        /// 新增模块
        /// </summary>
        protected void btn_add_Click(object sender, EventArgs e)
        {
            clearTxt();
            BindPermission();
            btn_update.Visible = false;
            btn_save.Visible = true;
            AddPanel.Visible = true;
        }

        /// <summary>
        /// 清空输入框数据
        /// </summary>
        public void clearTxt()
        {
            txt_Name.Text = "";
            txt_tag.Text = "";
            txt_url.Text = "";
            txt_state.SelectedIndex = 0;
            txt_order.Text = "";
            txt_Description.Text = "";
        }

        /// <summary>
        /// 取消新增模块
        /// </summary>
        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            AddPanel.Visible = false;
        }

        /// <summary>
        /// 保存新增模块信息
        /// </summary>
        protected void btn_save_Click(object sender, EventArgs e)
        {
            RedGlovePermission.Model.RGP_Modules model = new RedGlovePermission.Model.RGP_Modules();
            model.ModuleTypeID = int.Parse(ModuleType.SelectedValue);
            model.ModuleName = txt_Name.Text.Trim();
            model.ModuleTag = txt_tag.Text.Trim();
            model.ModuleURL = txt_url.Text.Trim();
            if (txt_state.SelectedValue == "0")
            { model.ModuleDisabled = true; }
            else
            { model.ModuleDisabled = true; }
            model.ModuleOrder = int.Parse(txt_order.Text.Trim());
            model.ModuleDescription = txt_Description.Text.Trim();

            if (IsMenu.SelectedValue == "0")
            { model.IsMenu = false; }
            else
            { model.IsMenu = true; }

            if (!bll.ModuleExists(txt_tag.Text.Trim()))
            {
                int RowID = bll.CreateModule(model);//返回模块ID;
                if (RowID != 0)//添加OK
                {
                    ArrayList list = new ArrayList();//建立事务列表
                    int n = int.Parse(AuthorityNum.Text);
                    for (int i = 0; i < n; i++)
                    {
                        if (Request.Form["Alist" + i.ToString()].ToString() == "1")//如果允许则插入记录
                        {
                            string item = string.Empty;
                            item = item + RowID.ToString() + "|" + Request.Form["Atag" + i.ToString()].ToString();
                            list.Add(item);
                        }
                    }
                    //权限加入是否成功！
                    if (bll.CreateAuthorityList(list))
                    {
                        clearTxt();
                        if (!Glist.Visible) Glist.Visible = true;
                        BindOrder();
                        AddPanel.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_true") + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_false") + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_add_false") + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_Iscaption") + "');", true);
            }
        }

        /// <summary>
        /// 更新模块信息
        /// </summary>
        protected void btn_update_Click(object sender, EventArgs e)
        {
            RedGlovePermission.Model.RGP_Modules model = new RedGlovePermission.Model.RGP_Modules();
            model.ModuleID = int.Parse(M_ID.Text);
            model.ModuleTypeID = int.Parse(ModuleType.SelectedValue);
            model.ModuleName = txt_Name.Text.Trim();
            model.ModuleTag = txt_tag.Text.Trim();
            model.ModuleURL = txt_url.Text.Trim();
            if (txt_state.SelectedValue == "0")
            { model.ModuleDisabled = false; }
            else
            { model.ModuleDisabled = true; }
            model.ModuleOrder = int.Parse(txt_order.Text.Trim());
            model.ModuleDescription = txt_Description.Text.Trim();
            if (IsMenu.SelectedValue == "0")
            { model.IsMenu = false; }
            else
            { model.IsMenu = true; }

            string[] vstr = Request.Form["verifystate"].Split(',');

            switch (bll.UpdateModule(model))
            {
                case 1:
                    ArrayList list = new ArrayList();//建立事务列表
                    int n = int.Parse(AuthorityNum.Text);
                    for (int i = 0; i < n; i++)
                    {
                        //判断权限是否有变化
                        if (vstr[i] != Request.Form["Alist" + i.ToString()].ToString())
                        {
                            string item = string.Empty;
                            item = item + model.ModuleID.ToString() + "|"
                                + Request.Form["Atag" + i.ToString()].ToString() + "|"
                                + Request.Form["Alist" + i.ToString()].ToString();//判断插入增加还是删除
                            list.Add(item);
                        }
                    }
                    //权限更新是否成功！
                    if (bll.UpdateAuthorityList(list))
                    {
                        BindPermissionUpdate(model.ModuleID);
                        BindOrder();
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_true") + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "');", true);
                    }
                    break;
                case 2:
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_Iscaption") + "');", true);
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "');", true);
                    break;
            }

        }

        protected void ModuleList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;

                #region 语言
                tcHeader[0].Text = ResourceManager.GetString("Modules_Lab_0");
                tcHeader[1].Text = ResourceManager.GetString("Modules_Lab_1");
                tcHeader[2].Text = ResourceManager.GetString("Modules_Lab_6");
                tcHeader[3].Text = ResourceManager.GetString("Modules_Lab_5");
                tcHeader[4].Text = ResourceManager.GetString("Modules_Lab_4");
                tcHeader[5].Text = ResourceManager.GetString("Pub_Lbtn_update");
                tcHeader[6].Text = ResourceManager.GetString("Pub_Lbtn_delete");
                #endregion
            }
        }
    }
}
