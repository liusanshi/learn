using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LL.Framework.Core.Domain.ExpressionVisitors;

namespace LL.Framework.Core.Domain.Expanders
{
    /// <summary>
    /// 对象的扩展
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class Expander<TEntity> where TEntity : class
    {
        private readonly IEnumerable<IExpandable<TEntity>> _expandables;
        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <param name="expandables"></param>
        public Expander(params IExpandable<TEntity>[] expandables)
        {
            _expandables = expandables ?? new IExpandable<TEntity>[0];
        }

        /// <summary>
        /// 编织扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        internal Expression<Func<TEntity, T>> Expand<T>(Expression<Func<TEntity, T>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var adjuster = new ParameterVisitor(node => source.Parameters.Single());
            var adjustedExpandables =
                from expandable in _expandables
                select new
                {
                    From = expandable.From(),
                    To = (adjuster.Visit(expandable.To()))
                };

            var propertyReplacer = new MemberVisitor(node =>
            {
                var query =
                    from expandable in adjustedExpandables
                    where (expandable.From == node.Member)
                    select expandable.To;
                return query.SingleOrDefault() ?? node;
            });

            return (Expression<Func<TEntity, T>>)propertyReplacer.Visit(source);
        }
    }
}
