using System;

using Castle.Windsor;
using Castle.MicroKernel.Registration;
using LL.Framework.Core.Infrastructure.Logging;
using LL.Framework.Core.Infrastructure.Adapter;
using LL.Framework.Impl.Infrastructure.Logging.Log4NetImpl;
using LL.Framework.Impl.Infrastructure.Logging.TraceSourceImpl;
using LL.Framework.Core.Infrastructure.Validator;
using LL.Framework.Impl.Infrastructure.Validator;
using LL.Framework.Impl.Infrastructure.Adapter.EmitMapperImpl;
using LL.Framework.Impl.Infrastructure.Adapter.AutoMapperImpl;

namespace LL.Framework.Impl.Infrastructure.IOC
{
    /// <summary>
    /// Container
    /// </summary>
    public static class Container
    {
        #region Members

        static IWindsorContainer _currentContainer = null;

        #endregion

        #region Constructor

        static Container()
        {
            _currentContainer = new WindsorContainer();

            _currentContainer.Register(
                Component.For<ILoggerFactory>().ImplementedBy<Log4NetLogFactory>().Named("log4net").IsDefault(),
                Component.For<ILoggerFactory>().ImplementedBy<TraceSourceLogFactory>().Named("tracesourcelog"),
                Component.For<ITypeAdapterFactory>().ImplementedBy<EmitMapperTypeAdapterFactory>().Named("EmitMapper"),
                Component.For<ITypeAdapterFactory>().ImplementedBy<AutoMapperTypeAdapterFactory>().Named("AutoMapper").IsDefault());

            var loggerFactory = _currentContainer.Resolve<ILoggerFactory>();
            LoggerFactory.SetCurrent(loggerFactory);
                        
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());

            var typeAdapterFactory = _currentContainer.Resolve<ITypeAdapterFactory>();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);
        }

        #endregion

        #region property

        /// <summary>
        /// Get current container
        /// </summary>
        public static IWindsorContainer Current
        {
            get { return _currentContainer; }
        }

        #endregion
    }
}
