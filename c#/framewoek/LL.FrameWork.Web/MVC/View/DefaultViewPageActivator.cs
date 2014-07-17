using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LL.Framework.Core.Infrastructure.IOC;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 默认的 页面创建者
    /// </summary>
    internal class DefaultViewPageActivator : IViewPageActivator
    {
        private Func<IDependencyResolver> _resolverThunk;
        public DefaultViewPageActivator()
            : this(null)
        {
        }
        public DefaultViewPageActivator(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                this._resolverThunk = (() => DependencyResolver.Current);
                return;
            }
            this._resolverThunk = (() => resolver);
        }
        public object Create(ControllerContext controllerContext, Type type)
        {
            return this._resolverThunk().GetService(type) ?? Activator.CreateInstance(type);
        }
    }
}
