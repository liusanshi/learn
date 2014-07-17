using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LL.Framework.Core.Domain.Expanders;

namespace LL.Framework.Core.Domain.Viewpoints
{
    /// <summary>
    /// 视点基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TValue">视点对应的值的类型</typeparam>
    /// <seealso cref="Order&lt;TEntity&gt;"/>
    /// <seealso cref="IExpandable&lt;TEntity&gt;"/>
    /// <seealso cref="Specifications.Specification&lt;TEntity&gt;"/>
    [Serializable]
    public abstract class Viewpoint<TEntity, TValue>
        where TEntity : class
    {
        /// <summary>
        /// <see cref="Viewpoint&lt;TEntity, TValue&gt;"/> 创建新的实例
        /// </summary>
        public Viewpoint()
        {
            _expression = null;
            _delegate = null;
        }


        /// <summary>
        /// 表达式树
        /// </summary>
        private Expression<Func<TEntity, TValue>> _expression;

        /// <summary>
        /// 表达式生成的委托
        /// </summary>
        private Func<TEntity, TValue> _delegate;


        /// <summary>
        /// 获取表达式树
        /// </summary>
        public Expression<Func<TEntity, TValue>> Expression
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
        /// 获取表达式生成的委托
        /// </summary>
        public Func<TEntity, TValue> Delegate
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
        /// 生成表达式树
        /// </summary>
        /// <returns></returns>
        protected abstract Expression<Func<TEntity, TValue>> CreateExpression();

        /// <summary>
        /// 获取扩展点
        /// 默认返回 null
        /// </summary>
        /// <returns>返回对象的扩展</returns>
        protected virtual Expander<TEntity> GetExpander()
        {
            return null;
        }

        /// <summary>
        /// 创建扩展表达式树
        /// </summary>
        /// <returns>返回表达式树</returns>
        private Expression<Func<TEntity, TValue>> CreateExpandedExpression()
        {
            var source = CreateExpression();
            var expander = GetExpander();
            return (expander != null) ? expander.Expand(source) : source;
        }


        /// <summary>
        /// 分析视点
        /// </summary>
        /// <param name="target">实体对象</param>
        /// <returns>返回值</returns>
        public TValue Analyze(TEntity target)
        {
            return Delegate(target);
        }


        /// <summary>
        /// 在视点的值上面进行升序
        /// </summary>
        /// <returns>返回排序的描述</returns>
        public SimpleOrder<TEntity, TValue> Asc()
        {
            return new SimpleOrder<TEntity, TValue>(this, Direction.Ascending);
        }

        /// <summary>
        /// 在视点的值上面进行降序
        /// </summary>
        /// <returns>返回排序的描述</returns>
        public SimpleOrder<TEntity, TValue> Desc()
        {
            return new SimpleOrder<TEntity, TValue>(this, Direction.Descending);
        }
    }
}
