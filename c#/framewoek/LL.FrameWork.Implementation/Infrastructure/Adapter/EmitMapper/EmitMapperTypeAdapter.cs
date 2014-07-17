namespace LL.Framework.Impl.Infrastructure.Adapter.EmitMapperImpl
{
    using System;
    
    using LL.Framework.Core.Infrastructure.Adapter;

    public class EmitMapperTypeAdapter : ITypeAdapter
    {
        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            if (source == null) return (TTarget)null;
            return EmitMapHelper.GetObjectsMapper<TSource, TTarget>().Map(source);
        }

        public TTarget Adapt<TTarget>(object source) where TTarget : class, new()
        {
            if (source == null) return (TTarget)null;
            return (TTarget)(EmitMapHelper.GetObjectsMapperImpl<TTarget>(source.GetType()).Map(source));
        }
    }
}
