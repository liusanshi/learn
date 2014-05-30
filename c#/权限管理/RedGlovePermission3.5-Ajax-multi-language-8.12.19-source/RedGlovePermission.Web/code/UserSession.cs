/**************************************
* 作用：系统Session处理
* 作者：Nick.Yan
* 日期: 2008-02-11
* 网址：www.redglove.com.cn
**************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedGlovePermission.Web
{
    /// <summary>
    /// 储存在Session中的已登录会员的基本信息。
    /// </summary>
    [Serializable]
    public class UserSession
    {
        /// <summary>
        /// 初始化用户登录Session
        /// </summary>
        /// <param name="_loginId">用户ID</param>
        /// <param name="_loginname">用户名</param>
        /// <param name="_roleid">角色ID</param>
        /// <param name="_islimit">是否受权限限制</param>
        /// <param name="_status">用户状态</param>
        public UserSession(int _loginId, string _loginname, int _roleid, bool _islimit, int _status)
        {
            this.LoginId = _loginId;
            this.LoginName = _loginname;
            this.RoleID = _roleid;
            this.IsLimit = _islimit;
            this.Status = _status;
        }

        public int LoginId;
        public string LoginName;
        public int RoleID;
        public bool IsLimit;
        public int Status;
    }
}
