using System;
using System.Globalization;

using NHibernate;
using LL.Framework.Core.Infrastructure.Logging;

namespace LL.Framework.Impl.Infrastructure.Logging.Log4NetImpl
{
    class Log4NetLog : ILogger
    {
        IInternalLogger logger = null;

        public Log4NetLog()
        {
            logger = LoggerProvider.LoggerFor("LL.FrameWorkApp");
        }

        public void Debug(string message, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            logger.Debug(messageToTrace);
        }

        public void Debug(string message, Exception exception, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            logger.Debug(messageToTrace, exception);
        }

        public void Debug(object item)
        {
            if (item != null)
            {
                logger.Debug(item.ToString());
            }
        }

        public void Fatal(string message, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);
            logger.Fatal(messageToTrace);
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);
            logger.Fatal(messageToTrace, exception);
        }

        public void LogInfo(string message, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);
            logger.Info(messageToTrace);
        }

        public void LogWarning(string message, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);
            logger.Warn(messageToTrace);
        }

        public void LogError(string message, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);
            logger.Error(messageToTrace);
        }

        public void LogError(string message, Exception exception, params object[] args)
        {
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);
            logger.Error(messageToTrace, exception);
        }
    }
}
