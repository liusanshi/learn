using System;
using System.Collections.Generic;
using System.Globalization;

using LL.FrameWork.Core.Infrastructure.IOC;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 控制器的生成器
    /// </summary>
    public class ControllerBuilder
    {
        private Func<IControllerFactory> _factoryThunk = () => null;
        private static ControllerBuilder _instance = new ControllerBuilder();
        private HashSet<string> _namespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private IResolver<IControllerFactory> _serviceResolver;
        public static ControllerBuilder Current
        {
            get
            {
                return ControllerBuilder._instance;
            }
        }
        /// <summary>
        /// 默认的命名空间
        /// </summary>
        public HashSet<string> DefaultNamespaces
        {
            get
            {
                return this._namespaces;
            }
        }
        public ControllerBuilder()
            : this(null)
        {
        }
        internal ControllerBuilder(IResolver<IControllerFactory> serviceResolver)
        {
            if (serviceResolver == null)
            {
                serviceResolver = new SingleServiceResolver<IControllerFactory>(() => this._factoryThunk(),
                    new DefaultControllerFactory() { ControllerBuilder = this },
                    "ControllerBuilder.GetControllerFactory");
            }
            this._serviceResolver = serviceResolver;
        }
        public IControllerFactory GetControllerFactory()
        {
            return this._serviceResolver.Current;
        }
        public void SetControllerFactory(IControllerFactory controllerFactory)
        {
            if (controllerFactory == null)
            {
                throw new ArgumentNullException("controllerFactory");
            }
            this._factoryThunk = (() => controllerFactory);
        }
        public void SetControllerFactory(Type controllerFactoryType)
        {
            if (controllerFactoryType == null)
            {
                throw new ArgumentNullException("controllerFactoryType");
            }
            if (!typeof(IControllerFactory).IsAssignableFrom(controllerFactoryType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "控制器工厂:{0}必须实现接口IControllerFactory。", new object[]
				{
					controllerFactoryType
				}), "controllerFactoryType");
            }
            this._factoryThunk = () =>
            {
                IControllerFactory result;
                try
                {
                    result = (IControllerFactory)Activator.CreateInstance(controllerFactoryType);
                }
                catch (Exception innerException)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "创建控制器工厂实例时发生错误，请确保该类有一个公开的没有参数的构造函数", new object[]
					{
						controllerFactoryType
					}), innerException);
                }
                return result;
            };
        }
    }
}
