using System;
using System.Globalization;
using System.Web.UI;

namespace LL.Framework.Web.MVC
{
	/// <summary>
    /// MasterPage基类 母版页基类
	/// </summary>
    [FileLevelControlBuilder(typeof(ViewMasterPageControlBuilder))]
    public class ViewMasterPage : MasterPage
	{
        /// <summary>
        /// HTML帮助对象
        /// </summary>
        public HTMLHelper<object> HTML
        {
            get
            {
                return this.ViewPage.HTML;
            }
        }
        /// <summary>
        /// 实体对像
        /// </summary>
        public object Model
        {
            get
            {
                return this.ViewData.Model;
            }
        }
        /// <summary>
        /// 临时数据
        /// </summary>
        public TempDataDictionary TempData
        {
            get
            {
                return this.ViewPage.TempData;
            }
        }
        /// <summary>
        /// 视图数据
        /// </summary>
        public ViewDataDictionary ViewData
        {
            get
            {
                return this.ViewContext.ViewData;
            }
        }
        /// <summary>
        /// 视图上下文
        /// </summary>
        public ViewContext ViewContext
        {
            get
            {
                return this.ViewPage.ViewContext;
            }
        }
        /// <summary>
        /// 母版页所在的页面
        /// </summary>
        internal ViewPageBase ViewPage
        {
            get
            {
                ViewPageBase viewPage = this.Page as ViewPageBase;
                if (viewPage == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "母版页所在的页面必须继承ViewPageBase 或者ViewPageBase<TModel>", new object[0]));
                }
                return viewPage;
            }
        }
        /// <summary>
        /// 页面HTML写入入口
        /// </summary>
        public HtmlTextWriter Writer
        {
            get
            {
                return this.ViewPage.Writer;
            }
        }
	}
}
