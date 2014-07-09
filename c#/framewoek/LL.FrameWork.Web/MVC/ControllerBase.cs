using System;
using System.Runtime.CompilerServices;
using System.Globalization;
//using System.Web.Routing;

using LL.FrameWork.Core.Async;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public abstract class ControllerBase : IController
    {
        private readonly SingleEntryGate _executeWasCalledGate = new SingleEntryGate();
        private bool _validateRequest = true;
        private IActionInvoker _actionInvoker;
        /// <summary>
        /// 控制器的上下文
        /// </summary>
        public ControllerContext ControllerContext
        {
            get;
            set;
        }
        /// <summary>
        /// 请求是否验证通过
        /// </summary>
        public bool ValidateRequest
        {
            get
            {
                return this._validateRequest;
            }
            set
            {
                this._validateRequest = value;
            }
        }
        /// <summary>
        /// 动作的调用者
        /// </summary>
        public IActionInvoker ActionInvoker
        {
            get
            {
                if (this._actionInvoker == null)
                {
                    this._actionInvoker = this.CreateActionInvoker();
                }
                return this._actionInvoker;
            }
            set
            {
                this._actionInvoker = value;
            }
        }
        /// <summary>
        /// 路由数据
        /// </summary>
        public Route RouteData
        {
            get
            {
                if (ControllerContext != null)
                {
                    return ControllerContext.RouteData;
                }
                return null;
            }
        }
        /// <summary>
        /// 创建动作的调用者
        /// </summary>
        /// <returns></returns>
        protected virtual IActionInvoker CreateActionInvoker()
        {
            return new ControllerActionInvoker();
        }
        protected virtual void Execute(RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            this.VerifyExecuteCalledOnce();
            this.Initialize(requestContext);
            
            this.ExecuteCore();//=====
        }

        /// <summary>
        /// 控制器执行的核心函数
        /// </summary>
        protected virtual void ExecuteCore()
        {
            string requiredString = this.RouteData.GetRequiredString("action");
            if (!this.ActionInvoker.InvokeAction(ControllerContext, requiredString))
            {
                throw new HttpException(404, string.Format(CultureInfo.CurrentCulture, "在控制器：{1}中没有找到公开的动作:{0}", new object[]
	                {
		                requiredString,
		                GetType().FullName
	                }));
            }
        }

        protected virtual void Initialize(RequestContext requestContext)
        {
            this.ControllerContext = new ControllerContext(requestContext, this);
        }
        internal void VerifyExecuteCalledOnce()
        {
            if (!this._executeWasCalledGate.TryEnter())
            {
                throw new InvalidOperationException("当前Controller已经执行过了");
            }
        }
        void IController.Execute(RequestContext requestContext)
        {
            this.Execute(requestContext);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose();
        }
    }
}
