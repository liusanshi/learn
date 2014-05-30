using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**************************************
 * 模块：系统错误提示
 * 作者：Nick.Yan
 * 日期:2008-02-11
 * 网址：www.redglove.com.cn
 * ***********************************/

namespace RedGlovePermission.Web
{
    public partial class Error : System.Web.UI.Page
    {
        public string ret = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["f"] != null)
                ret = Request.QueryString["f"].ToString();

        }
    }
}
