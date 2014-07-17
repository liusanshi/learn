using System;

using NHibernate;
using LL.Framework.Core.Domain;

namespace LL.Framework.Impl.UOW
{
    public interface INhibernateUnitOfWork : IUnitOfWork
    {
        bool IsInActiveTransaction { get; }
    }
}
