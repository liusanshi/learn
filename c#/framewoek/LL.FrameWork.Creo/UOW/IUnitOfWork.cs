using System;

using NHibernate;

namespace LL.FrameWork.Core.UOW
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
