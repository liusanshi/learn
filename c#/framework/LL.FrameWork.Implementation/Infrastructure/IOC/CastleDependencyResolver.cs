using System;
using System.Linq;

using LL.Framework.Core.Infrastructure.IOC;

namespace LL.Framework.Impl.Infrastructure.IOC
{
    public class CastleDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            return Container.Current.Resolve(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.Current.ResolveAll(serviceType).Cast<object>();
        }

        public object GetService(string key, Type serviceType)
        {
            return Container.Current.Resolve(key, serviceType);
        }
    }
}
