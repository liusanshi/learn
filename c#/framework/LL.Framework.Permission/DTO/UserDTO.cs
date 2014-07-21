using System;
using System.Collections.Generic;

namespace LL.Framework.Permission.DTO
{
    /// <summary>
    /// 用户的 DTO 对象
    /// </summary>
    public class UserDTO
    {
        #region 属性
        private string m_Id;
        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        private string m_CName;

        /// <summary>
        /// 中文名称
        /// </summary>
        public string CName
        {
            get { return m_CName; }
            set { m_CName = value; }
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


        private string m_EName;

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EName
        {
            get { return m_EName; }
            set { m_EName = value; }
        }

        private int m_Status;

        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }


        private int m_Type;

        /// <summary>
        /// 用户类型
        /// </summary>
        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private string m_Email;
        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
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


        private string m_IDCard;

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard
        {
            get { return m_IDCard; }
            set { m_IDCard = value; }
        }


        private int m_Sex;

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex
        {
            get { return m_Sex; }
            set { m_Sex = value; }
        }


        private DateTime m_BirthDay;

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDay
        {
            get { return m_BirthDay; }
            set { m_BirthDay = value; }
        }


        private string m_MobileNo;

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileNo
        {
            get { return m_MobileNo; }
            set { m_MobileNo = value; }
        }


        private DateTime m_WorkStartDate;

        /// <summary>
        /// 到职日期
        /// </summary>
        public DateTime WorkStartDate
        {
            get { return m_WorkStartDate; }
            set { m_WorkStartDate = value; }
        }


        private DateTime m_WorkEndDate;

        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime WorkEndDate
        {
            get { return m_WorkEndDate; }
            set { m_WorkEndDate = value; }
        }


        private string m_CompanyMail;

        /// <summary>
        /// 公司邮件地址
        /// </summary>
        public string CompanyMail
        {
            get { return m_CompanyMail; }
            set { m_CompanyMail = value; }
        }


        private string m_Extension;

        /// <summary>
        /// 分机号
        /// </summary>
        public string Extension
        {
            get { return m_Extension; }
            set { m_Extension = value; }
        }


        private string m_HomeTel;

        /// <summary>
        /// 家中电话
        /// </summary>
        public string HomeTel
        {
            get { return m_HomeTel; }
            set { m_HomeTel = value; }
        }


        private DateTime m_OperateDateTime;

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateDateTime
        {
            get { return m_OperateDateTime; }
            set { m_OperateDateTime = value; }
        }


        private string m_LastIP;

        /// <summary>
        /// 最后访问IP
        /// </summary>
        public string LastIP
        {
            get { return m_LastIP; }
            set { m_LastIP = value; }
        }


        private DateTime m_LastDateTime;

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastDateTime
        {
            get { return m_LastDateTime; }
            set { m_LastDateTime = value; }
        }


        private string m_ExtendField;

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string ExtendField
        {
            get { return m_ExtendField; }
            set { m_ExtendField = value; }
        }


        private string m_Mac;

        /// <summary>
        /// 锁定机器硬件地址
        /// </summary>
        public string Mac
        {
            get { return m_Mac; }
            set { m_Mac = value; }
        }


        private string m_PhotoUrl;

        /// <summary>
        /// 用户照片网址
        /// </summary>
        public string PhotoUrl
        {
            get { return m_PhotoUrl; }
            set { m_PhotoUrl = value; }
        }


        private string m_SignaturePUrl;

        /// <summary>
        /// 签名图片网址
        /// </summary>
        public string SignaturePUrl
        {
            get { return m_SignaturePUrl; }
            set { m_SignaturePUrl = value; }
        }


        private int m_LoginCount;

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount
        {
            get { return m_LoginCount; }
            set { m_LoginCount = value; }
        }

        /// <summary>
        /// 所属组织Id
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// 所属组织Name
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// 所属组织Code
        /// </summary>
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 所在职位Id
        /// </summary>
        public string PostId { get; set; }
        /// <summary>
        /// 所在职位Name
        /// </summary>
        public string PostName { get; set; }
        /// <summary>
        /// 所在职位Code
        /// </summary>
        public string PostCode { get; set; }
        #endregion
    }
}
