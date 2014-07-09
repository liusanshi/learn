using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;

using LL.FrameWork.Core.Infrastructure.IOC;
using System.Collections.Concurrent;

namespace LL.FrameWork.Web.MVC
{
    public class DefaultControllerFactory : IControllerFactory
    {
        private class DefaultControllerActivator : IControllerActivator
        {
            private Func<IDependencyResolver> _resolverThunk;
            public DefaultControllerActivator()
                : this(null)
            {
            }
            public DefaultControllerActivator(IDependencyResolver resolver)
            {
                if (resolver == null)
                {
                    this._resolverThunk = (() => DependencyResolver.Current);
                    return;
                }
                this._resolverThunk = (() => resolver);
            }
            public IController Create(RequestContext requestContext, Type controllerType)
            {
                IController result;
                try
                {
                    result = (IController)(this._resolverThunk().GetService(controllerType) ?? Activator.CreateInstance(controllerType));
                }
                catch (Exception innerException)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                        "在创建控制器：{0}的时候发生错误，请确保该控制器有一个公开的没有参数的构造函。", new object[]
					{
						controllerType
					}), innerException);
                }
                return result;
            }
        }

        private IResolver<IControllerActivator> _activatorResolver;
        private IControllerActivator _controllerActivator;
        private ControllerBuilder _controllerBuilder;
        private ControllerTypeCache _instanceControllerTypeCache;
        private static readonly ConcurrentDictionary<Type, SessionMode> _sessionStateCache = new ConcurrentDictionary<Type, SessionMode>();
        private static ControllerTypeCache _staticControllerTypeCache = new ControllerTypeCache();
        private IControllerActivator ControllerActivator
        {
            get
            {
                if (this._controllerActivator != null)
                {
                    return this._controllerActivator;
                }
                this._controllerActivator = this._activatorResolver.Current;
                return this._controllerActivator;
            }
        }
        internal ControllerBuilder ControllerBuilder
        {
            get
            {
                return this._controllerBuilder ?? ControllerBuilder.Current;
            }
            set
            {
                this._controllerBuilder = value;
            }
        }
        internal ControllerTypeCache ControllerTypeCache
        {
            get
            {
                return this._instanceControllerTypeCache ?? DefaultControllerFactory._staticControllerTypeCache;
            }
            set
            {
                this._instanceControllerTypeCache = value;
            }
        }
        public DefaultControllerFactory()
            : this(null, null, null)
        {
        }
        public DefaultControllerFactory(IControllerActivator controllerActivator)
            : this(controllerActivator, null, null)
        {
        }
        internal DefaultControllerFactory(IControllerActivator controllerActivator, IResolver<IControllerActivator> activatorResolver, IDependencyResolver dependencyResolver)
        {
            if (controllerActivator != null)
            {
                this._controllerActivator = controllerActivator;
                return;
            }
            if (activatorResolver == null)
            {
                activatorResolver = new SingleServiceResolver<IControllerActivator>(() => null, 
                    new DefaultControllerFactory.DefaultControllerActivator(dependencyResolver), "DefaultControllerFactory contstructor");
            }
            this._activatorResolver = activatorResolver;
        }
        internal static InvalidOperationException CreateAmbiguousControllerException(string url, string controllerName, ICollection<Type> matchingTypes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Type current in matchingTypes)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(current.FullName);
            }
            string message;
            if (!string.IsNullOrWhiteSpace(url))
            {
                message = string.Format(CultureInfo.CurrentCulture, "控制器：{0}匹配到多个类型。地址：{1}匹配到了类型：{2}，请确定唯一的类型", new object[]
				{
					controllerName,
					url,
					stringBuilder
				});
            }
            else
            {
                message = string.Format(CultureInfo.CurrentCulture, "控制器：{0}匹配到多个类型。匹配到了类型：{2}，请确定唯一的类型", new object[]
				{
					controllerName,
					stringBuilder
				});
            }
            return new InvalidOperationException(message);
        }
        public virtual IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException("值不能为null或者empty", "controllerName");
            }
            Type controllerType = this.GetControllerType(requestContext, controllerName);
            return this.GetControllerInstance(requestContext, controllerType);
        }
        protected internal virtual IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format(CultureInfo.CurrentCulture, "没有找到路径:{0}对应的控制器，或者器对应的控制器没有实现接口IController。", new object[]
				{
					requestContext.HttpRequest.Path
				}));
            }
            if (!typeof(IController).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "控制器：{0}必须实现接口IController。", new object[]
				{
					controllerType
				}), "controllerType");
            }
            return this.ControllerActivator.Create(requestContext, controllerType);
        }
        protected internal virtual SessionMode GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return SessionMode.Default;
            }
            return DefaultControllerFactory._sessionStateCache.GetOrAdd(controllerType, delegate(Type type)
            {
                bool inherit = true;
                SessionModeAttribute sessionStateAttribute = type.GetCustomAttributes(typeof(SessionModeAttribute), inherit).OfType<SessionModeAttribute>().FirstOrDefault<SessionModeAttribute>();
                if (sessionStateAttribute == null)
                {
                    return SessionMode.Default;
                }
                return sessionStateAttribute.SessionMode;
            });
        }
        protected internal virtual Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException("值不能为null或者empty", "controllerName");
            }
            object obj;
            if (requestContext != null && requestContext.RouteData.DataTokens.TryGetValue("Namespaces", out obj))
            {
                IEnumerable<string> enumerable = obj as IEnumerable<string>;
                if (enumerable != null && enumerable.Any<string>())
                {
                    HashSet<string> namespaces = new HashSet<string>(enumerable, StringComparer.OrdinalIgnoreCase);
                    Type controllerTypeWithinNamespaces = this.GetControllerTypeWithinNamespaces(requestContext.HttpRequest.Path, controllerName, namespaces);
                    if (controllerTypeWithinNamespaces != null || false.Equals(requestContext.RouteData.DataTokens["UseNamespaceFallback"]))
                    {
                        return controllerTypeWithinNamespaces;
                    }
                }
            }
            if (this.ControllerBuilder.DefaultNamespaces.Count > 0)
            {
                HashSet<string> namespaces2 = new HashSet<string>(this.ControllerBuilder.DefaultNamespaces, StringComparer.OrdinalIgnoreCase);
                Type controllerTypeWithinNamespaces = this.GetControllerTypeWithinNamespaces(requestContext.HttpRequest.Path, controllerName, namespaces2);
                if (controllerTypeWithinNamespaces != null)
                {
                    return controllerTypeWithinNamespaces;
                }
            }
            return this.GetControllerTypeWithinNamespaces(requestContext.HttpRequest.Path, controllerName, null);
        }
        private Type GetControllerTypeWithinNamespaces(string url, string controllerName, HashSet<string> namespaces)
        {
            this.ControllerTypeCache.EnsureInitialized();
            ICollection<Type> controllerTypes = this.ControllerTypeCache.GetControllerTypes(controllerName, namespaces);
            switch (controllerTypes.Count)
            {
                case 0:
                    return null;
                case 1:
                    return controllerTypes.First<Type>();
                default:
                    throw DefaultControllerFactory.CreateAmbiguousControllerException(url, controllerName, controllerTypes);
            }
        }
        public virtual void ReleaseController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        SessionMode IControllerFactory.GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("httpRequest");
            }
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException("值不能为null或者empty", "controllerName");
            }
            Type controllerType = this.GetControllerType(requestContext, controllerName);
            return this.GetControllerSessionBehavior(requestContext, controllerType);
        }
    }
}
