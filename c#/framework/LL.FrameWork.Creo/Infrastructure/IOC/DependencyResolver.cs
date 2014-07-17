using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace LL.FrameWork.Core.Infrastructure.IOC
{
    /// <summary>
    /// 解耦依赖
    /// </summary>
    public class DependencyResolver
    {
        /// <summary>
        /// 利用委托的解耦依赖
        /// </summary>
        private class DelegateBasedDependencyResolver : IDependencyResolver
        {
            private Func<Type, object> _getService;
            private Func<Type, IEnumerable<object>> _getServices;
            public DelegateBasedDependencyResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
            {
                this._getService = getService;
                this._getServices = getServices;
            }
            public object GetService(Type type)
            {
                object result;
                try
                {
                    result = this._getService(type);
                }
                catch
                {
                    result = null;
                }
                return result;
            }
            public IEnumerable<object> GetServices(Type type)
            {
                return this._getServices(type);
            }

            public object GetService(string key, Type serviceType)
            {
                return GetService(serviceType);
            }
        }
        /// <summary>
        /// 默认的使用反射来解耦依赖
        /// </summary>
        private class DefaultDependencyResolver : IDependencyResolver
        {
            public object GetService(Type serviceType)
            {
                object result;
                try
                {
                    result = Activator.CreateInstance(serviceType);
                }
                catch
                {
                    result = null;
                }
                return result;
            }
            public IEnumerable<object> GetServices(Type serviceType)
            {
                return Enumerable.Empty<object>();
            }

            public object GetService(string key, Type serviceType)
            {
                return GetService(serviceType);
            }
        }
        private static DependencyResolver _instance = new DependencyResolver();
        private IDependencyResolver _current = new DependencyResolver.DefaultDependencyResolver();
        public static IDependencyResolver Current
        {
            get
            {
                return DependencyResolver._instance.InnerCurrent;
            }
        }
        public IDependencyResolver InnerCurrent
        {
            get
            {
                return this._current;
            }
        }
        /// <summary>
        /// 设置解耦依赖方式
        /// </summary>
        /// <param name="resolver"></param>
        public static void SetResolver(IDependencyResolver resolver)
        {
            DependencyResolver._instance.InnerSetResolver(resolver);
        }
        /// <summary>
        /// 设置解耦依赖方式
        /// </summary>
        /// <param name="commonServiceLocator"></param>
        public static void SetResolver(object commonServiceLocator)
        {
            DependencyResolver._instance.InnerSetResolver(commonServiceLocator);
        }
        /// <summary>
        /// 使用委托来设置解耦方式
        /// </summary>
        /// <param name="getService"></param>
        /// <param name="getServices"></param>
        public static void SetResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
        {
            DependencyResolver._instance.InnerSetResolver(getService, getServices);
        }
        /// <summary>
        /// 设置内部的解耦方式
        /// </summary>
        /// <param name="resolver"></param>
        public void InnerSetResolver(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }
            this._current = resolver;
        }
        /// <summary>
        /// 设置内部的解耦方式
        /// </summary>
        /// <param name="commonServiceLocator"></param>
        public void InnerSetResolver(object commonServiceLocator)
        {
            if (commonServiceLocator == null)
            {
                throw new ArgumentNullException("commonServiceLocator");
            }
            Type type = commonServiceLocator.GetType();
            MethodInfo method = type.GetMethod("GetInstance", new Type[]
			{
				typeof(Type)
			});
            MethodInfo method2 = type.GetMethod("GetAllInstances", new Type[]
			{
				typeof(Type)
			});
            if (method == null || method.ReturnType != typeof(object) || method2 == null || method2.ReturnType != typeof(IEnumerable<object>))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "类型：{0}没有实现接口[Microsoft.Practices.ServiceLocation.IServiceLocator]。", new object[]
				{
					type.FullName
				}), "commonServiceLocator");
            }
            Func<Type, object> getService = (Func<Type, object>)Delegate.CreateDelegate(typeof(Func<Type, object>), commonServiceLocator, method);
            Func<Type, IEnumerable<object>> getServices = (Func<Type, IEnumerable<object>>)Delegate.CreateDelegate(typeof(Func<Type, IEnumerable<object>>), commonServiceLocator, method2);
            this._current = new DependencyResolver.DelegateBasedDependencyResolver(getService, getServices);
        }
        /// <summary>
        /// 使用委托来设置内部的解耦方式
        /// </summary>
        /// <param name="getService"></param>
        /// <param name="getServices"></param>
        public void InnerSetResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
        {
            if (getService == null)
            {
                throw new ArgumentNullException("getService");
            }
            if (getServices == null)
            {
                throw new ArgumentNullException("getServices");
            }
            this._current = new DependencyResolver.DelegateBasedDependencyResolver(getService, getServices);
        }
    }
}
