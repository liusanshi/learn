using System;

using LL.Framework.Core.Infrastructure.Adapter;
using EmitMapper;

namespace LL.Framework.Impl.Infrastructure.Adapter.EmitMapperImpl
{
    public interface IMappingSetting
    {
        TypeMapIdentity GetIdentity();

        IMappingConfigurator GetObjectsMapper();
    }
}
