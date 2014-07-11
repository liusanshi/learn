using System;
using System.Web.UI;
using System.CodeDom;
using System.Globalization;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 一个基于“System.Web.UI.Page”的类
    /// </summary>
    [FileLevelControlBuilder(typeof(ViewPageControlBuilder))]
    public class ViewPageBae : Page
    {
        [ThreadStatic]
        private static int _nextId;
        private string _requestUrlEncodeRawUrl;
        private HTMLHelper _htmlHelper;
        /// <summary>
        /// 获取序号
        /// </summary>
        /// <returns></returns>
        internal static string NextId()
        {
            int num = ++ViewPageBae._nextId;
            return num.ToString(CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// 当前页面的请求地址。
        /// 已经过UrlEncode()处理，用于构造URL中的一部分。
        /// </summary>
        public string RequestUrlEncodeRawUrl
        {
            get
            {
                if (_requestUrlEncodeRawUrl == null)
                    _requestUrlEncodeRawUrl = Server.UrlEncode(HttpContextHelper.RequestRawUrl);
                return _requestUrlEncodeRawUrl;
            }
        }

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
            get;
            private set;
        }
        /// <summary>
        /// 视图上下文
        /// </summary>
        public virtual ViewContext ViewContext { get; set; }
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            this.ID = ViewPageBae.NextId();
            base.ProcessRequest(context);
        }
        /// <summary>
        /// 呈现页面
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.Writer = writer;
            try
            {
                base.Render(writer);
            }
            finally
            {
                this.Writer = null;
            }
        }
        /// <summary>
        /// 呈现视图
        /// </summary>
        /// <param name="viewContext"></param>
        public virtual void RenderView(ViewContext viewContext)
        {
            this.ViewContext = viewContext;
            var switchWriter = viewContext.Writer;
            if (switchWriter != null)
            {
                using (switchWriter)
                {
                    int nextId = ViewPageBae._nextId;
                    try
                    {
                        ViewPageBae._nextId = 0;
                        viewContext.HttpContext.Server.Execute(this, switchWriter, true);
                    }
                    finally
                    {
                        ViewPageBae._nextId = nextId;
                    }
                }
            }
            else
            {
                this.ProcessRequest(viewContext.HttpContext);
            }
        }
    }


    internal sealed class ViewPageControlBuilder : FileLevelPageControlBuilder
    {
        public string PageBaseType
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
            if (!string.IsNullOrEmpty(PageBaseType))
            {
                derivedType.BaseTypes[0] = new CodeTypeReference(PageBaseType);
            }
        }
    }
}
