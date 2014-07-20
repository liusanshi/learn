using System;
using LL.Framework.Core.Domain;

namespace LL.Framework.Permission.DomainModel
{
    /// <summary>
    /// 在线用户
    /// </summary>
    public class OnlineUser : EntityBase<string>
    {

        /// <summary>
        /// 在线用户构造函数
        /// </summary>
        public OnlineUser()
        {
            ///Todo
        }

        private string m_SessionID;

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionID
        {
            get { return m_SessionID; }
            set { m_SessionID = value; }
        }


        private string m_UserName;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }


        private string m_Ip;

        /// <summary>
        /// 用户IP地址
        /// </summary>
        public string Ip
        {
            get { return m_Ip; }
            set { m_Ip = value; }
        }


        private DateTime m_LoginTime;

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get { return m_LoginTime; }
            set { m_LoginTime = value; }
        }


        private DateTime m_LastTime;

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastTime
        {
            get { return m_LastTime; }
            set { m_LastTime = value; }
        }


        private string m_UserID;

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

    }

}