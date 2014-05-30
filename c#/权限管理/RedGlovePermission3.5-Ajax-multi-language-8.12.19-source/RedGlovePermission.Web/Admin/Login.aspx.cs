using System;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RedGlove.Core.Languages;

namespace RedGlovePermission.Web.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                               
                btn_login.Attributes.Add("onclick", "return checklogin()");

                //取得当前系统的语言版本
                string[] strluanage = Request.ServerVariables.Get("HTTP_ACCEPT_LANGUAGE").ToLower().Split(',');

                //语言类型样式下拉选择框。
                NameValueCollection nvc = (NameValueCollection)ConfigurationManager.GetSection("WebLanguage");
                ArrayList Languagelist = new ArrayList();//语言列表

                for (int i = 0; i < nvc.Count; i++)
                {
                    Language.Items.Add(new ListItem(nvc[nvc.GetKey(i)], nvc.GetKey(i).ToLower()));
                    Languagelist.Add(nvc.GetKey(i).ToLower());
                }

                if (strluanage != null)
                {
                    //检测浏览器语言版本是否在语言列表中
                    bool aa = false;
                    for (int i = 0; i < Languagelist.Count; i++)
                    {
                        if (strluanage[0] == Languagelist[i].ToString())
                        {
                            aa = true;
                        }
                    }

                    if (aa)
                    {
                        HttpCookie WebLanguage = new HttpCookie("WebLanguage", strluanage[0].ToLower());
                        Session["userLanguage"] = strluanage[0].ToLower();
                        //默认语言选择
                        Language.SelectedValue = strluanage[0].ToLower();
                    }
                    else
                    {
                        string s = ConfigurationManager.AppSettings["InitLanguages"].ToLower();
                        //如果检测浏览器语言版在系统没有语言，则转成英文
                        HttpCookie WebLanguage = new HttpCookie("WebLanguage", s);
                        Session["userLanguage"] = s;
                        //默认语言选择
                        Language.SelectedValue = s;
                    }
                }

                UserName.Focus();

                #region 语言加入
                btn_login.Text = ResourceManager.GetString("Login_Btn_login");
                #endregion
            }
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            RedGlovePermission.BLL.Users bll = new RedGlovePermission.BLL.Users();
            RedGlovePermission.Model.Users model = new RedGlovePermission.Model.Users();

            if (UserName.Text.Trim() == "" || Password.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "", "alert('" + ResourceManager.GetString("Login_Msg_V_3") + "');", true);
            }
            else
            {
                if (bll.CheckLogin(UserName.Text.Trim(), RedGlovePermission.Lib.SecurityEncryption.MD5(Password.Text.Trim(), 32)))
                {
                    model = bll.GetUserModel(UserName.Text.Trim());
                    if (model.RoleID != 0)
                    {
                        if (model.Status != 0)
                        {
                            #region 登录成功，将登录id存入hash表

                            Hashtable hOnline = (Hashtable)Application["Online"];
                            if (hOnline != null)
                            {
                                int i = 0;
                                while (i < hOnline.Count) //增加此判断强制查询到底 
                                {
                                    IDictionaryEnumerator idE = hOnline.GetEnumerator();
                                    string strKey = "";
                                    while (idE.MoveNext())
                                    {
                                        if (idE.Value != null && idE.Value.ToString().Equals(model.UserName))
                                        {
                                            //已经登录             
                                            strKey = idE.Key.ToString();
                                            hOnline[strKey] = "XXXXXX";
                                            break;
                                        }
                                    }
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                hOnline = new Hashtable();
                            }
                            hOnline[Session.SessionID] = model.UserName;
                            Application.Lock();
                            Application["Online"] = hOnline;
                            Application.UnLock();

                            #endregion

                            bll.UpdateLoginTime(model.UserID);//更新登录时间
                            Session["userLanguage"] = Language.SelectedValue;
                            SessionBox.CreateUserSession(new UserSession(model.UserID, model.UserName, model.RoleID, model.IsLimit, model.Status));

                            Response.Redirect("~/Admin/Frameset/Default.aspx");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "", "alert('" + ResourceManager.GetString("Login_Msg_2") + "');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "", "alert('" + ResourceManager.GetString("Login_Msg_3") + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(CustomPanel1, this.GetType(), "", "alert('" + ResourceManager.GetString("Login_Msg_2") + "');", true);
                }
            }
        }
    }
}
