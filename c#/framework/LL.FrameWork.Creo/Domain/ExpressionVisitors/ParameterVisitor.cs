using System;
using System.Linq.Expressions;

namespace LL.Framework.Core.Domain.ExpressionVisitors
{
    /// <summary>
    /// 表达式参数的访问者
    /// </summary>
    internal sealed class ParameterVisitor : ExpressionVisitor
    {
        /// <summary>
        /// <see cref="ParameterVisitor"/> 创建新的实例
        /// </summary>
        /// <param name="visitor"><see cref="ParameterExpression"/> 要访问的参数 </param>
        /// <exception cref="System.ArgumentNullException">visitor 为 null </exception>
        public ParameterVisitor(Func<ParameterExpression, Expression> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }
            _visitor = visitor;
        }


        /// <summary>
        /// <see cref="ParameterExpression"/> 要访问的参数
        /// </summary>
        private readonly Func<ParameterExpression, Expression> _visitor;


        /// <inheritdoc/>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _visitor(node);
        }
    }
}
