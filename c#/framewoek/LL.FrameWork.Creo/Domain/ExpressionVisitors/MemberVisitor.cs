using System;
using System.Linq.Expressions;

namespace LL.Framework.Core.Domain.ExpressionVisitors
{
    /// <summary>
    /// 表达式树的成员访问者
    /// </summary>
    internal class MemberVisitor : ExpressionVisitor
    {
        /// <summary>
        /// <see cref="MemberVisitor"/> 创建新的实例
        /// </summary>
        /// <param name="visitor"><see cref="MemberExpression"/> 要访问的成员 </param>
        /// <exception cref="System.ArgumentNullException">visitor 为 null</exception>
        public MemberVisitor(Func<MemberExpression, Expression> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }
            _visitor = visitor;
        }


        /// <summary>
        /// <see cref="MemberExpression"/> 要访问的成员
        /// </summary>
        private readonly Func<MemberExpression, Expression> _visitor;


        /// <inheritdoc/>
        protected override Expression VisitMember(MemberExpression node)
        {
            return _visitor(node);
        }
    }
}
