using System;
using System.ServiceProcess;
using System.Collections;

namespace RedGlovePermission.Web
{
    /// <summary>
    /// 用于读、写、删除、比较Session中的用户信息。
    /// </summary>
    [Serializable]
    public class SessionBox
    {
        private SessionBox() { }

        #region 用户Session操作

        /// <summary>
        /// 檢測User Session是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckUserSession()
        {
            object o = System.Web.HttpContext.Current.Session["USER"];
            if (o == null)
                return false;
            else
                return true;

        }

        /// <summary>
        /// 登记User Session
        /// </summary>
        /// <param name="userinfo"></param>
        public static void CreateUserSession(UserSession userinfo)
        {
            System.Web.HttpContext.Current.Session["USER"] = userinfo;
        }

        /// <summary>
        /// 获取User Session
        /// </summary>
        /// <returns></returns>
        public static UserSession GetUserSession()
        {
            object o = System.Web.HttpContext.Current.Session["USER"];
            if (o == null) throw new ExceptionSession("读取UserSession失败。");
            else return (o as UserSession);
        }

        /// <summary>
        /// 移除User Session
        /// </summary>
        public static void RemoveUserSession()
        {
            object o = System.Web.HttpContext.Current.Session["USER"];
            if (o != null) System.Web.HttpContext.Current.Session.Remove("User");
        }

        #endregion

        #region 模块Session操作

        /// <summary>
        /// 登记Moudule Session
        /// </summary>
        /// <param name="lists"></param>
        public static void CreateModuleList(ArrayList lists)
        {
            System.Web.HttpContext.Current.Session["MODULE_TAG"] = lists;
        }

        /// <summary>
        /// 读取模块权限
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetModuleList()
        {
            object o = System.Web.HttpContext.Current.Session["MODULE_TAG"];
            if (o == null) throw new ExceptionSession("读取权限失败。");
            else return (o as ArrayList);
        }

        /// <summary>
        /// 移除模块权限
        /// </summary>
        public static void RemoveModuleList()
        {
            object o = System.Web.HttpContext.Current.Session["MODULE_TAG"];
            if (o != null)
            {
                System.Web.HttpContext.Current.Session.Remove("MODULE_TAG");
                //在移除模块权限时也清掉它的ID
                System.Web.HttpContext.Current.Session.Remove("MID");
            }
        }

        #endregion

        #region 当前已登录会员对当前模块的权限集合

        /// <summary>
        /// 创建模块权限列表
        /// </summary>
        /// <param name="lists"></param>
        public static void CreateAuthority(ArrayList lists)
        {
            System.Web.HttpContext.Current.Session["Authority"] = lists;
        }

        /// <summary>
        /// 读取模块权限
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetAuthority()
        {
            object o = System.Web.HttpContext.Current.Session["Authority"];
            if (o == null) throw new ExceptionSession("读取权限失败。");
            else return (o as ArrayList);
        }

        /// <summary>
        /// 移除模块权限
        /// </summary>
        public static void RemoveAuthority()
        {
            object o = System.Web.HttpContext.Current.Session["Authority"];
            if (o != null) System.Web.HttpContext.Current.Session.Remove("Authority");
        }

        #endregion
    }
}
