using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 描述参数的绑定信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class BindAttribute : Attribute
    {
        private string _exclude;
        private string[] _excludeSplit = new string[0];
        private string _include;
        private string[] _includeSplit = new string[0];
        public string Exclude
        {
            get
            {
                return this._exclude ?? string.Empty;
            }
            set
            {
                this._exclude = value;
                this._excludeSplit = AuthorizeAttribute.SplitString(value);
            }
        }
        public string Include
        {
            get
            {
                return this._include ?? string.Empty;
            }
            set
            {
                this._include = value;
                this._includeSplit = AuthorizeAttribute.SplitString(value);
            }
        }
        public string Prefix
        {
            get;
            set;
        }
        internal static bool IsPropertyAllowed(string propertyName, string[] includeProperties, string[] excludeProperties)
        {
            bool flag = includeProperties == null || includeProperties.Length == 0 || includeProperties.Contains(propertyName, StringComparer.OrdinalIgnoreCase);
            bool flag2 = excludeProperties != null && excludeProperties.Contains(propertyName, StringComparer.OrdinalIgnoreCase);
            return flag && !flag2;
        }
        public bool IsPropertyAllowed(string propertyName)
        {
            return BindAttribute.IsPropertyAllowed(propertyName, this._includeSplit, this._excludeSplit);
        }
    }
}
