using System;
using System.Collections.Concurrent;
using System.Linq;

namespace LL.FrameWork.Web.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class FilterAttribute : Attribute, IMvcFilter
    {
        private static readonly ConcurrentDictionary<Type, bool> _multiuseAttributeCache = new ConcurrentDictionary<Type, bool>();
        private int _order = -1;
        public bool AllowMultiple
        {
            get
            {
                return FilterAttribute.AllowsMultiple(base.GetType());
            }
        }
        public int Order
        {
            get
            {
                return this._order;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value", "Order 必须大于等于 -1.");
                }
                this._order = value;
            }
        }
        private static bool AllowsMultiple(Type attributeType)
        {
            return FilterAttribute._multiuseAttributeCache.GetOrAdd(attributeType, (Type type) => type.GetCustomAttributes(typeof(AttributeUsageAttribute), true).Cast<AttributeUsageAttribute>().First<AttributeUsageAttribute>().AllowMultiple);
        }
    }
}
