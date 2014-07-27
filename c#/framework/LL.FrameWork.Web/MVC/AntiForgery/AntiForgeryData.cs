using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web.Security;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 防伪验证的数据
    /// </summary>
    [Serializable]
    internal sealed class AntiForgeryData : ISerializable
    {
        #region 变量书属性
        /// <summary>
        /// 防伪票据的名称
        /// </summary>
        private const string AntiForgeryTokenFieldName = "__RequestVerificationToken";
        /// <summary>
        /// 防伪票据的长度
        /// </summary>
        private const int TokenLength = 16;
        private static readonly RNGCryptoServiceProvider _prng = new RNGCryptoServiceProvider();
        private DateTime _creationDate = DateTime.UtcNow;
        private string _salt;
        private string _username;
        private string _value;

        /// <summary>
        /// 创建的时间
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return this._creationDate;
            }
            set
            {
                this._creationDate = value;
            }
        }
        /// <summary>
        /// 添加的用于混淆的字符串(俗称加盐)
        /// </summary>
        public string Salt
        {
            get
            {
                return this._salt ?? string.Empty;
            }
            set
            {
                this._salt = value;
            }
        }
        /// <summary>
        /// 当前的用户名
        /// </summary>
        public string Username
        {
            get
            {
                return this._username ?? string.Empty;
            }
            set
            {
                this._username = value;
            }
        }
        /// <summary>
        /// 验证的值
        /// </summary>
        public string Value
        {
            get
            {
                return this._value ?? string.Empty;
            }
            set
            {
                this._value = value;
            }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 创建 AntiForgeryData 对象
        /// </summary>
        public AntiForgeryData()
        {
        }
        /// <summary>
        /// 创建 AntiForgeryData 对象
        /// </summary>
        /// <param name="token"></param>
        public AntiForgeryData(AntiForgeryData token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            this.CreationDate = token.CreationDate;
            this.Salt = token.Salt;
            this.Username = token.Username;
            this.Value = token.Value;
        }
        /// <summary>
        /// 创建 AntiForgeryData 对象
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public AntiForgeryData(SerializationInfo info, StreamingContext context)
        {
            Salt = info.GetString("Salt");
            Value = info.GetString("Value");
            Username = info.GetString("Username");
            CreationDate = info.GetDateTime("Username");
        }
        /// <summary>
        /// 创建 AntiForgeryData 对象
        /// </summary>
        /// <param name="ticket"></param>
        public AntiForgeryData(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException("ticket");
            }
            Salt = ticket.CookiePath;
            Value = ticket.UserData;
            Username = ticket.Name;
            CreationDate = ticket.IssueDate;
        }
        #endregion

        /// <summary>
        /// 将Cookie的名称转为Base64编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string Base64EncodeForCookieName(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            string text = Convert.ToBase64String(bytes);
            return text.Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }
        /// <summary>
        /// 生成随机的票据字符串
        /// </summary>
        /// <returns></returns>
        private static string GenerateRandomTokenString()
        {
            byte[] array = new byte[16];
            AntiForgeryData._prng.GetBytes(array);
            return Convert.ToBase64String(array);
        }
        /// <summary>
        /// 获取防伪票据的名称
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        internal static string GetAntiForgeryTokenName(string appPath)
        {
            if (string.IsNullOrEmpty(appPath))
            {
                return "__RequestVerificationToken";
            }
            return "__RequestVerificationToken_" + AntiForgeryData.Base64EncodeForCookieName(appPath);
        }
        /// <summary>
        /// 获取当前的用户名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static string GetUsername(IPrincipal user)
        {
            if (user != null)
            {
                IIdentity identity = user.Identity;
                if (identity != null && identity.IsAuthenticated)
                {
                    return identity.Name;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取新的防伪票据
        /// </summary>
        /// <returns></returns>
        public static AntiForgeryData NewToken()
        {
            return new AntiForgeryData
            {
                Value = AntiForgeryData.GenerateRandomTokenString()
            };
        }

        /// <summary>
        /// 将防伪数据转变为 FormsAuthenticationTicket
        /// </summary>
        /// <returns></returns>
        internal FormsAuthenticationTicket ConvertToFormsTicket()
        {
            return new FormsAuthenticationTicket(2,
                this.Username,
                this.CreationDate,
                this.CreationDate, true, this.Value, this.Salt);
        }

        /// <summary>
        /// 系列化是使用
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Salt", Salt);
            info.AddValue("Value", Value);
            info.AddValue("Username", Username);
            info.AddValue("Username", CreationDate); 
        }
    }
}
