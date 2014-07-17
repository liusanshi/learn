using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

namespace LL.Framework.Impl.UOW
{
    /// <summary>
    /// 自动管理Seesion
    /// </summary>
    public class AutoSessionAttribute : Attribute, IInterceptor
    {
        /// <summary>
        /// 是否有状态
        /// </summary>
        private bool IsStateSeesion = true;
        /// <summary>
        /// 创建 自动管理Seesion 对象
        /// 默认是有状态的
        /// </summary>
        public AutoSessionAttribute() : this(true) { }
        /// <summary>
        /// 创建 自动管理Seesion 对象
        /// </summary>
        /// <param name="hasStateless">是否有状态</param>
        public AutoSessionAttribute(bool hasStateless) { IsStateSeesion = hasStateless; }

        public void Intercept(IInvocation invocation)
        {
            if (UnitOfWork.IsStarted)
            {
                invocation.Proceed();
            }
            else if (IsStateSeesion)
            {
                using (UnitOfWork.Start())
                {
                    invocation.Proceed();
                }
            }
            else
            {
                using (UnitOfWork.StartStateless())
                {
                    invocation.Proceed();
                }
            }
        }
    }
}
