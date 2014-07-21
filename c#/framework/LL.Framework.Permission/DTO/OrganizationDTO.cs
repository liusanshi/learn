using System;

namespace LL.Framework.Permission.DTO
{
    /// <summary>
    /// 组织的 DTO 对象
    /// </summary>
    public class OrganizationDTO
    {
        /// <summary>
        /// 组织的 DTO 对象构造函数
        /// </summary>
        public OrganizationDTO()
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

        private string m_POID;

        /// <summary>
        /// 父组织ID
        /// </summary>
        public string POID
        {
            get { return m_POID; }
            set { m_POID = value; }
        }
    }
}
