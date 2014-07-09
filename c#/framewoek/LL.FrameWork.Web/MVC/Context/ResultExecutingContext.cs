using System;

namespace LL.FrameWork.Web.MVC
{
    public class ResultExecutingContext : ControllerContext
    {
        public bool Cancel
        {
            get;
            set;
        }
        public virtual ActionResult Result
        {
            get;
            set;
        }
        public ResultExecutingContext()
        {
        }
        public ResultExecutingContext(ControllerContext controllerContext, ActionResult result)
            : base(controllerContext)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            this.Result = result;
        }
    }
}
