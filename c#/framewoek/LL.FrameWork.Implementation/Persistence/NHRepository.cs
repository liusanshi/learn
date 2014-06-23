using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LL.FrameWork.Core.Domain;
using LL.FrameWork.Impl.UOW;
using NHibernate;
using NHibernate.Linq;

namespace LL.FrameWork.Impl.Persistence
{
    public class NHRepository<TEntity, TID> : RepositoryBase<TEntity, TID>
        where TEntity : EntityBase<TID>
    {
        INhibernateUnitOfWork NHUnitOfWork = null;
        ISession _currentSession;

        public NHRepository(INhibernateUnitOfWork nhunitofwork, ISession session)
        {
            NHUnitOfWork = nhunitofwork;
            _currentSession = session;
        }

        protected override IQueryable<TEntity> CreateBaseQuery()
        {
            return _currentSession.Query<TEntity>();
        }

        public override TEntity FindById(TID id)
        {
            return _currentSession.Get<TEntity>(id);
        }

        public override void Add(TEntity target)
        {
            _currentSession.Save(target);
        }

        public override void Remove(TEntity target)
        {
            _currentSession.Delete(target);
        }

        public override IUnitOfWork UnitOfWork
        {
            get { return NHUnitOfWork; }
        }

        public override void Modify(TEntity item)
        {
            _currentSession.Update(item);
        }

        public override void TrackItem(TEntity item)
        {
            _currentSession.Update(item);
        }

        public override void Merge(TEntity persisted, TEntity current)
        {
            if (!persisted.Id.Equals(current.Id))
                throw new ArgumentException("两个对象不能合并");
            _currentSession.SaveOrUpdate(persisted);
            _currentSession.Merge<TEntity>(current);
        }
    }
}
