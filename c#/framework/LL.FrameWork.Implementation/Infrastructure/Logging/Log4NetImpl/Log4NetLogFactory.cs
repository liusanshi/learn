using LL.FrameWork.Core.Infrastructure.Logging;

namespace LL.FrameWork.Impl.Infrastructure.Logging.Log4NetImpl
{
    public class Log4NetLogFactory : ILoggerFactory
    {
        public ILogger Create()
        {
            return new Log4NetLog();
        }
    }
}
