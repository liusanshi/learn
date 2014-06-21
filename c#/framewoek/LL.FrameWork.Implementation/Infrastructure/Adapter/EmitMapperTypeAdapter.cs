namespace LL.FrameWork.Implementation.Infrastructure.Adapter
{
    using System;
    
    using LL.FrameWork.Core.Infrastructure.Adapter;

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
