using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

using LL.FrameWork.Core.Reflection;
using LL.FrameWork.Web.MVC.Serializer;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 动作的描述类
    /// </summary>
    public class ActionDescriptor : ICustomAttributeProvider, IUniquelyIdentifiable
    {
        private readonly string _actionName;
        private readonly ControllerDescriptor _controllerDescriptor;
        private readonly Lazy<string> _uniqueId;
        private ParameterDescriptor[] _parametersCache;

        public ControllerDescriptor PageController; //为PageAction保留
        public MethodInfo MethodInfo { get; private set; }
        //public ActionAttribute Attr { get; private set; }
        public bool HasReturn { get; private set; }
        /// <summary>
        /// 对应的控制器
        /// </summary>
        public ControllerDescriptor ControllerDescriptor { get { return _controllerDescriptor; } }
        /// <summary>
        /// 动作的名称
        /// </summary>
        private string ActionName { get { return _actionName; } }
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string UniqueId
        {
            get { return _uniqueId.Value; }
        }
        /// <summary>
        /// 创建一个新的 ActionDescriptor
        /// </summary>
        /// <param name="m"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerdesp"></param>
        public ActionDescriptor(MethodInfo m, string actionName, ControllerDescriptor controllerdesp)
        {
            this.MethodInfo = m;
            //this.Attr = m.GetAttribute<ActionAttribute>();
            this.HasReturn = m.ReturnType != ReflectionHelper.VoidType;

            _uniqueId = new Lazy<string>(CreateUniqueId);
            _actionName = actionName;
            _controllerDescriptor = controllerdesp;
        }

        /// <summary>
        /// 动作的执行
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        public virtual object Execute(ControllerContext controllerContext, IDictionary<string, object> args)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            // 准备要传给调用方法的参数
            object[] parameters = null;
            if (args != null)
            {
                parameters = GetParameters().Select(p =>
                {
                    object val;
                    if (!args.TryGetValue(p.ParameterName, out val))
                    {
                        val = p.DefaultValue;
                    }
                    return val;
                }).ToArray();
            }

            // 调用方法
            if (this.HasReturn)
            {
                return MethodInfo.FastInvoke(controllerContext.Controller, parameters);
            }
            else
            {
                MethodInfo.FastInvoke(controllerContext.Controller, parameters);
                return null;
            }
        }

        private string CreateUniqueId()
        {
            return DescriptorUtil.CreateUniqueId(new object[]
	        {
		        GetType(),
		        this.ControllerDescriptor,
		        this.ActionName
	        });
        }

        internal IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
        {
            if (useCache)
            {
                return ReflectedAttributeCache.GetMethodFilterAttributes(this.MethodInfo);
            }
            return this.GetCustomAttributes(typeof(FilterAttribute), true).Cast<FilterAttribute>();
        }
        /// <summary>
        /// 获取参数描述
        /// </summary>
        /// <returns></returns>
        public ParameterDescriptor[] GetParameters()
        {
            ParameterDescriptor[] array = this.LazilyFetchParametersCollection();
            return (ParameterDescriptor[])array.Clone();
        }

        private ParameterDescriptor[] LazilyFetchParametersCollection()
        {
            return DescriptorUtil.LazilyFetchOrCreateDescriptors<ParameterInfo, ParameterDescriptor>(ref this._parametersCache, 
                new Func<ParameterInfo[]>(this.MethodInfo.GetParameters), 
                (ParameterInfo parameterInfo) => new ParameterDescriptor(parameterInfo, this));
        }

        #region ICustomAttributeProvider member

        public object[] GetCustomAttributes(bool inherit)
        {
            return MethodInfo.GetCustomAttributes(inherit);
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return MethodInfo.GetCustomAttributes(attributeType, inherit);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return MethodInfo.IsDefined(attributeType, inherit);
        }
        #endregion
    }
}
