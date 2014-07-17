using System;
using System.Collections.Generic;
using System.Linq;

namespace LL.Framework.Core.Infrastructure.IOC
{
    /// <summary>
    /// 解耦之一次返回多个对象
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class MultiServiceResolver<TService> : IResolver<IEnumerable<TService>> where TService : class
    {
        private IEnumerable<TService> _itemsFromService;
        private Func<IEnumerable<TService>> _itemsThunk;
        private Func<IDependencyResolver> _resolverThunk;
        public IEnumerable<TService> Current
        {
            get
            {
                if (this._itemsFromService == null)
                {
                    lock (this._itemsThunk)
                    {
                        if (this._itemsFromService == null)
                        {
                            this._itemsFromService = this._resolverThunk().GetServices<TService>();
                        }
                    }
                }
                return this._itemsFromService.Concat(this._itemsThunk());
            }
        }
        public MultiServiceResolver(Func<IEnumerable<TService>> itemsThunk)
        {
            if (itemsThunk == null)
            {
                throw new ArgumentNullException("itemsThunk");
            }
            this._itemsThunk = itemsThunk;
            this._resolverThunk = (() => DependencyResolver.Current);
        }

        public MultiServiceResolver(Func<IEnumerable<TService>> itemsThunk, IDependencyResolver resolver)
            : this(itemsThunk)
        {
            if (resolver != null)
            {
                this._resolverThunk = (() => resolver);
            }
        }
    }
}
