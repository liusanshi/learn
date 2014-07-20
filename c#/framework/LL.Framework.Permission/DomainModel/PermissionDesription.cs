using System;
using LL.Framework.Core.Domain;

namespace LL.Framework.Permission.DomainModel
{

    /// <summary>
    /// 权限描述信息
    /// </summary>
    public class PermissionDesription : EntityBase<string>
    {

        /// <summary>
        /// 权限描述信息构造函数
        /// </summary>
        public PermissionDesription()
        {
            ///Todo
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