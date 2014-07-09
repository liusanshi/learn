using System;
using System.Runtime.CompilerServices;
using System.Web.Routing;

using LL.FrameWork.Core.Async;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    public abstract class ControllerBase : IController
    {
        private readonly SingleEntryGate _executeWasCalledGate = new SingleEntryGate();
        private bool _validateRequest = true;
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
        protected virtual void Execute(HttpContext httpcontext)
        {
            if (httpcontext == null)
            {
                throw new ArgumentNullException("httpcontext");
            }
            this.VerifyExecuteCalledOnce();
            this.Initialize(httpcontext);
            
            this.ExecuteCore();//=====
        }
        protected abstract void ExecuteCore();
        protected virtual void Initialize(HttpContext httpcontext)
        {
            this.ControllerContext = new ControllerContext(httpcontext, this);
        }
        internal void VerifyExecuteCalledOnce()
        {
            if (!this._executeWasCalledGate.TryEnter())
            {
                throw new InvalidOperationException("当前Controller已经执行过了");
            }
        }
        void IController.Execute(HttpContext httpcontext)
        {
            this.Execute(httpcontext);
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
