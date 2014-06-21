using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LL.FrameWork.Core.Infrastructure.Adapter;
using LL.FrameWork.Implementation.Infrastructure.Adapter;

namespace LL.FrameWork.Implementation.Test.AdapterTest
{
    [TestClass]
    public class MappingTest
    {
        [TestInitialize]
        public void start()
        {
            TypeAdapterFactory.SetCurrent(new EmitMapperTypeAdapterFactory());
        }

        [TestMethod]
        public void Adapter_test()
        {
            var D = new Inner(){D1 = 1L, D2 = Guid.Empty};
            Sourse source = new Sourse(){
            A = 1,
            B = 1m,
            C = "2011-05-05",
            D = D,
            E = "efg"
            };
            var dest = TypeAdapterFactory.CreateAdapter().Adapt<Sourse, Dest>(source);

            Assert.AreEqual(source.A, dest.A);
            Assert.AreEqual(source.B, dest.B);
            Assert.AreNotEqual(source.C, dest.C);
            Assert.AreEqual(source.D.D1, dest.D.D12);
            Assert.IsNull(dest.F);

        }
    }
}
