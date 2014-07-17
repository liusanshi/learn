namespace LL.Framework.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// False specification
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification</typeparam>
    public sealed class FalseSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        #region Specification overrides

        protected override Expression<Func<TEntity, bool>> CreateExpression()
        {
            bool result = false;
            return entity => result;
        }

        #endregion
    }
}
