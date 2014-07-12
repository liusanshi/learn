using System;
using System.Web.UI;
using System.Globalization;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 一个基于“System.Web.UI.Page”的类
    /// </summary>
    [FileLevelControlBuilder(typeof(ViewPageControlBuilder))]
    public class ViewPageBase : Page
    {
        [ThreadStatic]
        private static int _nextId;
        private string _requestUrlEncodeRawUrl;
        private HTMLHelper<object> _htmlHelper;
        /// <summary>
        /// 获取序号
        /// </summary>
        /// <returns></returns>
        internal static string NextId()
        {
            int num = ++ViewPageBase._nextId;
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
            get;
            private set;
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
        /// 处理请求
        /// </summary>
        /// <param name="context"></param>
        public override void ProcessRequest(HttpContext context)
        {
            this.ID = ViewPageBase.NextId();
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
            this.InitHelpers();
            var switchWriter = viewContext.Writer;
            if (switchWriter != null)
            {
                using (switchWriter)
                {
                    int nextId = ViewPageBase._nextId;
                    try
                    {
                        ViewPageBase._nextId = 0;
                        viewContext.HttpContext.Server.Execute(this, switchWriter, true);
                    }
                    finally
                    {
                        ViewPageBase._nextId = nextId;
                    }
                }
            }
            else
            {
                this.ProcessRequest(viewContext.HttpContext);
            }
        }
    }
}
