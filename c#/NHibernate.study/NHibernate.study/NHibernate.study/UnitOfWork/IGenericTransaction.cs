using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Study.UnitOfWork
{
    public interface IGenericTransaction : IDisposable
    {
        /// <summary>
        /// 提交
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();
    }
}
