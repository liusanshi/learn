namespace LL.Framework.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// True specification
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification</typeparam>
    public sealed class TrueSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        #region Specification overrides

        protected override Expression<Func<TEntity, bool>> CreateExpression()
        {
            bool result = true;
            return entity => result;
        }

        #endregion
    }
}
