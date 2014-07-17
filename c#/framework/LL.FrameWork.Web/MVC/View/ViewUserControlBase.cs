using System;
using System.Web;
using System.Collections.Specialized;
using System.Web.UI;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 一个“用户控件”基类
    /// </summary>
    [FileLevelControlBuilder(typeof(ViewUserControlControlBuilder))]
    public class ViewUserControlBase : UserControl
    {
        private HTMLHelper<object> _htmlHelper;
        /// <summary>
        /// html帮助对象
        /// </summary>
        public HTMLHelper<object> HTML
        {
            get
            {
                return _htmlHelper;
            }
        }
        /// <summary>
        /// 试图数据
        /// </summary>
        public object Model
        {
            get
            {
                return this.ViewContext.Model;
            }
        }
        /// <summary>
        /// 可以保持的临时数据
        /// </summary>
        public TempDataDictionary TempData
        {
            get
            {
                return this.ViewContext.TempData;
            }
        }
        /// <summary>
        /// 页面HTML写入入口
        /// </summary>
        public HtmlTextWriter Writer
        {
            get { return this.ViewPage.Writer; }
        }

        /// <summary>
        /// 页面视图
        /// </summary>
        internal ViewPageBase ViewPage
        {
            get
            {
                ViewPageBase viewPage = this.Page as ViewPageBase;
                if (viewPage == null)
                {
                    throw new InvalidOperationException("这个ViewUserControl 必须用于继承至ViewPageBase 或 ViewPageBase<TModel> 的页面");
                }
                return viewPage;
            }
        }

        /// <summary>
        /// 视图上下文
        /// </summary>
        public virtual ViewContext ViewContext { get; set; }
        
        /// <summary>
        /// 初始化帮助对象
        /// </summary>
        protected virtual void InitHelpers()
        {
            _htmlHelper = new HTMLHelper<object>(ViewContext, Model);
        }

        /// <summary>
        /// 呈现视图
        /// </summary>
        /// <param name="viewContext"></param>
        public virtual void RenderView(ViewContext viewContext)
        {
            InitHelpers();
            using (ViewUserControlBase.ViewUserControlContainerPage viewUserControlContainerPage = new ViewUserControlBase.ViewUserControlContainerPage(this))
            {
                string contentType = viewContext.HttpContext.Response.ContentType;
                viewUserControlContainerPage.RenderView(viewContext);
                viewContext.HttpContext.Response.ContentType = contentType;
            }
        }

        /// <summary>
        /// 内部包裹UserControl的Page页面
        /// </summary>
        sealed class ViewUserControlContainerPage : ViewPageBase
        {
            private readonly ViewUserControlBase _userControl;
            public ViewUserControlContainerPage(ViewUserControlBase userControl)
            {
                this._userControl = userControl;
            }
            public override void ProcessRequest(HttpContext context)
            {
                this._userControl.ID = ViewPageBase.NextId();
                this.Controls.Add(this._userControl);
                base.ProcessRequest(context);
            }
        }
    }
}
