using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using EmitMapper.Utils;
using LL.FrameWork.Implementation.Infrastructure.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Implementation.Test.AdapterTest
{
    class MyEntity
    {
        public int Id { get; set; }
    }

    public class Sourse
    {
        public int A;
        public decimal? B;
        public string C;
        public Inner D;
        public string E;
    }

    public class Dest
    {
        public int? A;
        public decimal B;
        public DateTime C;
        public Inner2 D;
        public string F;
    }

    public class Inner
    {
        public long D1;
        public Guid D2;
    }

    public class Inner2
    {
        public long D12;
        public Guid D22;
    }

    public class MyMappingSetting : IMappingSetting
    {
        public Core.Infrastructure.Adapter.TypeMapIdentity GetIdentity()
        {
            return Core.Infrastructure.Adapter.TypeMapIdentity.GetIdentity<Sourse, Dest>();
        }

        IMappingConfigurator IMappingSetting.GetObjectsMapper()
        {
            return new DefaultMapConfig()
                .ConvertUsing<Sourse, Dest>(inner => new Dest()
                {
                    A = inner.A,
                    B = inner.B.Value,
                    C = Convert.ToDateTime(inner.C),
                    //D = inner.D,
                })
                .ConstructBy<Dest>(() => new Dest())
                .IgnoreMembers<Sourse, Dest>(new string[] { "E", "F" });

            //return new CustomMapConfig()
            //{
            //    ConfigurationName = "11",
            //    GetMappingOperationFunc = (from, to) =>
            //    {
            //        var members = ReflectionUtils.GetPublicFieldsAndProperties(to);



            //        return null;
            //    }
            //};
        }
    }
}
