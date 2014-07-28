namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 动作执行的结果
    /// 是动作执行到最终http输出的中间过程
    /// </summary>
    public abstract class ActionResult
    {
        public abstract void ExecuteResult(ControllerContext context);
    }
}
