using System;


namespace LL.Framework.Permission.DTO
{
    /// <summary>
    /// APP UI使用
    /// </summary>
    public class AppDTO
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

        private string m_Name;

        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        private string m_Code;

        /// <summary>
        /// 应用编码
        /// </summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        private string m_AppUrl;

        /// <summary>
        /// 应用地址
        /// </summary>
        public string AppUrl
        {
            get { return m_AppUrl; }
            set { m_AppUrl = value; }
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
    }
}
