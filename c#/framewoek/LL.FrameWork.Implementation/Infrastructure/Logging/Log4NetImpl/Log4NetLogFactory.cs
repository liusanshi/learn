using LL.Framework.Core.Infrastructure.Logging;

namespace LL.Framework.Impl.Infrastructure.Logging.Log4NetImpl
{
    public class Log4NetLogFactory : ILoggerFactory
    {
        public ILogger Create()
        {
            return new Log4NetLog();
        }
    }
}
