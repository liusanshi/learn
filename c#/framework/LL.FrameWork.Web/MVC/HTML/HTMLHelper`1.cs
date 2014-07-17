using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Web.MVC
{
    public class HTMLHelper<TModel> : HTMLHelper
    {
        public HTMLHelper(ViewContext context, TModel model)
            : base(context)
        {
            _model = model;
        }

        private TModel _model;

        /// <summary>
        /// 用于页面呈现时所需的数据实体对象
        /// </summary>
        public TModel Model
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
