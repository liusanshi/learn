namespace LL.FrameWork.Implementation.Infrastructure.Adapter
{
    using System;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using EmitMapper.MappingConfiguration.MappingOperations;
    using EmitMapper.MappingConfiguration;
    using EmitMapper;
    using LL.FrameWork.Core.Domain.Specification;
    using LL.FrameWork.Core.Reflection;

    public class FlatteringConfig : DefaultMapConfig
    {

        protected Specification<Tuple<string, string>> nestedMembersMatcher;

        public FlatteringConfig()
        {
            nestedMembersMatcher = new FalseSpecification<Tuple<string, string>>();
        }

        protected override bool MatchMembers(string m1, string m2)
        {
            var oldval = base.MatchMembers(m1, m2);

            return oldval || nestedMembersMatcher.IsSatisfiedBy(Tuple.Create(m1, m2));
        }

        /// <summary>
        /// 根据名称来
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public DefaultMapConfig ForMember<TFrom, TTo>(Expression<Func<TTo, object>> destinationMember, Expression<Func<TTo, object>> sourceMember)
        {
            return ForMember<TFrom, TTo>(ReflectionHelper.ExtractPropertyInfo(destinationMember).Name,
                ReflectionHelper.ExtractPropertyInfo(sourceMember).Name);
        }

        /// <summary>
        /// 根据名称来匹配
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="destName"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public DefaultMapConfig ForMember<TFrom, TTo>(string destName, string sourceName)
        {
            Specification<Tuple<string, string>> membersmatcher =
                new DirectSpecification<Tuple<string, string>>(tupl => sourceName == tupl.Item1 && tupl.Item2 == destName);

            nestedMembersMatcher |= membersmatcher;

            return this;
        }
    }
}
