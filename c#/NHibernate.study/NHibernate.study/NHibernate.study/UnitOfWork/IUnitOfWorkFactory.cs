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
        ISession CurrentSession { get; set; }

        IStatelessSession CurrentStatelessSession { get; set; }

        /// <summary>
        /// 创建有状态的工作单元 
        /// </summary>
        /// <returns></returns>
        IUnitOfWork Create();
        /// <summary>
        /// 创建是否有状态的工作单元
        /// </summary>
        /// <param name="hasState"></param>
        /// <returns></returns>
        IUnitOfWork Create(bool hasState);
        /// <summary>
        /// 释放工作单元
        /// </summary>
        /// <param name="adapter"></param>
        void DisposeUnitOfWork(IUnitOfWorkImplementor adapter);
    }
}
