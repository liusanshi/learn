using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LL.FrameWork.Core.Infrastructure.Adapter;

namespace LL.Core.Test
{
    [TestClass]
    public class TypeMapIdentityTest
    {
        [TestMethod]
        public void TypeMapIdentity_object_int()
        {
            TypeMapIdentity identity1 = TypeMapIdentity.GetIdentity<int, int>();
            TypeMapIdentity identity2 = TypeMapIdentity.GetIdentity<int, int>();
            object identity3 = new object();

            Assert.IsTrue(identity1.Equals(identity2));

            Assert.IsTrue(identity1 == identity2);
            Assert.IsFalse(identity1 != identity2);

            Assert.IsFalse(identity1 == identity3);
            Assert.IsFalse(identity3 == identity1);
            Assert.IsTrue(identity1 != identity3);
            Assert.IsTrue(identity3 != identity1);

            Assert.IsFalse(identity1 == null);
            Assert.IsFalse(null == identity1);

            Assert.IsFalse(identity1 != identity2);
        }

        [TestMethod]
        public void TypeMapIdentity_object_object()
        {
            TypeMapIdentity identity1 = TypeMapIdentity.GetIdentity<int, int>();
            TypeMapIdentity identity2 = TypeMapIdentity.GetIdentity<object, object>();

            Assert.IsFalse(identity1.Equals(identity2));

            Assert.IsFalse(identity1 == (object)identity2);
            Assert.IsTrue(identity1 != (object)identity2);
        }

        [TestMethod]
        public void TypeMapIdentity_hashcode()
        {
            TypeMapIdentity identity1 = TypeMapIdentity.GetIdentity<int, int>();
            TypeMapIdentity identity2 = TypeMapIdentity.GetIdentity<int, int>();

            Assert.IsTrue(identity1.GetHashCode() == identity2.GetHashCode());
        }

        [TestMethod]
        public void TypeMapIdentity_hashcode2()
        {
            TypeMapIdentity identity1 = TypeMapIdentity.GetIdentity<int, int>();
            TypeMapIdentity identity2 = TypeMapIdentity.GetIdentity<object, int>();

            Assert.IsFalse(identity1.GetHashCode() == identity2.GetHashCode());
        }
    }
}
