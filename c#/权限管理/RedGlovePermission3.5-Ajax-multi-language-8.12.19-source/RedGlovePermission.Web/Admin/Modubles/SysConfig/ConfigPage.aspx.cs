using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

/**************************************
 * 模块：系统配置
 * 作者：Nick.Yan
 * 日期: 2008-11-13
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web.Admin.Modubles.SysConfig
{
    public partial class ConfigPage : CommonPage
    {
        RedGlovePermission.BLL.RGP_Configuration bll = new RedGlovePermission.BLL.RGP_Configuration();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Admin/Login.aspx");
            }
            else
            {
                //初始化模块权限
                UserHandle.InitModule("Mod_Config");
                if (!IsPostBack)
                {
                    BindOrder();
                }
            }
        }

        /// <summary>
        /// 将数据绑定到DataSet
        /// </summary>
        public void BindOrder()
        {
            DataSet ds = bll.GetItemList("");
            ConfigList.DataSource = ds;
            ConfigList.DataBind();
        }

        /// <summary>
        /// 分页
        /// </summary>
        protected void ConfigList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ConfigList.PageIndex = e.NewPageIndex;
            BindOrder(); //重新绑定GridView数据的函数 
        }

        /// <summary>
        /// 退出编辑状态
        /// </summary>
        protected void ConfigList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ConfigList.EditIndex = -1;
            BindOrder();
        }

        /// <summary>
        /// 变更到编辑状态
        /// </summary>
        protected void ConfigList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ConfigList.EditIndex = e.NewEditIndex;
            BindOrder();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        protected void ConfigList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {            
            int id = int.Parse(ConfigList.DataKeys[e.RowIndex].Values[0].ToString());
            string s = ((TextBox)ConfigList.Rows[e.RowIndex].FindControl("txt_value")).Text.Trim();

            if (!bll.UpdateItem(id,s))
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "MsgBox", "alert('" + ResourceManager.GetString("Pub_Msg_update_false") + "')", true);
            }
            //返回浏览狀態
            ConfigList.EditIndex = -1;
            BindOrder();

        }

        /// <summary>
        /// 数据绑定到表格
        /// </summary>
        protected void ConfigList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)//判定当前的行是否属于datarow类型的行
            {
                //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#ffffcd',this.style.fontWeight='';");
                //当鼠标离开的时候 将背景颜色还原的以前的颜色
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            }
        }

        protected void ConfigList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCellCollection tcHeader = e.Row.Cells;

                #region 语言
                tcHeader[0].Text = ResourceManager.GetString("Pub_Lbtn_update");
                tcHeader[1].Text = ResourceManager.GetString("RolesPage_Lab_0");
                tcHeader[2].Text = ResourceManager.GetString("RolesPage_Lab_1");
                tcHeader[3].Text = ResourceManager.GetString("RolesPage_Lab_2");
                tcHeader[4].Text = ResourceManager.GetString("RolesPage_Lab_3");
                #endregion
            }
        }
    }
}
