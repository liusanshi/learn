using System;
using System.Collections.Generic;
using System.Linq;

namespace LL.Framework.Core.Domain.Viewpoints
{
    /// <summary>
    /// 排序操作的描述
    /// </summary>
    /// <example>
    /// 使用用例
    /// <code>
    ///     var order = new ToDoStateViewpoint().Asc()
    ///                 .Then(new ToDoRegisteredDateTimeViewpoint().Asc());
    /// </code>
    /// </example>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <seealso cref="Viewpoint&lt;TEntity, TValue&gt;"/>
    /// <seealso cref="SimpleOrder&lt;TEntity, TValue&gt;"/>
    /// <seealso cref="ComplexOrder&lt;TEntity&gt;"/>
    [Serializable]
    public abstract class Order<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// <see cref="Order&lt;TEntity&gt;"/> 创建实例
        /// </summary>
        internal Order()
        {
        }


        /// <summary>
        /// 对指定的序列进行排序。
        /// </summary>
        /// <param name="target">序列</param>
        /// <returns>排序后的序列</returns>
        /// <exception cref="System.ArgumentNullException">target 为 null </exception>
        public IEnumerable<TEntity> ApplyTo(IEnumerable<TEntity> target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            return OrderFirstly(target.AsQueryable()).AsEnumerable();
        }

        /// <summary>
        /// 对指定的序列进行排序。
        /// </summary>
        /// <param name="target">序列</param>
        /// <returns>排序后的序列</returns>
        /// <exception cref="System.ArgumentNullException">target 为 null </exception>
        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            return OrderFirstly(target);
        }


        /// <summary>
        /// 在序列上进行第一次排序
        /// </summary>
        /// <param name="target">序列</param>
        /// <returns>排序后的序列</returns>
        internal abstract IOrderedQueryable<TEntity> OrderFirstly(IQueryable<TEntity> target);

        /// <summary>
        /// 在已经排序的序列上进行再次排序
        /// </summary>
        /// <param name="target">序列</param>
        /// <returns>排序后的序列</returns>
        internal abstract IOrderedQueryable<TEntity> OrderFurthermore(IOrderedQueryable<TEntity> target);


        /// <summary>
        /// 与后续顺序结合生成新的顺序
        /// </summary>
        /// <param name="following">后续顺序</param>
        /// <returns>返回新的顺序</returns>
        /// <exception cref="System.ArgumentNullException">following 为 null</exception>
        public abstract ComplexOrder<TEntity> Then<TFollowingValue>(SimpleOrder<TEntity, TFollowingValue> following);
    }
}
