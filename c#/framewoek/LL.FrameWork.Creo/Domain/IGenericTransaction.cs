using System;

namespace LL.FrameWork.Core.Domain
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
