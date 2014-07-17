using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LL.Framework.Core.Domain.Viewpoints
{
    /// <summary>
    /// 组合顺序
    /// </summary>
    /// <typeparam name="TEntity">实体的类型。</typeparam>
    /// <seealso cref="Order&lt;TEntity&gt;"/>
    [Serializable]
    public sealed class ComplexOrder<TEntity> : Order<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// <see cref="ComplexOrder&lt;TEntity&gt;"/> 初始化类的新实例。
        /// </summary>
        /// <param name="first">原始顺序</param>
        /// <param name="followings">后续顺序</param>
        /// <exception cref="System.ArgumentNullException">first 是 null。</exception>
        internal ComplexOrder(Order<TEntity> first, params Order<TEntity>[] followings)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            _first = first;
            _followings = (followings ?? new Order<TEntity>[0]).ToList().AsReadOnly();
        }


        /// <summary>
        /// 原始顺序
        /// </summary>
        private readonly Order<TEntity> _first;

        /// <summary>
        /// 后续顺序
        /// </summary>
        private readonly ReadOnlyCollection<Order<TEntity>> _followings;


        /// <summary>
        /// 获取的原始顺序
        /// </summary>
        public Order<TEntity> First
        {
            get
            {
                return _first;
            }
        }

        /// <summary>
        /// 获取后续顺序
        /// </summary>
        public ReadOnlyCollection<Order<TEntity>> Followings
        {
            get
            {
                return _followings;
            }
        }


        /// <inheritdoc/>
        internal override IOrderedQueryable<TEntity> OrderFirstly(IQueryable<TEntity> target)
        {
            var result = _first.OrderFirstly(target);

            foreach (var following in _followings)
            {
                result = following.OrderFurthermore(result);
            }

            return result;
        }

        /// <inheritdoc/>
        internal override IOrderedQueryable<TEntity> OrderFurthermore(IOrderedQueryable<TEntity> target)
        {
            var result = _first.OrderFurthermore(target);

            foreach (var following in _followings)
            {
                result = following.OrderFurthermore(result);
            }

            return result;
        }


        /// <summary>
        /// 与后续顺序结合生成新的顺序。
        /// </summary>
        /// <param name="following">后续顺序</param>
        /// <returns>返回新的顺序</returns>
        /// <exception cref="System.ArgumentNullException">following 为 null</exception>
        public override ComplexOrder<TEntity> Then<TFollowingValue>(SimpleOrder<TEntity, TFollowingValue> following)
        {
            if (following == null)
            {
                throw new ArgumentNullException("following");
            }
            var followings = Enumerable.Concat(_followings, new[] { following }).ToArray();
            return new ComplexOrder<TEntity>(_first, followings);
        }
    }
}
