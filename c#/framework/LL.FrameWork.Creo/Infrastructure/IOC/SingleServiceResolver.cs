using System;
using System.Globalization;

namespace LL.FrameWork.Core.Infrastructure.IOC
{
    /// <summary>
    ///  解耦之一次返回一个对象
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class SingleServiceResolver<TService> : IResolver<TService> where TService : class
    {
        private TService _currentValueFromResolver;
        private Func<TService> _currentValueThunk;
        private TService _defaultValue;
        private Func<IDependencyResolver> _resolverThunk;
        private string _callerMethodName;
        public TService Current
        {
            get
            {
                if (this._resolverThunk != null)
                {
                    lock (this._currentValueThunk)
                    {
                        if (this._resolverThunk != null)
                        {
                            this._currentValueFromResolver = this._resolverThunk().GetService<TService>();
                            this._resolverThunk = null;
                            if (this._currentValueFromResolver != null && this._currentValueThunk() != null)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "在容器的所有在注册的服务中没有找到该实例{0}，请检查{1}", new object[]
								{
									typeof(TService).Name.ToString(),
									this._callerMethodName
								}));
                            }
                        }
                    }
                }
                TService result;
                if ((result = this._currentValueFromResolver) == null && (result = this._currentValueThunk()) == null)
                {
                    result = this._defaultValue;
                }
                return result;
            }
        }
        public SingleServiceResolver(Func<TService> currentValueThunk, TService defaultValue, string callerMethodName)
        {
            if (currentValueThunk == null)
            {
                throw new ArgumentNullException("currentValueThunk");
            }
            if (defaultValue == null)
            {
                throw new ArgumentNullException("defaultValue");
            }
            this._resolverThunk = (() => DependencyResolver.Current);
            this._currentValueThunk = currentValueThunk;
            this._defaultValue = defaultValue;
            this._callerMethodName = callerMethodName;
        }
        public SingleServiceResolver(Func<TService> staticAccessor, TService defaultValue, IDependencyResolver resolver, string callerMethodName)
            : this(staticAccessor, defaultValue, callerMethodName)
        {
            if (resolver != null)
            {
                this._resolverThunk = (() => resolver);
            }
        }
    }
}
