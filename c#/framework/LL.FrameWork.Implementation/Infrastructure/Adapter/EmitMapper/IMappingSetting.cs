using System;

using LL.FrameWork.Core.Infrastructure.Adapter;
using EmitMapper;

namespace LL.FrameWork.Impl.Infrastructure.Adapter.EmitMapperImpl
{
    public interface IMappingSetting
    {
        TypeMapIdentity GetIdentity();

        IMappingConfigurator GetObjectsMapper();
    }
}
