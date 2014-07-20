using System;
using LL.Framework.Core.Domain;

namespace LL.Framework.Permission.DomainModel
{

    /// <summary>
    /// 应用
    /// </summary>
    public class App : EntityBase<string>
    {

        /// <summary>
        /// 应用构造函数
        /// </summary>
        public App()
        {
            ///Todo
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

        private App m_ParentApp;
        /// <summary>
        /// 应用的父应用
        /// </summary>
        public virtual App ParentApp
        {
            get { return m_ParentApp; }
            set { m_ParentApp = value; }
        }

        //private string m_PAppId;

        ///// <summary>
        ///// 父应用ID
        ///// </summary>
        //public string PAppId
        //{
        //    get { return m_PAppId; }
        //    set { m_PAppId = value; }
        //}

    }

}