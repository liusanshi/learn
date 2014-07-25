namespace LL.Framework.Web.MVC
{
    using System;

    /// <summary>
    /// 允许匿名访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnonymousAttribute : Attribute
    {

    }
}
