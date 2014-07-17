using System;
using System.Collections.Generic;
using System.Linq;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 特性筛选器的提供者
    /// </summary>
    public class FilterAttributeFilterProvider : IFilterProvider
    {
        private readonly bool _cacheAttributeInstances;
        /// <summary>
        /// 特性筛选器的提供者
        /// </summary>
        public FilterAttributeFilterProvider()
            : this(true)
        {
        }
        public FilterAttributeFilterProvider(bool cacheAttributeInstances)
        {
            this._cacheAttributeInstances = cacheAttributeInstances;
        }
        protected virtual IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetFilterAttributes(this._cacheAttributeInstances);
        }
        protected virtual IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.ControllerDescriptor.GetFilterAttributes(this._cacheAttributeInstances);
        }
        public virtual IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (controllerContext.Controller == null)
            {
                return Enumerable.Empty<Filter>();
            }
            IEnumerable<Filter> first =
                from attr in this.GetControllerAttributes(controllerContext, actionDescriptor)
                select new Filter(attr, FilterScope.Controller, null);
            IEnumerable<Filter> second =
                from attr in this.GetActionAttributes(controllerContext, actionDescriptor)
                select new Filter(attr, FilterScope.Action, null);
            return first.Concat(second).ToList<Filter>();
        }
    }
}
