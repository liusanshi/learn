using System;
using System.IO;
using System.Reflection;
using System.Web.UI;

using LL.FrameWork.Core.Reflection;
using System.Web;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 用于呈现（Render）用户控件的执行器
    /// </summary>
    public static class UcExecutor
    {
        /// <summary>
        /// Page页面的
        /// </summary>
        private static FieldInfo Page_request = typeof(Page).GetField("_request", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// 用指定的用户控件以及视图数据呈现结果，最后返回生成的HTML代码。
        /// 用户控件应从MyUserControlView&lt;T&gt;继承
        /// </summary>
        /// <param name="ucVirtualPath">用户控件的虚拟路径</param>
        /// <param name="model">视图数据</param>
        /// <returns>生成的HTML代码</returns>
        public static string Render(string ucVirtualPath, object model)
        {
            if (string.IsNullOrEmpty(ucVirtualPath))
                throw new ArgumentNullException("ucVirtualPath");

            Page page = new Page();
            Control ctl = page.LoadControl(ucVirtualPath);
            if (ctl == null)
                throw new InvalidOperationException(
                    string.Format("指定的用户控件 {0} 没有找到。", ucVirtualPath));

            // 将用户控件放在Page容器中。
            page.Controls.Add(ctl);

            if (model != null)
            {
                PartialCachingControl mycachectl = ctl as PartialCachingControl;//如果有 写outputcache指令
                Control temp = ctl;
                if (mycachectl != null)
                {
                    Page_request.FastSetValue(page, HttpContext.Current.Request);//将当前的Request 写入当前页面
                    page.DesignerInitialize();
                    temp = mycachectl.CachedControl;
                }
                MyBaseUserControl myctl = temp as MyBaseUserControl;
                if (myctl != null)
                    myctl.SetModel(model);
            }

            StringWriter output = new StringWriter();
            HtmlTextWriter write = new HtmlTextWriter(output, string.Empty);
            page.RenderControl(write);

            // 用下面的方法也可以的。目前是不行的
            //HttpContext.Current.Server.Execute(page, output, false);

            return output.ToString();
        }
    }
}
