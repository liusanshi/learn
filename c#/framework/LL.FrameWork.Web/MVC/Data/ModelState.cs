using System;
using System.Collections.Generic;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 模型状态信息
    /// </summary>
    [Serializable]
    public class ModelState
    {
        private ModelErrorCollection _errors = new ModelErrorCollection();
        /// <summary>
        /// 模型错误信息
        /// </summary>
        public ModelErrorCollection Errors
        {
            get
            {
                return this._errors;
            }
        }
    }
}
