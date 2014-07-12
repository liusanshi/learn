using System;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 母版页基类
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ViewMasterPage<TModel> : ViewMasterPage
    {
        private HTMLHelper<TModel> _htmlhelper;
        /// <summary>
        /// html帮助对象
        /// </summary>
        public new HTMLHelper<TModel> HTML
        {
            get
            {
                if (this._htmlhelper == null)
                {
                    this._htmlhelper = new HTMLHelper<TModel>(ViewContext, Model);
                }
                return this._htmlhelper;
            }
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
    }
}
