using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 参数的绑定信息
    /// </summary>
    public class ParameterBindingInfo
    {
        private ICollection<string> _exclude = new string[0];
        private ICollection<string> _include = new string[0];
        private readonly ParameterInfo _parameterInfo;
        private string _prefix;
        /// <summary>
        /// 排除字段
        /// </summary>
        public ICollection<string> Exclude
        {
            get
            {
                return this._exclude;
            }
        }
        /// <summary>
        /// 包含字段
        /// </summary>
        public ICollection<string> Include
        {
            get
            {
                return this._include;
            }
        }
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix
        {
            get
            {
                return this._prefix;
            }
        }


        internal ParameterBindingInfo() { }

        public ParameterBindingInfo(ParameterInfo parameterInfo)
        {
            this._parameterInfo = parameterInfo;
            this.ReadSettingsFromBindAttribute();
        }
        private void ReadSettingsFromBindAttribute()
        {
            BindAttribute bindAttribute = (BindAttribute)Attribute.GetCustomAttribute(this._parameterInfo, typeof(BindAttribute));
            if (bindAttribute == null)
            {
                return;
            }
            this._exclude = new ReadOnlyCollection<string>(AuthorizeAttribute.SplitString(bindAttribute.Exclude));
            this._include = new ReadOnlyCollection<string>(AuthorizeAttribute.SplitString(bindAttribute.Include));
            this._prefix = bindAttribute.Prefix;
        }
    }
}
