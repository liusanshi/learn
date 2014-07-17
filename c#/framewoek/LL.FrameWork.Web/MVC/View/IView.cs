using System;
using System.IO;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 视图接口
    /// </summary>
    public interface IView
    {
        void Render(ViewContext viewContext, TextWriter writer);
    }
}
