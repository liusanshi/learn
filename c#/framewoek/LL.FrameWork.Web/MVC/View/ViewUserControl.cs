using System;
using System.Web;
using System.Collections.Specialized;
using System.CodeDom;
using System.Web.UI;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 一个“用户控件”基类
    /// </summary>
    [FileLevelControlBuilder(typeof(ViewUserControlControlBuilder))]
    public class ViewUserControl : UserControl
    {
        private HTMLHelper _htmlHelper;
        /// <summary>
        /// html帮助类
        /// </summary>
        public HTMLHelper HTML
        {
            get
            {
                if (_htmlHelper == null)
                {
                    _htmlHelper = new HTMLHelper(ViewContext);
                }
                return _htmlHelper;
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
        internal ViewPageBae ViewPage
        {
            get
            {
                ViewPageBae viewPage = this.Page as ViewPageBae;
                if (viewPage == null)
                {
                    throw new InvalidOperationException("这个ViewUserControl 必须用于继承至ViewPageBae 或 ViewPageBae<TModel> 的页面");
                }
                return viewPage;
            }
        }

        /// <summary>
        /// 视图上下文
        /// </summary>
        public virtual ViewContext ViewContext { get; set; }
        /// <summary>
        /// 呈现视图
        /// </summary>
        /// <param name="viewContext"></param>
        public virtual void RenderView(ViewContext viewContext)
        {
            using (ViewUserControl.ViewUserControlContainerPage viewUserControlContainerPage = new ViewUserControl.ViewUserControlContainerPage(this))
            {
                string contentType = viewContext.HttpContext.Response.ContentType;
                viewUserControlContainerPage.RenderView(viewContext);
                viewContext.HttpContext.Response.ContentType = contentType;
            }
        }

        /// <summary>
        /// 内部包裹UserControl的Page页面
        /// </summary>
        sealed class ViewUserControlContainerPage : ViewPageBae
        {
            private readonly ViewUserControl _userControl;
            public ViewUserControlContainerPage(ViewUserControl userControl)
            {
                this._userControl = userControl;
            }
            public override void ProcessRequest(HttpContext context)
            {
                this._userControl.ID = ViewPageBae.NextId();
                this.Controls.Add(this._userControl);
                base.ProcessRequest(context);
            }
        }
    }

    internal sealed class ViewUserControlControlBuilder : FileLevelUserControlBuilder
    {
        internal string UserControlBaseType
        {
            get;
            set;
        }

        public override void ProcessGeneratedCode(
            CodeCompileUnit codeCompileUnit,
            CodeTypeDeclaration baseType,
            CodeTypeDeclaration derivedType,
            CodeMemberMethod buildMethod,
            CodeMemberMethod dataBindingMethod)
        {
            // 如果分析器找到一个有效的类型，就使用它。
            if (!string.IsNullOrEmpty(UserControlBaseType))
            {
                derivedType.BaseTypes[0] = new CodeTypeReference(UserControlBaseType);
            }
        }
    }
}
