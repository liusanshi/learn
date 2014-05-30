/**************************************
* 作用：防止用户多点登陆，安全检测
* 作者：Nick.Yan
* 日期: 2008-02-11
* 网址：www.redglove.com.cn
**************************************/
using System;
using System.Collections;
using System.Web;

namespace RedGlovePermission.Web
{
    public class CommonPage : System.Web.UI.Page
    {
        public CommonPage()
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 
        }

        override protected void OnInit(EventArgs e)
        {
            Hashtable hOnline = (Hashtable)Application["Online"];
            if (hOnline != null)
            {
                IDictionaryEnumerator IDE = hOnline.GetEnumerator();
                while (IDE.MoveNext())
                {
                    if (IDE.Key != null && IDE.Key.ToString().Equals(Session.SessionID))
                    {
                        if (IDE.Value != null && "XXXXXX".Equals(IDE.Value.ToString()))
                        {
                            hOnline.Remove(Session.SessionID);
                            Application.Lock();
                            Application["Online"] = hOnline;
                            Application.UnLock();
                            Response.Redirect("~/Error.aspx?f=2");
                            return;
                        }
                        break;
                    }
                }
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpRequestValidationException)
            {
                Response.Redirect("~/Error.aspx?f=5");
                //Response.Write(ex.Message.ToString());
                Server.ClearError(); // 如果不ClearError()这个异常会继续传到Application_Error()。
            }
        }
    }
}
