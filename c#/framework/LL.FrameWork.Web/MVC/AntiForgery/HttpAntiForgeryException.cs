using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 发生了伪造的请求，发出的异常
    /// </summary>
    [Serializable]
    public class HttpAntiForgeryException : HttpException
    {
        /// <summary>
        /// 伪造请求异常
        /// </summary>
        public HttpAntiForgeryException()
        {
        }
        /// <summary>
        /// 伪造请求异常
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private HttpAntiForgeryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        /// <summary>
        /// 伪造请求异常
        /// </summary>
        /// <param name="message"></param>
        public HttpAntiForgeryException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// 伪造请求异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HttpAntiForgeryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
