namespace LL.Framework.Web.MVC
{
    using System;

    /// <summary>
    /// 动作结果执行之前的上下文
    /// </summary>
    public class ResultExecutingContext : ControllerContext
    {
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }
        /// <summary>
        /// 动作执行的结果
        /// </summary>
        public virtual ActionResult Result
        {
            get;
            set;
        }
        /// <summary>
        /// 创建ResultExecutingContext 实例
        /// </summary>
        protected ResultExecutingContext()
        {
        }
        /// <summary>
        /// 创建ResultExecutingContext 实例
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="result"></param>
        public ResultExecutingContext(ControllerContext controllerContext, ActionResult result)
            : base(controllerContext)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            this.Result = result;
        }
    }
}
