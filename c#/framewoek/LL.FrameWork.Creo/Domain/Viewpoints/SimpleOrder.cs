using System;
using System.Collections.Generic;
using System.Linq;

namespace LL.FrameWork.Core.Domain.Viewpoints
{
    /// <summary>
    /// 单一视点组成序列
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TValue">指定视点的值的类型</typeparam>
    /// <seealso cref="Order&lt;TEntity&gt;"/>
    [Serializable]
    public sealed class SimpleOrder<TEntity, TValue> : Order<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// <see cref="SimpleOrder&lt;TEntity, TValue&gt;"/> 创建新的实例
        /// </summary>
        /// <param name="viewpoint">视点</param>
        /// <param name="direction">排序的顺序</param>
        /// <exception cref="System.ArgumentNullException">viewpoint 为 null</exception>
        /// <exception cref="System.ArgumentException">direction 非法</exception>
        internal SimpleOrder(Viewpoint<TEntity, TValue> viewpoint, Direction direction = Direction.Ascending)
        {
            if (viewpoint == null)
            {
                throw new ArgumentNullException("viewpoint");
            }
            if ((direction != Direction.Ascending) && (direction != Direction.Descending))
            {
                throw new ArgumentException("direction 非法", "direction");
            }
            _viewpoint = viewpoint;
            _direction = direction;
        }


        /// <summary>
        /// 视点
        /// </summary>
        private readonly Viewpoint<TEntity, TValue> _viewpoint;

        /// <summary>
        /// 排序的顺序
        /// </summary>
        private readonly Direction _direction;


        /// <summary>
        /// 获取视点
        /// </summary>
        public Viewpoint<TEntity, TValue> Viewpoint
        {
            get
            {
                return _viewpoint;
            }
        }

        /// <summary>
        /// 获取排序的顺序
        /// </summary>
        public Direction Direction
        {
            get
            {
                return _direction;
            }
        }


        /// <inheritdoc/>
        internal override IOrderedQueryable<TEntity> OrderFirstly(IQueryable<TEntity> target)
        {
            if (_direction == Direction.Ascending)
            {
                return target.OrderBy(_viewpoint.Expression);
            }
            else
            {
                return target.OrderByDescending(_viewpoint.Expression);
            }
        }

        /// <inheritdoc/>
        internal override IOrderedQueryable<TEntity> OrderFurthermore(IOrderedQueryable<TEntity> target)
        {
            if (_direction == Direction.Ascending)
            {
                return target.ThenBy(_viewpoint.Expression);
            }
            else
            {
                return target.ThenByDescending(_viewpoint.Expression);
            }
        }


        /// <summary>
        /// 指定した順序を後続の順序として組み合わせた、新しい順序を生成します。
        /// </summary>
        /// <param name="following">後続の順序。</param>
        /// <returns>指定した順序を後続の順序として組み合わせた、新しい順序。</returns>
        /// <exception cref="System.ArgumentNullException">following が null です。</exception>
        public override ComplexOrder<TEntity> Then<TFollowingValue>(SimpleOrder<TEntity, TFollowingValue> following)
        {
            if (following == null)
            {
                throw new ArgumentNullException("following");
            }
            return new ComplexOrder<TEntity>(this, following);
        }
    }
}
