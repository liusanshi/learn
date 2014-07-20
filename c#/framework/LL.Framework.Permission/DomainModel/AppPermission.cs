using System;
using LL.Framework.Core.Domain;

namespace LL.Framework.Permission.DomainModel
{

    /// <summary>
    /// 应用权限
    /// </summary>
    public class AppPermission : EntityBase<string>
    {
        /// <summary>
        /// 应用权限构造函数
        /// </summary>
        public AppPermission()
        {
            ///Todo
        }

        private string m_LicenseeID;

        /// <summary>
        /// 被授权人
        /// </summary>
        public string LicenseeID
        {
            get { return m_LicenseeID; }
            set { m_LicenseeID = value; }
        }


        private string m_Remark;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }


        private int m_LicenseeType;

        /// <summary>
        /// 被授权人类型
        /// </summary>
        public int LicenseeType
        {
            get { return m_LicenseeType; }
            set { m_LicenseeType = value; }
        }


        private Int64 m_PValue;

        /// <summary>
        /// 权限值
        /// </summary>
        public Int64 PValue
        {
            get { return m_PValue; }
            set { m_PValue = value; }
        }

        private App m_App;
        /// <summary>
        /// 权限对应的应用
        /// </summary>
        public virtual App App
        {
            get { return m_App; }
            set { m_App = value; }
        }
    }

}

