using System;

using NHibernate;
using NHibernate.Cfg;

namespace LL.FrameWork.Implementation.UOW
{
    public static class UnitOfWork
    {
        private static IUnitOfWorkFactory mUnitOfWorkFactory = new UnitOfWorkFactory();

        /// <summary>
        /// 当前的配置
        /// </summary>
        public static Configuration Configuration
        {
            get { return mUnitOfWorkFactory.Configuration; }
            set { mUnitOfWorkFactory.Configuration = value; }
        }

        private const string CurrentUnitOfWorkKey = "CurrentUnitOfWork.Key";

        private static INhibernateUnitOfWork CurrentUnitOfWork
        {
            get { return Local.Data[CurrentUnitOfWorkKey] as INhibernateUnitOfWork; }
            set { Local.Data[CurrentUnitOfWorkKey] = value; }
        }
        /// <summary>
        /// 启动一个有状态的工作单元
        /// </summary>
        /// <returns></returns>
        public static INhibernateUnitOfWork Start()
        {
            if (CurrentUnitOfWork != null)
                throw new InvalidOperationException("You cannot start more than one unit of work at the same time.");

            var unitOfWork = mUnitOfWorkFactory.Create();
            CurrentUnitOfWork = unitOfWork;
            return unitOfWork;
        }
        /// <summary>
        /// 启动一个无状态的工作单元
        /// </summary>
        /// <returns></returns>
        public static INhibernateUnitOfWork StartStateless()
        {
            if (CurrentUnitOfWork != null)
                throw new InvalidOperationException("You cannot start more than one unit of work at the same time.");

            var unitOfWork = mUnitOfWorkFactory.Create(false);
            CurrentUnitOfWork = unitOfWork;
            return unitOfWork;
        }

        /// <summary>
        /// 当前的工作单元
        /// </summary>
        public static INhibernateUnitOfWork Current
        {
            get
            {
                var unitOfWork = CurrentUnitOfWork;
                if (unitOfWork == null)
                    throw new InvalidOperationException("You are not in a unit of work");
                return unitOfWork;
            }
        }
        /// <summary>
        /// 当前有状态的会话
        /// </summary>
        public static ISession CurrentSession
        {
            get { return mUnitOfWorkFactory.CurrentSession; }
            internal set { mUnitOfWorkFactory.CurrentSession = value; }
        }
        /// <summary>
        /// 当前无状态的会话
        /// </summary>
        public static IStatelessSession CurrentStatelessSession
        {
            get { return mUnitOfWorkFactory.CurrentStatelessSession; }
            internal set { mUnitOfWorkFactory.CurrentStatelessSession = value; }
        }

        /// <summary>
        /// 是否已经启动
        /// </summary>
        public static bool IsStarted
        {
            get { return CurrentUnitOfWork != null; }
        }

        /// <summary>
        /// 释放工作单元
        /// </summary>
        /// <param name="adapter"></param>
        public static void DisposeUnitOfWork(INhibernateUnitOfWork adapter)
        {
            CurrentUnitOfWork = null;
        }
    }
}
