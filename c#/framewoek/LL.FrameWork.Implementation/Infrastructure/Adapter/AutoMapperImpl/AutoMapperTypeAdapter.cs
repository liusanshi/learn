namespace LL.Framework.Impl.Infrastructure.Adapter.AutoMapperImpl
{
    using System;

    using AutoMapper;
    using LL.Framework.Core.Infrastructure.Adapter;

    public class AutoMapperTypeAdapter : ITypeAdapter
    {
        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            if (source == null) return (TTarget)null;
            return Mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adapt<TTarget>(object source) where TTarget : class, new()
        {
            if (source == null) return (TTarget)null;
            return Mapper.Map<TTarget>(source);
        }
    }
}
