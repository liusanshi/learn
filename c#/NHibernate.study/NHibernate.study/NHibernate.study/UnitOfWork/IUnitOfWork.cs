using System;

namespace NHibernate.Study.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsInActiveTransaction { get; }

        IGenericTransaction BeginTransaction();

        IGenericTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel);

        void TransactionalFlush();

        void TransactionalFlush(System.Data.IsolationLevel isolationLevel);
    }

    public interface IUnitOfWorkImplementor : IUnitOfWork
    {

    }
}
