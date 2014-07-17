using AutoMapper;
using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using EmitMapper.Utils;
using LL.Framework.Impl.Infrastructure.Adapter.EmitMapperImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LL.Framework.Impl.Test.AdapterTest
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

        //public long DD12;
        //public Guid DD22;
    }

    public class Dest
    {
        public int? A;
        public decimal B;
        public DateTime C;
        //public Inner2 D;
        public string F;

        public long DD1;
        public Guid DD2;
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

    public class MyProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Sourse, Dest>()
                .ForMember(dest => dest.F, ce => ce.MapFrom(source => source.E));

            Mapper.CreateMap<Dest, Sourse>()
                 .ForMember(dest => dest.E, ce => ce.MapFrom(source => source.F))
                 .ForMember(dest => dest.D, ce => ce.MapFrom<Inner>(source => new Inner() { D1 = source.DD1, D2 = source.DD2 }));
        }
    }

    public class MyMappingSetting : IMappingSetting
    {
        public Core.Infrastructure.Adapter.TypeMapIdentity GetIdentity()
        {
            return Core.Infrastructure.Adapter.TypeMapIdentity.GetIdentity<Sourse, Dest>();
        }

        IMappingConfigurator IMappingSetting.GetObjectsMapper()
        {
            return new FlatteringConfig()
                //.ForMember<Dest, Sourse>(new System.Linq.Expressions.LambdaExpression[] { (Expression<Func<Sourse, object>>)((Sourse source) => source.D), (Expression<Func<Inner, object>>)((Inner source) => source.D1), },
                //new System.Linq.Expressions.Expression<Func<Dest, object>>[] { dest => dest.DD1 })
                //.ForMember<Sourse, Dest>(, d => d.D)
                //.ForMember<Sourse, Dest>(s => s.D, d => d.D)
                //.ConvertUsing<Sourse, Dest>(inner => new Dest()
                //{
                //    A = inner.A,
                //    B = inner.B.Value,
                //    C = Convert.ToDateTime(inner.C),
                //    //D = inner.D,
                //})
                //.ConvertUsing<Inner, Inner2>(inner => new Inner2() { D12 = inner.D1, D22 = inner.D2 })
                .ConstructBy<Inner>(() => new Inner())
                .IgnoreMembers<Sourse, Dest>(new string[] { "E", "F" })
                .ResolveUsing<Sourse, Dest>(dest => dest.F, source => source.E)
                .ForMember<Sourse, Dest>(dest => dest.DD1, source => source.D.D1);
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
