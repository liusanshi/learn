using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using LL.FrameWork.Core.Infrastructure.IOC;

namespace LL.FrameWork.Web.MVC
{
    public class FilterProviderCollection : Collection<IFilterProvider>
    {
        private class FilterComparer : IComparer<Filter>
        {
            public int Compare(Filter x, Filter y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                if (x.Order < y.Order)
                {
                    return -1;
                }
                if (x.Order > y.Order)
                {
                    return 1;
                }
                if (x.Scope < y.Scope)
                {
                    return -1;
                }
                if (x.Scope > y.Scope)
                {
                    return 1;
                }
                return 0;
            }
        }
        private static FilterProviderCollection.FilterComparer _filterComparer = new FilterProviderCollection.FilterComparer();
        private IResolver<IEnumerable<IFilterProvider>> _serviceResolver;
        private IResolver<IEnumerable<IFilterProvider>> CombinedItems
        {
            get
            {
                return this._serviceResolver;
            }
        }
        public FilterProviderCollection()
        {
            this._serviceResolver = new MultiServiceResolver<IFilterProvider>(() => base.Items);
        }
        public FilterProviderCollection(IList<IFilterProvider> providers)
            : base(providers)
        {
            this._serviceResolver = new MultiServiceResolver<IFilterProvider>(() => base.Items);
        }
        internal FilterProviderCollection(IResolver<IEnumerable<IFilterProvider>> serviceResolver, params IFilterProvider[] providers)
            : base(providers)
        {
            IResolver<IEnumerable<IFilterProvider>> tempResolver = serviceResolver;
            if (serviceResolver == null)
            {
                tempResolver = new MultiServiceResolver<IFilterProvider>(() => base.Items);
            }
            this._serviceResolver = tempResolver;
        }
        private static bool AllowMultiple(object filterInstance)
        {
            IMvcFilter mvcFilter = filterInstance as IMvcFilter;
            return mvcFilter == null || mvcFilter.AllowMultiple;
        }
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (actionDescriptor == null)
            {
                throw new ArgumentNullException("actionDescriptor");
            }
            IEnumerable<Filter> source = this.CombinedItems.Current.SelectMany((IFilterProvider fp) => fp.GetFilters(controllerContext, actionDescriptor)).OrderBy((Filter filter) => filter, FilterProviderCollection._filterComparer);
            return this.RemoveDuplicates(source.Reverse<Filter>()).Reverse<Filter>();
        }
        private IEnumerable<Filter> RemoveDuplicates(IEnumerable<Filter> filters)
        {
            HashSet<Type> hashSet = new HashSet<Type>();
            foreach (Filter current in filters)
            {
                object instance = current.Instance;
                Type type = instance.GetType();
                if (!hashSet.Contains(type) || FilterProviderCollection.AllowMultiple(instance))
                {
                    yield return current;
                    hashSet.Add(type);
                }
            }
            yield break;
        }
    }
}
