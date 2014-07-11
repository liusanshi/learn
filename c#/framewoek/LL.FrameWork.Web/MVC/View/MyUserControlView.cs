using System;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 基于用户控件的视图基类，这类用户控件只是用于呈现数据。
    /// </summary>
    /// <typeparam name="TModel">传递给用户控件呈现时所需的数据实体对象类型</typeparam>
    public class MyUserControlView<TModel> : ViewUserControl
    {
        /// <summary>
        /// 用于页面呈现时所需的数据实体对象
        /// </summary>
        public TModel Model { get; set; }
        private ViewContext _viewContext;

        /// <summary>
        /// 视图上下文
        /// </summary>
        public override ViewContext ViewContext
        {
            get
            {
                return _viewContext;
            }
            set
            {
                _viewContext = value;
                try
                {
                    this.Model = (TModel)value.Model;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("参数model与目标类型不匹配。", ex);
                }
            }
        }
    }
}
