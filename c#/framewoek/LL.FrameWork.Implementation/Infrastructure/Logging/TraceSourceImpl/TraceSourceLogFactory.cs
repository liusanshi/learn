using LL.FrameWork.Core.Infrastructure.Logging;

namespace LL.FrameWork.Impl.Infrastructure.Logging.TraceSourceImpl
{
    /// <summary>
    /// A Trace Source base, log factory
    /// </summary>
    public class TraceSourceLogFactory
        : ILoggerFactory
    {
        /// <summary>
        /// Create the trace source log
        /// </summary>
        /// <returns>New ILog based on Trace Source infrastructure</returns>
        public ILogger Create()
        {
            return new TraceSourceLog();
        }
    }
}
