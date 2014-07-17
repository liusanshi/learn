using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LL.FrameWork.Impl.UOW;
using LL.FrameWork.Impl.Test.DomainModel;
using LL.FrameWork.Impl.Infrastructure.IOC;
using Castle.MicroKernel.Registration;

namespace LL.FrameWork.Implementation.Test.DomainModel
{
    [TestClass]
    public class AutoSessionTest
    {
        [TestInitialize]
        public void Initializa()
        {
            Container.Current.Register(
                Component.For<AutoSessionAttribute>(),
                Component.For<IClassService>().ImplementedBy<ClassService>().Interceptors<AutoSessionAttribute>());
        }

        [TestMethod]
        public void TestMethod1()
        {
            var coll = Container.Current.Resolve<IClassService>().GetAllClass();

            Assert.AreEqual(1, coll.Count());

            Assert.AreEqual("中一班", coll.First().Name);
        }
    }

    public class ClassService : IClassService
    {
        //[AutoSession]
        public IEnumerable<Class> GetAllClass()
        {
            return UnitOfWork.CurrentSession.CreateCriteria<Class>().List<Class>();
        }
    }

    public interface IClassService
    {
        IEnumerable<Class> GetAllClass();
    }
}
