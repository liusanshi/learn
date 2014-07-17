namespace LL.FrameWork.Core.Domain.Specification
{
    using System;
    using System.Linq.Expressions;

    using LL.FrameWork.Core.Domain;
    using LL.FrameWork.Core.Domain.Expanders;


    /// <summary>
    /// Represent a Expression Specification
    /// <remarks>
    /// Specification overload operators for create AND,OR or NOT specifications.
    /// Additionally overload AND and OR operators with the same sense of ( binary And and binary Or ).
    /// C# couldn’t overload the AND and OR operators directly since the framework doesn’t allow such craziness. But
    /// with overloading false and true operators this is posible. For explain this behavior please read
    /// http://msdn.microsoft.com/en-us/library/aa691312(VS.71).aspx
    /// </remarks>
    /// </summary>
    /// <typeparam name="TValueObject">Type of item in the criteria</typeparam>
    public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        /// <summary>
        /// 常量 TrueSpecification
        /// </summary>
        public static readonly Specification<TEntity> True = new TrueSpecification<TEntity>();

        /// <summary>
        /// 常量 FalseSpecification
        /// </summary>
        public static readonly Specification<TEntity> False = new FalseSpecification<TEntity>();

        #region 组装表达式树

        /// <summary>
        /// 描述 Specification 的表达式树
        /// </summary>
        private Expression<Func<TEntity, bool>> _expression;

        /// <summary>
        /// 表达数树生成的委托
        /// </summary>
        private Func<TEntity, bool> _delegate;


        /// <summary>
        /// 获取描述 Specification 的表达式树
        /// </summary>
        public Expression<Func<TEntity, bool>> Expression
        {
            get
            {
                if (_expression == null)
                {
                    _expression = CreateExpandedExpression();
                }
                return _expression;
            }
        }

        /// <summary>
        /// 获取表达数树生成的委托
        /// </summary>
        public Func<TEntity, bool> Delegate
        {
            get
            {
                if (_delegate == null)
                {
                    _delegate = Expression.Compile();
                }
                return _delegate;
            }
        }


        /// <summary>
        /// 判断指定的对象是否符合 Specification
        /// </summary>
        /// <param name="target">实体对象</param>
        /// <returns>判断指定的对象是否符合 Specification，满足返回 true、否则false。</returns>
        public bool IsSatisfiedBy(TEntity target)
        {
            return Delegate(target);
        }


        /// <summary>
        /// 生成表达式树
        /// </summary>
        /// <returns>表达式树</returns>
        protected abstract Expression<Func<TEntity, bool>> CreateExpression();

        /// <summary>
        /// 获取扩展点
        /// 默认返回null
        /// </summary>
        /// <returns>返回扩展点</returns>
        protected virtual Expander<TEntity> GetExpander()
        {
            return null;
        }

        /// <summary>
        /// 创建扩展表达式树
        /// </summary>
        /// <returns>返回表达式树</returns>
        private Expression<Func<TEntity, bool>> CreateExpandedExpression()
        {
            var source = CreateExpression();
            var expander = GetExpander();
            return (expander != null) ? expander.Expand(source) : source;
        }

        #endregion

        #region Override Operators

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this AND operation</param>
        /// <param name="rightSideSpecification">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }
       
        /// <summary>
        /// Or operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this OR operation</param>
        /// <param name="rightSideSpecification">left operand in this OR operation</param>
        /// <returns>New specification </returns>
        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }
        
        /// <summary>
        /// Not specification
        /// </summary>
        /// <param name="specification">Specification to negate</param>
        /// <returns>New specification</returns>
        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }
      
        /// <summary>
        /// Override operator false, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See False operator in C#</returns>
        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }
        
        /// <summary>
        /// Override operator True, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See True operator in C#</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specification")]
        public static bool operator true(Specification<TEntity> specification)
        {
            return false;
        }
      
        #endregion
    }
}

