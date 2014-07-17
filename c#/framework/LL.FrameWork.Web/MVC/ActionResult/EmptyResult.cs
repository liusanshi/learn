using System;

namespace LL.Framework.Web.MVC
{
    public class EmptyResult : ActionResult
    {
        private EmptyResult() { }
        private static readonly EmptyResult _singleton = new EmptyResult();
        internal static EmptyResult Instance
        {
            get
            {
                return EmptyResult._singleton;
            }
        }
        public override void ExecuteResult(ControllerContext context)
        {
        }
    }
}
