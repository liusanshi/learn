namespace LL.Framework.Web.MVC
{
    using System;

    /// <summary>
    /// 元素呈现的方式
    /// </summary>
    public enum TagRenderMode
    {
        /// <summary>
        /// 一般
        /// </summary>
        Normal,
        /// <summary>
        /// 起始
        /// </summary>
        StartTag,
        /// <summary>
        /// 结束
        /// </summary>
        EndTag,
        /// <summary>
        /// 自结束
        /// </summary>
        SelfClosing
    }
}
