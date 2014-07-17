using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using LL.Framework.Web.MVC.Serializer;

namespace LL.Framework.Web.MVC
{
    internal class ControllerActionInvoker : IActionInvoker
    {
        private static readonly ControllerDescriptorCache _staticDescriptorCache = new ControllerDescriptorCache();
        private Func<ControllerContext, ActionDescriptor, IEnumerable<Filter>> _getFiltersThunk = (ControllerContext cc, ActionDescriptor ad) => FilterProviders.Providers.GetFilters(cc, ad);

        internal ControllerActionInvoker(params object[] filters): this()
        {
            if (filters != null)
            {
                this._getFiltersThunk = ((ControllerContext cc, ActionDescriptor ad) =>
                    from f in filters
                    select new Filter(f, FilterScope.Action, null));
            }
        }
        internal ControllerActionInvoker() { }

        internal ControllerDescriptorCache DescriptorCache
        {
            get
            {
                return _staticDescriptorCache;
            }
        }
        public virtual bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentException("动作名称为null", "actionName");
            }
            ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(controllerContext);
            ActionDescriptor actionDescriptor = this.FindAction(controllerContext, controllerDescriptor, actionName);
            if (actionDescriptor != null)
            {
                FilterInfo filters = this.GetFilters(controllerContext, actionDescriptor);
                try
                {
                    AuthorizationContext authorizationContext = this.InvokeAuthorizationFilters(controllerContext, filters.AuthorizationFilters, actionDescriptor);
                    if (authorizationContext.Result != null)
                    {
                        this.InvokeActionResult(controllerContext, authorizationContext.Result);
                    }
                    else
                    {
                        if (controllerContext.Controller.ValidateRequest)
                        {
                            ControllerActionInvoker.ValidateRequest(controllerContext);
                        }
                        IDictionary<string, object> parameterValues = this.GetParameterValues(controllerContext, actionDescriptor);
                        ActionExecutedContext actionExecutedContext = this.InvokeActionMethodWithFilters(controllerContext, filters.ActionFilters, actionDescriptor, parameterValues);
                        this.InvokeActionResultWithFilters(controllerContext, filters.ResultFilters, actionExecutedContext.Result);
                    }
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    ExceptionContext exceptionContext = this.InvokeExceptionFilters(controllerContext, filters.ExceptionFilters, exception);
                    if (!exceptionContext.ExceptionHandled)
                    {
                        throw;
                    }
                    this.InvokeActionResult(controllerContext, exceptionContext.Result);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 调用动作方法
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> args)
		{
            object actionReturnValue = actionDescriptor.Execute(controllerContext, args);
			return this.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
		}

        /// <summary>
        /// 创建 ActionResult
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <param name="actionReturnValue"></param>
        /// <returns></returns>
        protected virtual ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            if (actionReturnValue == null)
            {
                return EmptyResult.Instance;
            }
            ActionResult result = actionReturnValue as ActionResult;
            if (result == null)
            {
                result = new ContentResult
                {
                    Content = Convert.ToString(actionReturnValue, CultureInfo.InvariantCulture)
                };
            }
            return result;
        }
        /// <summary>
        /// 获取控制器的描述
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        protected virtual ControllerDescriptor GetControllerDescriptor(ControllerContext controllerContext)
        {
            Type controllerType = controllerContext.Controller.GetType();

            return this.DescriptorCache.GetDescriptor(controllerType);
        }
        /// <summary>
        /// 查找动作
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="controllerDescriptor"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        protected virtual ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            return controllerDescriptor.FindAction(controllerContext, actionName);
        }
        /// <summary>
        /// 获取所有筛选器
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected virtual FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return new FilterInfo(this._getFiltersThunk(controllerContext, actionDescriptor));
        }
        /// <summary>
        /// 执行请求验证
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="filters"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected virtual AuthorizationContext InvokeAuthorizationFilters(ControllerContext controllerContext, IList<IAuthorizationFilter> filters, ActionDescriptor actionDescriptor)
        {
            AuthorizationContext authorizationContext = new AuthorizationContext(controllerContext, actionDescriptor);
            foreach (IAuthorizationFilter current in filters)
            {
                current.OnAuthorization(authorizationContext);
                if (authorizationContext.Result != null)
                {
                    break;
                }
            }
            return authorizationContext;
        }
        /// <summary>
        /// 执行请求结果
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionResult"></param>
        protected virtual void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
        {
            actionResult.ExecuteResult(controllerContext);
        }
        /// <summary>
        /// 获取动作的参数
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> GetParameterValues(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var context = controllerContext.HttpContext;
            IActionParametersProvider provider = ActionParametersProviderFactory.CreateActionParametersProvider(context.Request);
            return provider.GetParameters(context.Request, actionDescriptor);
        }
        /// <summary>
        /// 执行请求方法的筛选器
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="filters"></param>
        /// <param name="actionDescriptor"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            ActionExecutingContext preContext = new ActionExecutingContext(controllerContext, actionDescriptor, parameters);
            Func<ActionExecutedContext> seed = () => new ActionExecutedContext(controllerContext, actionDescriptor, false, null)
            {
                Result = this.InvokeActionMethod(controllerContext, actionDescriptor, parameters)
            };
            Func<ActionExecutedContext> func = filters.Reverse<IActionFilter>().Aggregate(seed
                , (Func<ActionExecutedContext> next, IActionFilter filter) => 
                    () => 
                        ControllerActionInvoker.InvokeActionMethodFilter(filter, preContext, next));
            return func();
        }
        /// <summary>
        /// 执行请求结果的筛选器
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="filters"></param>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        protected virtual ResultExecutedContext InvokeActionResultWithFilters(ControllerContext controllerContext, IList<IResultFilter> filters, ActionResult actionResult)
        {
            ResultExecutingContext preContext = new ResultExecutingContext(controllerContext, actionResult);
            Func<ResultExecutedContext> seed = delegate
            {
                this.InvokeActionResult(controllerContext, actionResult);
                return new ResultExecutedContext(controllerContext, actionResult, false, null);
            };
            Func<ResultExecutedContext> func = filters.Reverse<IResultFilter>().Aggregate(seed, 
                (Func<ResultExecutedContext> next, IResultFilter filter) => 
                    () => 
                        ControllerActionInvoker.InvokeActionResultFilter(filter, preContext, next));
            return func();
        }
        /// <summary>
        /// 执行异常筛选器
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="filters"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual ExceptionContext InvokeExceptionFilters(ControllerContext controllerContext, IList<IExceptionFilter> filters, Exception exception)
        {
            ExceptionContext exceptionContext = new ExceptionContext(controllerContext, exception);
            foreach (IExceptionFilter current in filters.Reverse<IExceptionFilter>())
            {
                current.OnException(exceptionContext);
            }
            return exceptionContext;
        }

        /// <summary>
        /// 执行活动方法筛选器
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="preContext"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        internal static ActionExecutedContext InvokeActionMethodFilter(IActionFilter filter, ActionExecutingContext preContext, Func<ActionExecutedContext> continuation)
        {
            filter.OnActionExecuting(preContext);
            if (preContext.Result != null)
            {
                return new ActionExecutedContext(preContext, preContext.ActionDescriptor, true, null)
                {
                    Result = preContext.Result
                };
            }
            bool flag = false;
            ActionExecutedContext actionExecutedContext = null;
            try
            {
                actionExecutedContext = continuation();
            }
            catch (ThreadAbortException)
            {
                actionExecutedContext = new ActionExecutedContext(preContext, preContext.ActionDescriptor, false, null);
                filter.OnActionExecuted(actionExecutedContext);
                throw;
            }
            catch (Exception exception)
            {
                flag = true;
                actionExecutedContext = new ActionExecutedContext(preContext, preContext.ActionDescriptor, false, exception);
                filter.OnActionExecuted(actionExecutedContext);
                if (!actionExecutedContext.ExceptionHandled)
                {
                    throw;
                }
            }
            if (!flag)
            {
                filter.OnActionExecuted(actionExecutedContext);
            }
            return actionExecutedContext;
        }
        /// <summary>
        /// 验证请求
        /// </summary>
        /// <param name="controllerContext"></param>
        internal static void ValidateRequest(ControllerContext controllerContext)
        {
            //if (controllerContext.IsChildAction)
            //{
            //    return;
            //}
            //ValidationUtility.EnableDynamicValidation(controllerContext.HttpContext);
            controllerContext.HttpContext.Request.ValidateInput();
        }
        /// <summary>
        /// 执行请求结果的筛选器
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="preContext"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        internal static ResultExecutedContext InvokeActionResultFilter(IResultFilter filter, ResultExecutingContext preContext, Func<ResultExecutedContext> continuation)
        {
            filter.OnResultExecuting(preContext);
            if (preContext.Cancel)
            {
                return new ResultExecutedContext(preContext, preContext.Result, true, null);
            }
            bool flag = false;
            ResultExecutedContext resultExecutedContext = null;
            try
            {
                resultExecutedContext = continuation();
            }
            catch (ThreadAbortException)
            {
                resultExecutedContext = new ResultExecutedContext(preContext, preContext.Result, false, null);
                filter.OnResultExecuted(resultExecutedContext);
                throw;
            }
            catch (Exception exception)
            {
                flag = true;
                resultExecutedContext = new ResultExecutedContext(preContext, preContext.Result, false, exception);
                filter.OnResultExecuted(resultExecutedContext);
                if (!resultExecutedContext.ExceptionHandled)
                {
                    throw;
                }
            }
            if (!flag)
            {
                filter.OnResultExecuted(resultExecutedContext);
            }
            return resultExecutedContext;
        }
    }
}
