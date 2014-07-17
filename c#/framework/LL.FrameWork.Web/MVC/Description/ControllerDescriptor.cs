using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 对控制器的描述类
    /// </summary>
    public class ControllerDescriptor : ICustomAttributeProvider, IUniquelyIdentifiable
    {
        private readonly Lazy<string> _uniqueId;
        private readonly ActionMethodSelector _selector;

        /// <summary>
        /// 控制器的类型
        /// </summary>
        public Type ControllerType { get; private set; }
        /// <summary>
        /// 控制器的名称
        /// </summary>
        public virtual string ControllerName
        {
            get
            {
                string name = this.ControllerType.Name;
                if (name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                {
                    return name.Substring(0, name.Length - "Controller".Length);
                }
                return name;
            }
        }
        /// <summary>
        /// 创建一个新的 ControllerDescriptor
        /// </summary>
        /// <param name="t"></param>
        public ControllerDescriptor(Type t)
        {
            this.ControllerType = t;
            _uniqueId = new Lazy<string>(CreateUniqueId);
            this._selector = new ActionMethodSelector(this.ControllerType);
        }
        /// <summary>
        /// 控制器的唯一标识
        /// </summary>
        public string UniqueId
        {
            get
            {
                return _uniqueId.Value;
            }
        }
        private string CreateUniqueId()
        {
            return DescriptorUtil.CreateUniqueId(new object[]
			{
				GetType(),
				this.ControllerName,
				this.ControllerType
			});
        }
        /// <summary>
        /// 查找控制器中的动作的描述
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public ActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentException("动作名称不能为空", "actionName");
            }
            MethodInfo methodInfo = this._selector.FindActionMethod(controllerContext, actionName);
            if (methodInfo == null)
            {
                return null;
            }
            return new ActionDescriptor(methodInfo, actionName, this);
        }

        internal IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
        {
            if (useCache)
            {
                return ReflectedAttributeCache.GetTypeFilterAttributes(this.ControllerType);
            }
            return this.GetCustomAttributes(typeof(FilterAttribute), true).Cast<FilterAttribute>();
        }

        #region ICustomAttributeProvider member

        public object[] GetCustomAttributes(bool inherit)
        {
            return ControllerType.GetCustomAttributes(inherit);
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return ControllerType.GetCustomAttributes(attributeType, inherit);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return ControllerType.IsDefined(attributeType, inherit);
        }
        #endregion
    }
}
