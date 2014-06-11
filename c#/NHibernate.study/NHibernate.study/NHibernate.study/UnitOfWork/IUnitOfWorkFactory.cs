using System;

using NHibernate.Cfg;

namespace NHibernate.Study.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// NHibernate Configuration
        /// </summary>
        Configuration Configuration { get; }
        /// <summary>
        /// SessionFactory
        /// </summary>
        ISessionFactory SessionFactory { get; }
        /// <summary>
        /// 当前的Session
        /// </summary>
        ISession CurrentSession { get; }

        IUnitOfWork Create();
    }
}
