using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LL.Framework.Core.Domain.Expanders
{
    public interface IExpandable<TEntity> where TEntity : class
    {
        /// <summary>
        /// 获取源的属性
        /// </summary>
        /// <returns></returns>
        PropertyInfo From();
        /// <summary>
        /// 获取表达式目录树的目的地
        /// </summary>
        /// <returns></returns>
        Expression To();
    }
}
