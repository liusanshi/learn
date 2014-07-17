namespace LL.Framework.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;
    using Expression_ = System.Linq.Expressions.Expression;
    using LL.Framework.Core.Domain;
    using LL.Framework.Core.Domain.ExpressionVisitors;

    /// <summary>
    /// Base class for composite specifications
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification</typeparam>
    public abstract class CompositeSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        #region Properties

        /// <summary>
        /// Left side specification for this composite element
        /// </summary>
        public abstract Specification<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// Right side specification for this composite element
        /// </summary>
        public abstract Specification<TEntity> RightSideSpecification { get; }

        #endregion

        #region override

        protected override System.Linq.Expressions.Expression<System.Func<TEntity, bool>> CreateExpression()
        {
            var parameter = Expression_.Parameter(typeof(TEntity), "entity");
            var replacer = new ParameterVisitor(node => parameter);
            var leftBody = replacer.Visit(LeftSideSpecification.Expression.Body);
            var rightBody = replacer.Visit(RightSideSpecification.Expression.Body);
            var newBody = CreateBody(leftBody, rightBody);
            return Expression_.Lambda<Func<TEntity, bool>>(newBody, parameter);
        }

        /// <summary>
        /// 根据两个 Specification 生成二元运算
        /// </summary>
        /// <param name="left">左运算符</param>
        /// <param name="right">右运算符</param>
        /// <returns>返回运算表达式</returns>
        protected abstract BinaryExpression CreateBody(Expression left, Expression right);
        #endregion
    }
}
