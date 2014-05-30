using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using RedGlove.Core.Languages;

namespace RedGlovePermission.Web.Admin.Frameset
{
    public partial class Default : CommonPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                try
                {
                    if (!IsPostBack)
                    {
                        
                        //在Cookie中保存界面样式选择。
                        if (Response.Cookies["UIStyle"].Value == null)
                        {
                            HttpCookie style = new HttpCookie("UIStyle", "default");
                            style.Expires = DateTime.Now.AddMonths(1);
                            Response.Cookies.Add(style);
                        }
                        Request.Cookies["UIStyle"].Value = ConfigurationManager.AppSettings["InitStyle"];
                        loginname.Text = SessionBox.GetUserSession().LoginName;
                        loadStyle();//加载样式
                        LoadModuleTree();//加载菜单
                    }
                }
                catch
                {
                    MenuTreeView.Nodes.Clear();
                }
            }
        }

        /// <summary>
        /// 获取菜单数据
        /// </summary>
        private void LoadModuleTree()
        {
            //获取当前登录的会员信息。
            UserSession user = SessionBox.GetUserSession();
            RedGlovePermission.BLL.RGP_Modules bll = new RedGlovePermission.BLL.RGP_Modules();

            //获取所有顶层模块。
            DataSet ModuleType = bll.GetModuleTypeList("");

            //增加模块分类和模块。
            if (ModuleType.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ModuleType.Tables[0].Rows)
                {
                    //添加顶级分类
                    string s = ResourceManager.GetString(dr["ModuleTypeName"].ToString());
                    if (s != null)
                    {
                        s = ResourceManager.GetString(dr["ModuleTypeName"].ToString());
                    }
                    else
                    {
                        s = dr["ModuleTypeName"].ToString();
                    }
                    TreeNode masterNode = new TreeNode(s);
                    MenuTreeView.Nodes.Add(masterNode);
                    masterNode.SelectAction = TreeNodeSelectAction.Expand;
                    masterNode.Expanded = false;
                    int i = 0;
                    //增加子模块分类
                    DataSet Module = bll.GetModuleList("ModuleTypeID=" + dr["ModuleTypeID"].ToString());
                    foreach (DataRow child_dr in Module.Tables[0].Rows)
                    {
                        if ((user.IsLimit == true
                            || ((child_dr["ModuleDisabled"].ToString().ToLower() == "true") || (child_dr["ModuleDisabled"].ToString().ToLower() == "1")
                            && UserHandle.ValidationModule(int.Parse(child_dr["ModuleID"].ToString()), RGP_Tag.Browse)))
                            && (child_dr["IsMenu"].ToString().ToLower() == "true" || child_dr["IsMenu"].ToString().ToLower() == "1"))
                        {
                            string childs = ResourceManager.GetString(child_dr["ModuleName"].ToString());
                            if (childs != null)
                            {
                                childs = ResourceManager.GetString(child_dr["ModuleName"].ToString());
                            }
                            else
                            {
                                childs = child_dr["ModuleName"].ToString();
                            }
                            TreeNode childNode = new TreeNode(childs);
                            childNode.Expanded = false;
                            childNode.NavigateUrl = child_dr["ModuleURL"].ToString();
                            childNode.Target = "modulePanel";
                            childNode.ToolTip = child_dr["ModuleDescription"].ToString();
                            masterNode.ChildNodes.Add(childNode);
                            i++;
                        }
                    }
                    //删除不必要的模块分类节点。
                    if (i == 0)
                    {
                        MenuTreeView.Nodes.Remove(masterNode);
                    }
                }

                MenuTreeView.CollapseImageToolTip = "";
                MenuTreeView.ExpandImageToolTip = "";
            }
        }

        /// <summary>
        /// 获取样式列表
        /// </summary>
        private void loadStyle()
        {
            //填充界面样式下拉选择框。
            NameValueCollection UIList = (NameValueCollection)ConfigurationManager.GetSection("StyleList");
            for (int i = 0; i < UIList.Count; i++)
            {
                StyleList.Items.Add(new ListItem(UIList[UIList.GetKey(i)], UIList.GetKey(i)));
            }
            HttpCookie ui = Request.Cookies["UIStyle"];

            if (ui != null)
            {
                ListItem li = StyleList.Items.FindByValue(ui.Value);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        protected void lnkbtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Logout.aspx");
        }

        /// <summary>
        /// 变更样式
        /// </summary>
        protected void StyleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Request.Cookies["UIStyle"].Value = StyleList.SelectedValue.ToLower();
        }
    }
}
