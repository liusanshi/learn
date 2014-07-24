using System;
using System.IO;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 带有缓存功能的上下文
    /// </summary>
    public class CacheViewContext : ViewContext
    {
        /// <summary>
        /// 创建 CacheViewContext 对象
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="getModel"></param>
        /// <param name="viewData"></param>
        /// <param name="tempData"></param>
        /// <param name="writer"></param>
        public CacheViewContext(ControllerContext controllerContext,
            Func<object> getModel,
            ViewDataDictionary viewData,
            TempDataDictionary tempData,
            TextWriter writer)
            : base(controllerContext, viewData, tempData, writer)
        {
            if (getModel == null)
            {
                GetModel = () => null;
            }
            else
            {
                this.GetModel = getModel;
            }
        }

        private Func<object> GetModel;
        private bool isCalac = false;
        private object model = null;

        /// <summary>
        /// 视图的model
        /// </summary>
        public override object Model
        {
            get
            {
                if (!isCalac)
                {
                    model = GetModel();
                    isCalac = true;
                }
                return model;
            }
            set
            {
                model = value;
                if (model != null)
                    isCalac = true;
            }
        }

        /// <summary>
        /// 创建ViewContext
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="getModel"></param>
        /// <param name="viewData">视图数据</param>
        /// <param name="tempData"></param>
        /// <returns></returns>
        internal static CacheViewContext CreateCacheViewContext(ControllerContext controllerContext, 
            Func<object> getModel, 
            ViewDataDictionary viewData, 
            TempDataDictionary tempData)
        {
            return new CacheViewContext(controllerContext, getModel, viewData,
                tempData ?? new TempDataDictionary(),
                controllerContext.HttpContext.Response.Output);
        }

        /// <summary>
        /// 创建ViewContext
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="getModel"></param>
        /// <returns></returns>
        internal static CacheViewContext CreateCacheViewContext(ControllerContext controllerContext, Func<object> getModel)
        {
            return new CacheViewContext(controllerContext, getModel,
                new ViewDataDictionary(),
                new TempDataDictionary(),
                controllerContext.HttpContext.Response.Output
                );
        }
    }
}
