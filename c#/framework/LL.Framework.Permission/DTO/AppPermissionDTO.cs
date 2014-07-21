using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Permission.DTO
{
    public class AppPermissionDTO
    {
        private string m_Id;
        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
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
    }
}
