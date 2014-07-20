using System;
using LL.Framework.Core.Domain;

namespace LL.Framework.Permission.DomainModel
{

    /// <summary>
    /// 角色
    /// </summary>
    public class Role : EntityBase<string>
    {

        /// <summary>
        /// 角色构造函数
        /// </summary>
        public Role()
        {
            ///Todo
        }

        private string m_Name;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        private string m_Code;

        /// <summary>
        /// 名称
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