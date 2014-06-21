using System;

using LL.FrameWork.Core.Infrastructure.Adapter;
using EmitMapper;

namespace LL.FrameWork.Implementation.Infrastructure.Adapter
{
    public interface IMappingSetting
    {
        TypeMapIdentity GetIdentity();

        IMappingConfigurator GetObjectsMapper();
    }
}
