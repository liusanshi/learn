using System;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 模型错误信息
    /// </summary>
    [Serializable]
    public class ModelError
    {
        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception
        {
            get;
            private set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        #region 构造函数
        /// <summary>
        /// 创建 ModelError 实例
        /// </summary>
        /// <param name="exception">异常信息</param>
        public ModelError(Exception exception)
            : this(exception, null)
        {
        }
        /// <summary>
        /// 创建 ModelError 实例
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="errorMessage">错误描述</param>
        public ModelError(Exception exception, string errorMessage)
            : this(errorMessage)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            this.Exception = exception;
        }
        /// <summary>
        /// 创建 ModelError 实例
        /// </summary>
        /// <param name="errorMessage">错误描述</param>
        public ModelError(string errorMessage)
        {
            this.ErrorMessage = (errorMessage ?? string.Empty);
        }
        #endregion
        
    }
}
