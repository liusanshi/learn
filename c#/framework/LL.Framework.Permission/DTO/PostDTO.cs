using System;

namespace LL.Framework.Permission.DTO
{
    /// <summary>
    /// 岗位的 DTO 对象
    /// </summary>
    public class PostDTO
    {
        /// <summary>
        /// 岗位的 DTO 对象构造函数
        /// </summary>
        public PostDTO()
        {
            ///Todo
        }

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
        /// 岗位名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        private string m_Code;

        /// <summary>
        /// 岗位编码
        /// </summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }


        private string m_Reamrk;

        /// <summary>
        /// 备注
        /// </summary>
        public string Reamrk
        {
            get { return m_Reamrk; }
            set { m_Reamrk = value; }
        }
    }
}
