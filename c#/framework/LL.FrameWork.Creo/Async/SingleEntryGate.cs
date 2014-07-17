using System.Threading;

namespace LL.Framework.Core.Async
{
    /// <summary>
    /// 只能执行一次
    /// </summary>
    public sealed class SingleEntryGate
    {
        /// <summary>
        /// 为执行过
        /// </summary>
        private const int NOT_ENTERED = 0;
        /// <summary>
        /// 执行过
        /// </summary>
        private const int ENTERED = 1;
        /// <summary>
        /// 当前状态
        /// </summary>
        private int _status;
        /// <summary>
        /// 是否已经执行过
        /// </summary>
        /// <returns></returns>
        public bool TryEnter()
        {
            int num = Interlocked.Exchange(ref this._status, ENTERED);
            return num == NOT_ENTERED;
        }
    }
}
