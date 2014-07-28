using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using LL.Framework.Web.MVC;

namespace WebTest
{
    public class HomeController : ControllerBase
    {
        [Action]
        [PageUrl("/BigPipeDemo.aspx")]
        [OutputCache()]
        public void ShowHomePage()
        {
            // 先输出页框架
            this.WritePage(null /* pageVirtualPath */, null /* model */, true /* flush */);


            BlogBLL bll = new BlogBLL();

            // 加载博客内容，第一个数据
            string blogFilePath = Path.Combine(HttpContextHelper.AppRootPath, "App_Data\\BlogBody.txt");
            this.WriteCacheUserControl("~/UserControls/BlogBody.ascx", () => bll.GetBlog(blogFilePath), "blog-body", true);

            // 加载左链接导航栏，第二个数据
            string linksFilePath = Path.Combine(HttpContextHelper.AppRootPath, "App_Data\\Links.txt");
            this.WriteCacheUserControl("~/UserControls/TagLinks.ascx", () => bll.GetLinks(linksFilePath), "right", true);

            // 加载评论，第三个数据
            string commentFilePath = Path.Combine(HttpContextHelper.AppRootPath, "App_Data\\Comments.txt");
            this.WriteCacheUserControl("~/UserControls/CommentList.ascx", () => bll.GetComments(commentFilePath), "blog-comments-placeholder", true);


            this.WriteUserControl("~/UserControls/PageEnd.ascx", null /* model */, true /* flush */);



            //return View("~/UserControls/CommentList.ascx", bll.GetComments(commentFilePath));
        }
    }
}
