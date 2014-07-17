using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LL.Framework.Web.MVC
{
    public sealed class GlobalFilterCollection : IEnumerable<Filter>, IEnumerable, IFilterProvider
    {
        private List<Filter> _filters = new List<Filter>();
        public int Count
        {
            get
            {
                return this._filters.Count;
            }
        }
        public void Add(object filter)
        {
            this.AddInternal(filter, null);
        }
        public void Add(object filter, int order)
        {
            this.AddInternal(filter, new int?(order));
        }
        private void AddInternal(object filter, int? order)
        {
            this._filters.Add(new Filter(filter, FilterScope.Global, order));
        }
        public void Clear()
        {
            this._filters.Clear();
        }
        public bool Contains(object filter)
        {
            return this._filters.Any((Filter f) => f.Instance == filter);
        }
        public IEnumerator<Filter> GetEnumerator()
        {
            return this._filters.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._filters.GetEnumerator();
        }
        IEnumerable<Filter> IFilterProvider.GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return this;
        }
        public void Remove(object filter)
        {
            this._filters.RemoveAll((Filter f) => f.Instance == filter);
        }
    }
}
