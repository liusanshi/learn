using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;

using LL.Framework.Core.Reflection;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 带OutputCache指令 的 UserControl 视图 
    /// </summary>
    public class CacheUserControlView : IView
    {
        /// <summary>
        /// Page页面的是有变量
        /// </summary>
        private static FieldInfo Page_request = typeof(Page).GetField("_request", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// 创建 CachingUserControlView 对象
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewPath"></param>
        public CacheUserControlView(ControllerContext controllerContext, string viewPath)
        {
            ViewPath = viewPath;
            _controllerContext = controllerContext;
        }

        /// <summary>
        /// 视图路径
        /// </summary>
        public string ViewPath
        {
            get;
            protected set;
        }
        private ControllerContext _controllerContext;

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            Page page = new Page();
            Control ctl = page.LoadControl(ViewPath);
            if (ctl == null)
                throw new InvalidOperationException(string.Format("指定的用户控件 {0} 没有找到。", ViewPath));

            // 将用户控件放在Page容器中。
            page.Controls.Add(ctl);
            PartialCachingControl mycachectl = ctl as PartialCachingControl;//如果有 写outputcache指令
            Control temp = ctl;
            if (mycachectl != null)
            {
                Page_request.FastSetValue(page, viewContext.HttpContext.Request);//将当前的Request 写入当前页面
                page.DesignerInitialize();
                temp = mycachectl.CachedControl;
            }
            ViewUserControlBase myctl = temp as ViewUserControlBase;
            if (myctl != null)
                myctl.ViewContext = viewContext;

            HtmlTextWriter write = new HtmlTextWriter(writer, string.Empty);
            page.RenderControl(write);
        }
    }
}
