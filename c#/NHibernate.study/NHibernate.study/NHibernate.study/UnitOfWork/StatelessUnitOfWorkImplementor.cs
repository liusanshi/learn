using System;

namespace NHibernate.Study.UnitOfWork
{
    public class StatelessUnitOfWorkImplementor : IUnitOfWorkImplementor
   {
        private readonly IUnitOfWorkFactory _factory;
        private readonly IStatelessSession _session;

        public StatelessUnitOfWorkImplementor(IUnitOfWorkFactory factory, IStatelessSession session)
        {
            _factory = factory;
            _session = session;
        }

        public void Dispose()
        {
            _factory.DisposeUnitOfWork(this);
            _session.Dispose();
        }

        public bool IsInActiveTransaction
        {
            get
            {
                return _session.Transaction.IsActive;
            }
        }

        public IUnitOfWorkFactory Factory
        {
            get { return _factory; }
        }

        public IStatelessSession Session
        {
            get { return _session; }
        }

        public IGenericTransaction BeginTransaction()
        {
            return new GenericTransaction(_session.BeginTransaction());
        }

        public IGenericTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return new GenericTransaction(_session.BeginTransaction(isolationLevel));
        }

        public void TransactionalFlush()
        {
            TransactionalFlush(System.Data.IsolationLevel.ReadCommitted);
        }

        public void TransactionalFlush(System.Data.IsolationLevel isolationLevel)
        {
            IGenericTransaction tx = BeginTransaction(isolationLevel);
            try
            {
                //forces a flush of the current unit of work
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            finally
            {
                tx.Dispose();
            }
        }
    }
}
