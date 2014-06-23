namespace LL.FrameWork.Impl.Infrastructure.Adapter.EmitMapperImpl
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
        /// <summary>
        /// 扩展时使用的配置条件
        /// </summary>
        //protected Specification<Tuple<string, string>> ExtendMembersMatcher;
        /// <summary>
        /// 内部嵌套时使用的匹配条件
        /// </summary>
        protected Func<string, string, bool> nestedMembersMatcher;

        private List<IMappingOperation> AppendMappingOperations = null;

        public FlatteringConfig()
        {
            //ExtendMembersMatcher = new FalseSpecification<Tuple<string, string>>();
            AppendMappingOperations = new List<IMappingOperation>();
            nestedMembersMatcher = (m1, m2) => m1.StartsWith(m2);
        }

        //protected override bool MatchMembers(string m1, string m2)
        //{
        //    var oldval = base.MatchMembers(m1, m2);

        //    return oldval || ExtendMembersMatcher.IsSatisfiedBy(Tuple.Create(m1, m2));
        //}

        public override IMappingOperation[] GetMappingOperations(Type from, Type to)
        {
            var mapoperations = base.GetMappingOperations(from, to);
            var listmapop = Enumerable.Empty<IMappingOperation>();
            if (mapoperations == null || mapoperations.Length == 0)
            {
                listmapop = GetFlatteringMappingOperations(from, to);
            }
            else
            {
                listmapop = mapoperations.Concat(GetFlatteringMappingOperations(from, to));
            }
            if (AppendMappingOperations.Count > 0)
                listmapop = listmapop.Concat(AppendMappingOperations);

            return listmapop.ToArray();
        }

        /// <summary>
        /// 使用指定的方式来填充目标
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public FlatteringConfig ResolveUsing<TFrom, TTo>(Expression<Func<TTo, object>> destinationMember, Func<TFrom, object> sourceMember)
        {
            AppendMappingOperations.Add(new DestWriteOperation()
            {
                Destination = new MemberDescriptor(ReflectionHelper.FindMember(destinationMember)),
                Getter = ((ValueGetter<object>)((from, state) =>
                {
                    return ValueToWrite<object>.ReturnValue(sourceMember((TFrom)from));
                }))
            });

            return this;
        }
        /// <summary>
        /// 使用指定的方式来填充源
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="sourceMember"></param>
        /// <param name="destinationMember"></param>
        /// <returns></returns>
        public FlatteringConfig ResolveSourceUsing<TFrom, TTo>(Expression<Func<TFrom, object>> sourceMember, Func<TTo, object> destinationMember)
        {
            AppendMappingOperations.Add(new DestWriteOperation()
            {
                Destination = new MemberDescriptor(ReflectionHelper.FindMember(sourceMember)),
                Getter = ((ValueGetter<object>)((to, state) =>
                {
                    return ValueToWrite<object>.ReturnValue(destinationMember((TTo)to));
                }))
            });

            return this;
        }

        /// <summary>
        /// 使用成员路径类配置对应关系
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public FlatteringConfig ForMember<TFrom, TTo>(Expression<Func<TTo, object>>[] destinationMember, Expression<Func<TFrom, object>>[] sourceMember)
        {
            AppendMappingOperations.Add(new ReadWriteSimple()
            {
                Source = new MemberDescriptor(ConvertToMemberInfo(sourceMember)),
                Destination = new MemberDescriptor(ConvertToMemberInfo(destinationMember))
            });

            return this;
        }
        /// <summary>
        /// 使用成员路径类配置对应关系
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public FlatteringConfig ForMember<TFrom, TTo>(Expression<Func<TTo, object>> destinationMember, Expression<Func<TFrom, object>>[] sourceMember)
        {
            return ForMember<TFrom, TTo>(new Expression<Func<TTo, object>>[] { destinationMember }, sourceMember);
        }
        /// <summary>
        /// 使用成员路径类配置对应关系
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public FlatteringConfig ForMember<TFrom, TTo>(Expression<Func<TTo, object>>[] destinationMember, Expression<Func<TFrom, object>> sourceMember)
        {
            return ForMember<TFrom, TTo>(destinationMember, new Expression<Func<TFrom, object>>[] { sourceMember });
        }

        /// <summary>
        /// 根据名称来匹配
        /// </summary>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public FlatteringConfig ForMember<TFrom, TTo>(Expression<Func<TTo, object>> destinationMember, Expression<Func<TFrom, object>> sourceMember)
        {
            AppendMappingOperations.Add(new ReadWriteSimple()
            {
                Destination = new MemberDescriptor(ReflectionHelper.FindMember(destinationMember)),
                Source = new MemberDescriptor(ReflectionHelper.FindMember(sourceMember))
            });

            return this;
        }

        #region 私有方法
        /// <summary>
        /// 将类型LambdaExpression[]转换为  MemberInfo[]
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static MemberInfo[] ConvertToMemberInfo(LambdaExpression[] expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            MemberInfo[] result = new MemberInfo[expression.Length];
            for (int i = 0; i < expression.Length; i++)
            {
                result[i] = ReflectionHelper.FindMember(expression[i]);
            }
            return result;
        }

        /// <summary>
        /// 获取 Flattering 配置类型
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private IEnumerable<IMappingOperation> GetFlatteringMappingOperations(Type from, Type to)
        {
            var destinationMembers = GetDestinationMemebers(to);
            var sourceMembers = GetSourceMemebers(from);
            var result = new List<IMappingOperation>();
            foreach (var dest in destinationMembers)
            {
                var matchedChain = GetMatchedChain(dest.Name, sourceMembers);
                if (matchedChain == null || !matchedChain.Any())
                {
                    continue;
                }
                result.Add(
                    new ReadWriteSimple
                    {
                        Source = new MemberDescriptor(matchedChain.ToArray()),
                        Destination = new MemberDescriptor(new[] { dest })
                    }
                );
            }
            return result;
        }
        /// <summary>
        /// 获取匹配的类型
        /// </summary>
        /// <param name="destName"></param>
        /// <param name="sourceMembers"></param>
        /// <returns></returns>
        private List<MemberInfo> GetMatchedChain(string destName, IEnumerable<MemberInfo> sourceMembers)
        {
            var matches = sourceMembers.Where(s => MatchMembers(destName, s.Name) || nestedMembersMatcher(destName, s.Name));
            int len = 0;
            MemberInfo match = null;
            foreach (var m in matches)
            {
                if (m.Name.Length > len)
                {
                    len = m.Name.Length;
                    match = m;
                }
            }
            if (match == null)
            {
                return null;
            }
            var result = new List<MemberInfo> { match };
            if (!MatchMembers(destName, match.Name))
            {
                var matchedChain = GetMatchedChain(destName.Substring(match.Name.Length), GetDestinationMemebers(match));
                if (matchedChain != null && matchedChain.Any())
                {
                    result.AddRange(matchedChain);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取源的数据类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static IEnumerable<MemberInfo> GetSourceMemebers(Type t)
        {
            return GetMemebers(t)
                .Where(
                    m =>
                        m.MemberType == MemberTypes.Field ||
                        m.MemberType == MemberTypes.Property ||
                        m.MemberType == MemberTypes.Method
                );
        }
        /// <summary>
        /// 根据成员元数据获取 对应类型的成员元数据
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        private static IEnumerable<MemberInfo> GetDestinationMemebers(MemberInfo mi)
        {
            Type t;
            if (mi.MemberType == MemberTypes.Field)
            {
                t = mi.DeclaringType.GetField(mi.Name).FieldType;
            }
            else
            {
                t = mi.DeclaringType.GetProperty(mi.Name).PropertyType;
            }
            return GetDestinationMemebers(t);
        }
        /// <summary>
        /// 根据类型获取成员信息(字段与属性)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static IEnumerable<MemberInfo> GetDestinationMemebers(Type t)
        {
            return GetMemebers(t).Where(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);
        }
        /// <summary>
        /// 根据类型获取成员信息(字段、属性、方法)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static IEnumerable<MemberInfo> GetMemebers(Type t)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            return t.GetMembers(bindingFlags).ToList();
        }
        #endregion
    }

    /// <summary>
    /// 将支持在 DefaultMapConfig 调用子类 FlatteringConfig 的方法
    /// </summary>
    public static class DefaultMapConfigExtend
    {
        /// <summary>
        /// 使用指定的方式类填充目标对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="mapconfig"></param>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public static FlatteringConfig ResolveUsing<TFrom, TTo>(this DefaultMapConfig mapconfig, Expression<Func<TTo, object>> destinationMember, Func<TFrom, object> sourceMember)
        {
            FlatteringConfig flattering = mapconfig as FlatteringConfig;
            if (flattering == null)
            {
                throw new NotSupportedException("当前对象不支持 ResolveUsing 方法");
            }
            return flattering.ResolveUsing<TFrom, TTo>(destinationMember, sourceMember);
        }
        /// <summary>
        /// 使用指定的方式类填充目标对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="mapconfig"></param>
        /// <param name="sourceMember"></param>
        /// <param name="destinationMember"></param>
        /// <returns></returns>
        public static FlatteringConfig ResolveSourceUsing<TFrom, TTo>(this DefaultMapConfig mapconfig, Expression<Func<TFrom, object>> sourceMember, Func<TTo, object> destinationMember)
        {
            FlatteringConfig flattering = mapconfig as FlatteringConfig;
            if (flattering == null)
            {
                throw new NotSupportedException("当前对象不支持 ResolveUsing 方法");
            }
            return flattering.ResolveSourceUsing<TFrom, TTo>(sourceMember, destinationMember);
        }
        /// <summary>
        /// 根据成员路径来填充对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="mapconfig"></param>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public static FlatteringConfig ForMember<TFrom, TTo>(this DefaultMapConfig mapconfig, Expression<Func<TTo, object>>[] destinationMember, Expression<Func<TFrom, object>>[] sourceMember)
        {
            FlatteringConfig flattering = mapconfig as FlatteringConfig;
            if (flattering == null)
            {
                throw new NotSupportedException("当前对象不支持 ForMember 方法");
            }
            return flattering.ForMember<TFrom, TTo>(destinationMember, sourceMember);
        }
        /// <summary>
        /// 根据成员路径来填充对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="mapconfig"></param>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public static FlatteringConfig ForMember<TFrom, TTo>(this DefaultMapConfig mapconfig, Expression<Func<TTo, object>> destinationMember, Expression<Func<TFrom, object>>[] sourceMember)
        {
            FlatteringConfig flattering = mapconfig as FlatteringConfig;
            if (flattering == null)
            {
                throw new NotSupportedException("当前对象不支持 ForMember 方法");
            }
            return flattering.ForMember<TFrom, TTo>(destinationMember, sourceMember);
        }
        /// <summary>
        /// 根据成员路径来填充对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="mapconfig"></param>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public static FlatteringConfig ForMember<TFrom, TTo>(this DefaultMapConfig mapconfig, Expression<Func<TTo, object>>[] destinationMember, Expression<Func<TFrom, object>> sourceMember)
        {
            FlatteringConfig flattering = mapconfig as FlatteringConfig;
            if (flattering == null)
            {
                throw new NotSupportedException("当前对象不支持 ForMember 方法");
            }
            return flattering.ForMember<TFrom, TTo>(destinationMember, sourceMember);
        }
        /// <summary>
        /// 一对一的指定对应关系
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="mapconfig"></param>
        /// <param name="destinationMember"></param>
        /// <param name="sourceMember"></param>
        /// <returns></returns>
        public static FlatteringConfig ForMember<TFrom, TTo>(this DefaultMapConfig mapconfig, Expression<Func<TTo, object>> destinationMember, Func<TFrom, object> sourceMember)
        {
            FlatteringConfig flattering = mapconfig as FlatteringConfig;
            if (flattering == null)
            {
                throw new NotSupportedException("当前对象不支持 ForMember 方法");
            }
            return flattering.ForMember<TFrom, TTo>(destinationMember, sourceMember);
        }
    }
}
