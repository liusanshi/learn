using System;
using System.Linq;
using System.Collections.Generic;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 筛选器信息
    /// </summary>
    public class FilterInfo
    {
        private List<IActionFilter> _actionFilters = new List<IActionFilter>();
        private List<IAuthorizationFilter> _authorizationFilters = new List<IAuthorizationFilter>();
        private List<IExceptionFilter> _exceptionFilters = new List<IExceptionFilter>();
        private List<IResultFilter> _resultFilters = new List<IResultFilter>();
        public IList<IActionFilter> ActionFilters
        {
            get
            {
                return this._actionFilters;
            }
        }
        public IList<IAuthorizationFilter> AuthorizationFilters
        {
            get
            {
                return this._authorizationFilters;
            }
        }
        public IList<IExceptionFilter> ExceptionFilters
        {
            get
            {
                return this._exceptionFilters;
            }
        }
        public IList<IResultFilter> ResultFilters
        {
            get
            {
                return this._resultFilters;
            }
        }
        public FilterInfo()
        {
        }
        public FilterInfo(IEnumerable<Filter> filters)
        {
            List<object> source = (
                from f in filters
                select f.Instance).ToList<object>();
            this._actionFilters.AddRange(source.OfType<IActionFilter>());
            this._authorizationFilters.AddRange(source.OfType<IAuthorizationFilter>());
            this._exceptionFilters.AddRange(source.OfType<IExceptionFilter>());
            this._resultFilters.AddRange(source.OfType<IResultFilter>());
        }
    }

    /// <summary>
    /// 单个筛选器
    /// </summary>
    public class Filter
    {
        public const int DefaultOrder = -1;
        public object Instance
        {
            get;
            protected set;
        }
        public int Order
        {
            get;
            protected set;
        }
        public FilterScope Scope
        {
            get;
            protected set;
        }
        public Filter(object instance, FilterScope scope, int? order)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (!order.HasValue)
            {
                IMvcFilter mvcFilter = instance as IMvcFilter;
                if (mvcFilter != null)
                {
                    order = new int?(mvcFilter.Order);
                }
            }
            this.Instance = instance;
            this.Order = (order ?? -1);
            this.Scope = scope;
        }
    }

    /// <summary>
    /// 筛选器的作用范围
    /// </summary>
    public enum FilterScope
    {
        First,
        Global = 10,
        Controller = 20,
        Action = 30,
        Last = 100
    }
}
