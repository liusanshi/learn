using System;

using NHibernate;
using LL.FrameWork.Core.Domain;

namespace LL.FrameWork.Implementation.UOW
{
    public interface INhibernateUnitOfWork : IUnitOfWork
    {
        bool IsInActiveTransaction { get; }
    }
}
