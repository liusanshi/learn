using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：角色授权数据管理
 * 作者：Nick.Yan
 * 日期: 2008-11-13
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.SysConfig
{
    public partial class RoleAuthorizedPage : CommonPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                //初始化模块权限
                UserHandle.InitModule("Mod_RoleAuthority");

                if (!IsPostBack)
                {
                    BindGroups();
                    BindModuleType();
                    BindRoles();

                    #region 语言加入
                    btn_AllSave.Text = ResourceManager.GetString("RoleAuthorizedPage_Btn_saveall");
                    #endregion
                }
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        protected void BindRoles()
        {
            RedGlovePermission.BLL.RGP_Roles Rolsebll = new RedGlovePermission.BLL.RGP_Roles();
            DataSet ds = Rolsebll.GetRoleList("RoleGroupID=" + GroupList.SelectedValue, "");
            RoleView.DataSource = ds;
            RoleView.DataBind();
        }

        /// <summary>
        /// 获取模块权限列表
        /// </summary>
        protected void BindModules()
        {
            if (Rid.Text != "")
            {
                RedGlovePermission.BLL.RGP_Modules Mbll = new RedGlovePermission.BLL.RGP_Modules();
                DataSet ds = Mbll.GetModuleList("ModuleDisabled=1 and ModuleTypeID=" + ModuleTypeList.SelectedValue);
                ModuleView.DataSource = ds;
                ModuleView.DataBind();
            }
            else
            {
                ModuleView.DataSource = null;
                ModuleView.DataBind();
            }
        }

        /// <summary>
        /// 绑定分组数据
        /// </summary>
        protected void BindGroups()
        {
            RedGlovePermission.BLL.RGP_Groups Groupbll = new RedGlovePermission.BLL.RGP_Groups();
            DataSet ds = Groupbll.GetGroupList("", "order by GroupOrder asc");

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
        }

        /// <summary>
        /// 绑定分组数据
        /// </summary>
        protected void BindModuleType()
        {
            RedGlovePermission.BLL.RGP_Modules MTbll = new RedGlovePermission.BLL.RGP_Modules();
            DataSet ds = MTbll.GetModuleTypeList("");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string s = ds.Tables[0].Rows[i]["ModuleTypeName"].ToString();
                if (ResourceManager.GetString(s) != "")
                {
                    ModuleTypeList.Items.Add(new ListItem(ResourceManager.GetString(s), ds.Tables[0].Rows[i]["ModuleTypeID"].ToString()));
                }
                else
                {
                    ModuleTypeList.Items.Add(new ListItem(s, ds.Tables[0].Rows[i]["ModuleTypeID"].ToString()));
                }
            }
        }

        /// <summary>
        /// 查看角权限
        /// </summary>
        protected void RoleView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "EditView")
            {
                Rid.Text = e.CommandArgument.ToString();
                if (!ModuleTypeList.Enabled)
                    ModuleTypeList.Enabled = true;
                //int.Parse(e.CommandArgument.ToString());

                BindModules();
            }
        }

        /// <summary>
        /// 根据分组排序角色
        /// </summary>
        protected void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Rid.Text = "";
            ModuleTypeList.Enabled = false;
            BindRoles();
            BindModules();
        }

        /// <summary>
        /// 根据模块分类排序模块
        /// </summary>
        protected void ModuleTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindModules();
        }

        /// <summary>
        /// 模块分类数据绑定
        /// </summary>
        protected void ModuleView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RedGlovePermission.BLL.RGP_AuthorityDir AD = new RedGlovePermission.BLL.RGP_AuthorityDir();
                RedGlovePermission.BLL.RGP_Modules Modulebll = new RedGlovePermission.BLL.RGP_Modules();
                RedGlovePermission.BLL.RGP_Roles Rolebll = new RedGlovePermission.BLL.RGP_Roles();

                CheckBoxList AuthorityList = (CheckBoxList)e.Row.FindControl("AuthorityList");
                Label lab_ID = (Label)e.Row.FindControl("lab_ID");
                Label lab_Verify = (Label)e.Row.FindControl("lab_Verify");

                DataSet ds = AD.GetAuthorityList("", "order by AuthorityOrder asc");
                DataSet MALDS = Modulebll.GetAuthorityList(int.Parse(lab_ID.Text));
                DataSet RALDS= Rolebll.GetRoleAuthorityList(int.Parse(Rid.Text),int.Parse(lab_ID.Text));

                int n = ds.Tables[0].Rows.Count;//系统权限个数

                string[] vstate = new string[n];

                //获取系统配置的权限列表，如果模块没有该权限，则禁用该权限
                for (int i = 0; i < n; i++)
                {
                    AuthorityList.Items.Add(new ListItem(ResourceManager.GetString(ds.Tables[0].Rows[i]["AuthorityName"].ToString()), ds.Tables[0].Rows[i]["AuthorityTag"].ToString()));
                    AuthorityList.Items[i].Enabled = false;
                    for (int k = 0; k < MALDS.Tables[0].Rows.Count; k++)
                    {
                        if (ds.Tables[0].Rows[i]["AuthorityTag"].ToString() == MALDS.Tables[0].Rows[k]["AuthorityTag"].ToString())
                        {
                            AuthorityList.Items[i].Enabled = true;
                            break;
                        }
                    }
                    vstate[i] = "0";//初始状态数组;
                }
                AuthorityList.DataBind();

                //将模块权限付值
                for (int j = 0; j < RALDS.Tables[0].Rows.Count; j++)
                {
                    for (int l = 0; l < AuthorityList.Items.Count; l++)
                    {
                        if (RALDS.Tables[0].Rows[j]["AuthorityTag"].ToString() == AuthorityList.Items[l].Value)
                        {
                            if (AuthorityList.Items[l].Enabled)
                                vstate[l] = "1";//权限存在
                                AuthorityList.Items[l].Selected = true;
                            break;
                        }
                    }
                }

                lab_Verify.Text = RedGlovePermission.Lib.TypeParse.StringArrayToString(vstate, ',');
            }
        }

        /// <summary>
        /// 保存所有修改的权限
        /// </summary>
        protected void btn_AllSave_Click(object sender, EventArgs e)
        {
            RedGlovePermission.BLL.RGP_Roles Rolebll = new RedGlovePermission.BLL.RGP_Roles();

            ArrayList list = new ArrayList();//建立事务列表
            for (int i = 0; i <= ModuleView.Rows.Count - 1; i++)
            {
                CheckBoxList cal = (CheckBoxList)this.ModuleView.Rows[i].Cells[1].FindControl("AuthorityList");
                Label lab_Verify = (Label)this.ModuleView.Rows[i].Cells[0].FindControl("lab_Verify");
                string[] vstate = lab_Verify.Text.Split(',');//获取原始状态

                for (int j = 0; j < cal.Items.Count; j++)
                {
                    if (cal.Items[j].Enabled)
                    {
                        if (cal.Items[j].Selected)
                        {
                            if (vstate[j] != "1")//检查数据有没有变化
                            {
                                string item = string.Empty;
                                item = item + Rid.Text + "|"
                                    + this.ModuleView.DataKeys[i].Values[0].ToString() + "|"
                                    + cal.Items[j].Value + "|1";//设置为1，加入权限
                                list.Add(item);
                            }
                        }
                        else
                        {
                            if (vstate[j] != "0")//检查数据有没有变化
                            {
                                string item = string.Empty;
                                item = item + Rid.Text + "|"
                                    + this.ModuleView.DataKeys[i].Values[0].ToString() + "|"
                                    + cal.Items[j].Value + "|0";//设置为0，删除删除
                                list.Add(item);
                            }
                        }
                    }
                }
            }

            if (Rolebll.UpdateRoleAuthority(list))
            {
                BindModules();
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('设置成功！')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('设置操作失败！')", true);
            }
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        protected void ModuleView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            ArrayList list = new ArrayList();//建立事务列表
            RedGlovePermission.BLL.RGP_Roles Rolebll = new RedGlovePermission.BLL.RGP_Roles();
            CheckBoxList cal = (CheckBoxList)this.ModuleView.Rows[e.NewSelectedIndex].Cells[1].FindControl("AuthorityList");
            Label lab_Verify = (Label)this.ModuleView.Rows[e.NewSelectedIndex].Cells[0].FindControl("lab_Verify");
            string[] vstate = lab_Verify.Text.Split(',');//获取原始状态

            for (int i = 0; i < cal.Items.Count; i++)
            {
                if (cal.Items[i].Enabled)
                {
                    if (cal.Items[i].Selected)
                    {
                        if (vstate[i] != "1")//检查数据有没有变化
                        {
                            string item = string.Empty;
                            item = item + Rid.Text + "|"
                                + this.ModuleView.DataKeys[e.NewSelectedIndex].Values[0].ToString() + "|"
                                + cal.Items[i].Value + "|1";//设置为1，加入权限
                            list.Add(item);
                        }
                    }
                    else
                    {
                        if (vstate[i] != "0")//检查数据有没有变化
                        {
                            string item = string.Empty;
                            item = item + Rid.Text + "|"
                                + this.ModuleView.DataKeys[e.NewSelectedIndex].Values[0].ToString() + "|"
                                + cal.Items[i].Value + "|0";//设置为0，删除删除
                            list.Add(item);
                        }
                    }
                }
            }

            if (Rolebll.UpdateRoleAuthority(list))
            {
                BindModules();
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_true") + "')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "')", true);
            }

        }

        protected void ModuleView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;

                #region 语言
                tcHeader[0].Text = ResourceManager.GetString("RoleAuthorizedPage_Lab_1");
                tcHeader[1].Text = ResourceManager.GetString("RoleAuthorizedPage_Lab_2");
                #endregion
            }
        }
    }
}
