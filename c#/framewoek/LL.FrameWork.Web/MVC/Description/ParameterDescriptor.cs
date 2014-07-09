using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LL.FrameWork.Web.MVC
{
    public class ParameterDescriptor : ICustomAttributeProvider
    {
        private readonly ActionDescriptor _actionDescriptor;
        private readonly ParameterBindingInfo _bindingInfo;
        public ParameterDescriptor(ParameterInfo parameterInfo, ActionDescriptor actionDescriptor)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException("parameterInfo");
            }
            if (actionDescriptor == null)
            {
                throw new ArgumentNullException("actionDescriptor");
            }
            this.ParameterInfo = parameterInfo;
            this._actionDescriptor = actionDescriptor;
            this._bindingInfo = new ParameterBindingInfo(parameterInfo);
        }
        private static readonly ParameterBindingInfo _emptyBindingInfo = new ParameterBindingInfo();
        public ActionDescriptor ActionDescriptor
        {
            get { return _actionDescriptor; }
        }
        public ParameterInfo ParameterInfo
        {
            get;
            protected set;
        }
        public virtual ParameterBindingInfo BindingInfo
        {
            get
            {
                return _bindingInfo;
            }
        }
        /// <summary>
        /// 默认值
        /// </summary>
        public virtual object DefaultValue
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        public virtual string ParameterName
        {
            get { return ParameterInfo.Name; }
        }
        /// <summary>
        /// 参数类型
        /// </summary>
        public virtual Type ParameterType
        {
            get { return ParameterInfo.ParameterType; }
        }

        #region ICustomAttributeProvider Member
        public object[] GetCustomAttributes(bool inherit)
        {
            return this.ParameterInfo.GetCustomAttributes(inherit);
        }
        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return this.ParameterInfo.GetCustomAttributes(attributeType, inherit);
        }
        public bool IsDefined(Type attributeType, bool inherit)
        {
            return this.ParameterInfo.IsDefined(attributeType, inherit);
        }
        #endregion

        /// <summary>
        /// 属性筛选器
        /// </summary>
        /// <param name="parameterDescriptor"></param>
        /// <returns></returns>
        internal static Predicate<string> GetPropertyFilter(ParameterDescriptor parameterDescriptor)
        {
            ParameterBindingInfo bindingInfo = parameterDescriptor.BindingInfo;
            return (string propertyName) => BindAttribute.IsPropertyAllowed(propertyName, bindingInfo.Include.ToArray<string>(), bindingInfo.Exclude.ToArray<string>());
        }
    }
}
