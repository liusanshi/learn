using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LL.FrameWork.Core.Infrastructure.Adapter;
using LL.FrameWork.Impl.Infrastructure.Adapter.EmitMapperImpl;
using LL.FrameWork.Impl.Infrastructure.Adapter.AutoMapperImpl;
using LL.FrameWork.Impl.Infrastructure.IOC;
using EmitMapper.MappingConfiguration.MappingOperations;

namespace LL.FrameWork.Impl.Test.AdapterTest
{
    [TestClass]
    public class MappingTest
    {
        [TestInitialize]
        public void start()
        {
            //var container = Container.Current;
            //ITypeAdapterFactory adapt = Container.Current.Resolve<ITypeAdapterFactory>();
        }

        [TestMethod]
        public void Adapter_test()
        {
            var D = new Inner() { D1 = 1L, D2 = Guid.Empty };
            Sourse source = new Sourse()
            {
                A = 1,
                B = 1m,
                C = "2011-05-05",
                D = D,
                E = "efg"
            };
            var dest = Container.Current.Resolve<ITypeAdapterFactory>("EmitMapper").Create().Adapt<Sourse, Dest>(source);

            var source2 = TypeAdapterFactory.CreateAdapter().Adapt<Dest, Sourse>(dest);

            Assert.AreEqual(source.A, dest.A);
            Assert.AreEqual(source.B, dest.B);
            Assert.AreNotEqual(source.C, dest.C);
            Assert.AreEqual(source.D.D1, dest.DD1);
            Assert.AreEqual(source.E, dest.F);

            //ReadWriteSimple
            var members = LL.FrameWork.Core.Reflection.ReflectionHelper.FindMembers(((Expression<Func<ReadWriteSimple, object>>)(rw => rw.Destination.MemberInfo.Name.Length)));

            Assert.IsNotNull(members);
            Assert.AreEqual(4, members.Length);
            Assert.AreEqual("Destination", members[0].Name);
            Assert.AreEqual("MemberInfo", members[1].Name);
            Assert.AreEqual("Name", members[2].Name);
            Assert.AreEqual("Length", members[3].Name);
        }
    }
}
