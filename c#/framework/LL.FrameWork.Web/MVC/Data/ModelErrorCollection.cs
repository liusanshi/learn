using System;
using System.Collections.ObjectModel;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// ModelError 集合
    /// </summary>
    public class ModelErrorCollection : Collection<ModelError>
    {
        /// <summary>
        /// 添加异常信息
        /// </summary>
        /// <param name="exception"></param>
        public void Add(Exception exception)
        {
            base.Add(new ModelError(exception));
        }
        /// <summary>
        /// 添加错误描述
        /// </summary>
        /// <param name="errorMessage"></param>
        public void Add(string errorMessage)
        {
            base.Add(new ModelError(errorMessage));
        }
    }
}
