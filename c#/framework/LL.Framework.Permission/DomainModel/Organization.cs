using System;
using LL.Framework.Core.Domain;

namespace LL.Framework.Permission.DomainModel
{
    /// <summary>
    /// 组织
    /// </summary>
    public class Organization : EntityBase<string>
    {

        /// <summary>
        /// 组织构造函数
        /// </summary>
        public Organization()
        {
            ///Todo
        }

        private string m_Name;

        /// <summary>
        /// 组织名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        private string m_Code;

        /// <summary>
        /// 组织编码
        /// </summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
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

        private Organization m_ParentOrganization;
        /// <summary>
        /// 父组织
        /// </summary>
        public virtual Organization ParentOrganization
        {
            get { return m_ParentOrganization; }
            set { m_ParentOrganization = value; }
        }
    }

}

