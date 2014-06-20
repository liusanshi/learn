using System;

using NHibernate;
using NHibernate.Cfg;

namespace LL.FrameWork.Implementation.UOW
{
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// NHibernate Configuration
        /// </summary>
        Configuration Configuration { get; set; }
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
        INhibernateUnitOfWork Create();
        /// <summary>
        /// 创建是否有状态的工作单元
        /// </summary>
        /// <param name="hasState"></param>
        /// <returns></returns>
        INhibernateUnitOfWork Create(bool hasState);
        /// <summary>
        /// 释放工作单元
        /// </summary>
        /// <param name="adapter"></param>
        void DisposeUnitOfWork(INhibernateUnitOfWork adapter);
    }
}
