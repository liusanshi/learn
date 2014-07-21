using System;

namespace LL.Framework.Permission.DTO
{
    public class PermissionDesriptionDTO
    {
        private string m_Id;
        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return m_AppId; }
            set { m_AppId = value; }
        }

        private string m_Name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        private string m_Code;

        /// <summary>
        /// 编码
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


        private Int64 m_Value;

        /// <summary>
        /// 值
        /// </summary>
        public Int64 Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private string m_AppId;
        /// <summary>
        /// 应用Id
        /// </summary>
        public string AppId
        {
            get { return m_AppId; }
            set { m_AppId = value; }
        }
    }
}
