using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedGlovePermission.Web
{
    /// <summary>
    /// 丢失Session异常。
    /// </summary>
    public class ExceptionSession : Exception
    {
        public ExceptionSession() : base() { }
        public ExceptionSession(string message) : base(message) { }
        public ExceptionSession(string message, Exception inner) : base(message, inner) { }
    }
}
