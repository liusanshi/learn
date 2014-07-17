using System;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 基于用户控件的视图基类，这类用户控件只是用于呈现数据。
    /// </summary>
    /// <typeparam name="TModel">传递给用户控件呈现时所需的数据实体对象类型</typeparam>
    public class ViewUserControlBase<TModel> : ViewUserControlBase
    {
        private HTMLHelper<TModel> _htmlhelper;
        /// <summary>
        /// html帮助对象
        /// </summary>
        public new HTMLHelper<TModel> HTML
        {
            get { return _htmlhelper; }
        }
        /// <summary>
        /// 用于页面呈现时所需的数据实体对象
        /// </summary>
        public new TModel Model
        {
            get
            {
                try
                {
                    return (TModel)ViewContext.Model;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("参数model与目标类型不匹配。", ex);
                }
            }
            set { ViewContext.Model = value; }
        }

        protected override void InitHelpers()
        {
            base.InitHelpers();
            _htmlhelper = new HTMLHelper<TModel>(ViewContext, Model);
        }
    }
}
