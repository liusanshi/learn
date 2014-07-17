namespace LL.Framework.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LL.Framework.Core.Domain.Specification;
    using LL.Framework.Core.Domain.Viewpoints;

    /// <summary>
    /// Repository 的基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class RepositoryBase<TEntity, TID> : IRepository<TEntity, TID>
        where TEntity : EntityBase<TID>
    {
        /// <inheritdoc/>
        public TEntity Find(Specification<TEntity> specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }
            var query = CreateQuery(specification);
            return query.SingleOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> FindList(Specification<TEntity> specification, Order<TEntity> order = null, int? skipCount = null, int? takeCount = null)
        {
            var query = CreateQuery(specification, order, skipCount, takeCount);
            return query.ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> FindAll(Order<TEntity> order = null, int? skipCount = null, int? takeCount = null)
        {
            var query = CreateQuery(null, order, skipCount, takeCount);
            return query.ToList();
        }


        /// <inheritdoc/>
        public bool Exists(Specification<TEntity> specification)
        {
            var query = CreateQuery(specification);
            return query.Any();
        }


        /// <inheritdoc/>
        public int GetCount(Specification<TEntity> specification)
        {
            var query = CreateQuery(specification);
            return query.Count();
        }


        /// <summary>
        /// 生成查询对象
        /// </summary>
        /// <param name="specification">查询条件的描述</param>
        /// <param name="order">排序的描述</param>
        /// <param name="skipCount">需要跳过的数量</param>
        /// <param name="takeCount">返回的数量</param>
        /// <returns>查询对象</returns>
        private IQueryable<TEntity> CreateQuery(Specification<TEntity> specification, Order<TEntity> order = null, int? skipCount = null, int? takeCount = null)
        {
            var query = CreateBaseQuery();
            if (specification != null)
            {
                query = query.Where(specification.Expression);
            }
            if (order != null)
            {
                query = order.ApplyTo(query);
            }
            if (skipCount.HasValue)
            {
                query = query.Skip(skipCount.Value);
            }
            if (takeCount.HasValue)
            {
                query = query.Take(takeCount.Value);
            }
            return query;
        }

        /// <summary>
        /// 生成基础查询对象
        /// </summary>
        /// <returns>返回查询对象</returns>
        protected abstract IQueryable<TEntity> CreateBaseQuery();

        /// <inheritdoc/>
        public abstract TEntity FindById(TID id);

        /// <inheritdoc/>
        public abstract void Add(TEntity target);

        /// <inheritdoc/>
        public abstract void Remove(TEntity target);

        /// <inheritdoc/>
        public abstract IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc/>
        public abstract void Modify(TEntity item);

        /// <inheritdoc/>
        public abstract void TrackItem(TEntity item);

        /// <inheritdoc/>
        public abstract void Merge(TEntity persisted, TEntity current);

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
