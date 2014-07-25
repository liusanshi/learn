using System;
using System.Runtime.CompilerServices;
using System.Globalization;

using LL.Framework.Core.Async;
using System.Web;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public abstract class ControllerBase : IController, IActionFilter, IAuthorizationFilter, IDisposable, IExceptionFilter, IResultFilter
    {
        private readonly SingleEntryGate _executeWasCalledGate = new SingleEntryGate();
        private bool _validateRequest = true;
        private IActionInvoker _actionInvoker;
        private TempDataDictionary _tempDataDictionary;
        private ViewDataDictionary _viewDataDictionary;
        private ITempDataProvider _tempDataProvider;
        /// <summary>
        /// 控制器的上下文
        /// </summary>
        public ControllerContext ControllerContext
        {
            get;
            set;
        }
        /// <summary>
        /// 是否验证请求
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
        /// 视图数据
        /// </summary>
        public ViewDataDictionary ViewData
        {
            get
            {
                if (this._viewDataDictionary == null)
                {
                    this._viewDataDictionary = new ViewDataDictionary();
                }
                return this._viewDataDictionary;
            }
            set
            {
                this._viewDataDictionary = value;
            }
        }
        /// <summary>
        /// 临时数据
        /// </summary>
        public TempDataDictionary TempData
        {
            get
            {
                if (this._tempDataDictionary == null)
                {
                    this._tempDataDictionary = new TempDataDictionary();
                }
                return this._tempDataDictionary;
            }
            set
            {
                this._tempDataDictionary = value;
            }
        }
        /// <summary>
        /// 模型的状态字典
        /// </summary>
        public ModelStateDictionary ModelState
        {
            get { return ViewData.ModelState; }
        }
        /// <summary>
        /// 临时数据提供者
        /// </summary>
        public ITempDataProvider TempDataProvider
        {
            get
            {
                if (this._tempDataProvider == null)
                {
                    this._tempDataProvider = this.CreateTempDataProvider();
                }
                return this._tempDataProvider;
            }
            set
            {
                this._tempDataProvider = value;
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
        /// <summary>
        /// 临时数据提供者
        /// </summary>
        /// <returns></returns>
        protected virtual ITempDataProvider CreateTempDataProvider()
        {
            return new SessionStateTempDataProvider();
        }
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="requestContext"></param>
        protected virtual void Execute(RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            this.VerifyExecuteCalledOnce();
            this.Initialize(requestContext);

            this.ExecuteCore();
        }

        /// <summary>
        /// 控制器执行的核心函数
        /// </summary>
        protected virtual void ExecuteCore()
        {
            this.PossiblyLoadTempData();
            try
            {
                string requiredString = this.RouteData.Action;
                if (!this.ActionInvoker.InvokeAction(ControllerContext, requiredString))
                {
                    throw new HttpException(404, string.Format(CultureInfo.CurrentCulture, "在控制器：{1}中没有找到公开的动作:{0}", new object[]
	                {
		                requiredString,
		                GetType().FullName
	                }));
                }
            }
            finally
            {
                this.PossiblySaveTempData();
            }
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="requestContext"></param>
        protected virtual void Initialize(RequestContext requestContext)
        {
            this.ControllerContext = new ControllerContext(requestContext, this);
        }
        /// <summary>
        /// 验证控制器当前实例只调用了一次
        /// </summary>
        internal void VerifyExecuteCalledOnce()
        {
            if (!this._executeWasCalledGate.TryEnter())
            {
                throw new InvalidOperationException("当前Controller已经执行过了");
            }
        }
        /// <summary>
        /// 尝试加载临时数据
        /// </summary>
        internal void PossiblyLoadTempData()
        {
            TempData.Load(ControllerContext, this.TempDataProvider);
        }
        /// <summary>
        /// 尝试保存临时数据
        /// </summary>
        internal void PossiblySaveTempData()
        {
            TempData.Save(ControllerContext, this.TempDataProvider);
        }
        void IController.Execute(RequestContext requestContext)
        {
            this.Execute(requestContext);
        }

        #region Result 返回
        /// <summary>
        /// 计算view的虚拟路径
        /// </summary>
        /// <returns></returns>
        private string CompViewVirtualPath()
        {
            if (RouteData.UsePageUrlRoute)
            {
                return RouteData.Url;
            }
            else
            {
                return string.Format("/Views/{0}/{1}.ascx", RouteData.ShortController, RouteData.Action);
            }
        }

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected internal virtual RedirectResult Redirect(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("不能为空或者null", "url");
            }
            return new RedirectResult(url);
        }
        /// <summary>
        /// 永久重定向
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected internal virtual RedirectResult RedirectPermanent(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("不能为空或者null", "url");
            }
            return new RedirectResult(url, true);
        }
        /// <summary>
        /// 呈现模板
        /// </summary>
        /// <returns></returns>
        protected internal virtual TemplateViewResult View()
        {
            return View(CompViewVirtualPath());
        }
        /// <summary>
        /// 呈现模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal virtual TemplateViewResult View(object model)
        {
            return View(CompViewVirtualPath(), model);
        }
        /// <summary>
        /// 呈现模板
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        protected internal TemplateViewResult View(string virtualPath)
        {
            return View(virtualPath, null);
        }
        /// <summary>
        /// 呈现模板
        /// </summary>
        /// <param name="virtualPath">文件路径</param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal TemplateViewResult View(string virtualPath, object model)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                throw new ArgumentException("不能为空或者null", "virtualPath");
            }
            if (model != null)
            {
                ViewData.Model = model;
            }
            return new TemplateViewResult(virtualPath, ViewData, TempData);
        }

        /// <summary>
        /// 呈现有缓存功能的模板
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        protected internal CacheTemplateViewResult CacheView(string virtualPath)
        {
            return CacheView(virtualPath, null);
        }
        /// <summary>
        /// 呈现有缓存功能的模板
        /// </summary>
        /// <param name="virtualPath">文件路径</param>
        /// <param name="getModel"></param>
        /// <returns></returns>
        protected internal CacheTemplateViewResult CacheView(string virtualPath, Func<object> getModel)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                throw new ArgumentException("不能为空或者null", "virtualPath");
            }
            return new CacheTemplateViewResult(virtualPath, getModel, ViewData, TempData);
        }

        /// <summary>
        /// 输出xml 数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="requestBehavior"></param>
        /// <returns></returns>
        protected internal XmlResult XML(object model, DataRequestBehavior requestBehavior)
        {
            if (model == null)
            {
                throw new ArgumentException("不能为空或者null", "model");
            }
            return new XmlResult(model, requestBehavior);
        }
        /// <summary>
        /// 输出xml 数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal XmlResult XML(object model)
        {
            return new XmlResult(model);
        }
        /// <summary>
        /// 输出json 数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="requestBehavior"></param>
        /// <returns></returns>
        protected internal JsonResult Json(object model, DataRequestBehavior requestBehavior)
        {
            if (model == null)
            {
                throw new ArgumentException("不能为空或者null", "model");
            }
            return new JsonResult(model, requestBehavior);
        }
        /// <summary>
        /// 输出json 数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal JsonResult Json(object model)
        {
            return new JsonResult(model);
        }
        /// <summary>
        /// 返回 404
        /// </summary>
        /// <returns></returns>
        protected internal virtual HttpNotFoundResult HttpNotFound()
        {
            return new HttpNotFoundResult();
        }
        /// <summary>
        /// 返回 404
        /// </summary>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        protected internal virtual HttpNotFoundResult HttpNotFound(string statusDescription)
        {
            return new HttpNotFoundResult(statusDescription);
        }
        /// <summary>
        /// 返回 js 字符串
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        protected internal virtual JavaScriptResult JavaScript(string script)
        {
            return new JavaScriptResult
            {
                Script = script
            };
        }
        #endregion

        #region IActionFilter 成员
        /// <summary>
        /// 动作执行之前触发
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnActionExecuting(ActionExecutingContext filterContext) { }
        /// <summary>
        /// 动作执行之后触发
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnActionExecuted(ActionExecutedContext filterContext) { }
        /// <summary>
        /// 动作执行之前触发
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 动作执行之后触发
        /// </summary>
        /// <param name="filterContext"></param>
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.OnActionExecuted(filterContext);
        }
        #endregion

        #region IAuthorizationFilter 成员
        /// <summary>
        /// 成员验证的时候触发
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnAuthorization(AuthorizationContext filterContext) { }
        /// <summary>
        /// 成员验证的时候触发
        /// </summary>
        /// <param name="filterContext"></param>
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            this.OnAuthorization(filterContext);
        }
        #endregion

        #region IDisposable 成员
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
        #endregion

        #region IExceptionFilter 成员
        /// <summary>
        /// 发生异常时触发
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnException(ExceptionContext filterContext) { }
        /// <summary>
        /// 发生异常时触发
        /// </summary>
        /// <param name="filterContext"></param>
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            this.OnException(filterContext);
        }
        #endregion

        #region IResultFilter 成员
        /// <summary>
        /// 动作结果结果执行之前触发
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnResultExecuting(ResultExecutingContext filterContext) { }
        /// <summary>
        /// 动作结果结果执行之后触发
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnResultExecuted(ResultExecutedContext filterContext) { }
        /// <summary>
        /// 动作结果结果执行之前触发
        /// </summary>
        /// <param name="filterContext"></param>
        void IResultFilter.OnResultExecuting(ResultExecutingContext filterContext)
        {
            this.OnResultExecuting(filterContext);
        }
        /// <summary>
        /// 动作结果结果执行之后触发
        /// </summary>
        /// <param name="filterContext"></param>
        void IResultFilter.OnResultExecuted(ResultExecutedContext filterContext)
        {
            this.OnResultExecuted(filterContext);
        }
        #endregion

    }
}
