using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LL.FrameWork.Core.Infrastructure.Validator;
using LL.FrameWork.Impl.Infrastructure.Validator;

namespace LL.FrameWork.Implementation.Test.ValidatorsTests
{
    [TestClass]
    public class ValidatorsTests
    {
        #region Class Initialize

        [ClassInitialize()]
        public static void ClassInitialze(TestContext context)
        {
            // Initialize default log factory
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }

        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            var entityA = new EntityWithValidationAttribute();
            entityA.RequiredProperty = null;

            var entityB = new EntityWithValidatableObject();
            entityB.RequiredProperty = null;

            IEntityValidator entityValidator = EntityValidatorFactory.CreateValidator();

            //Act
            var entityAValidationResult = entityValidator.IsValid(entityA);
            var entityAInvalidMessages = entityValidator.GetInvalidMessages(entityA);

            var entityBValidationResult = entityValidator.IsValid(entityB);
            var entityBInvalidMessages = entityValidator.GetInvalidMessages(entityB);

            //Assert
            Assert.IsFalse(entityAValidationResult);
            Assert.IsFalse(entityBValidationResult);

            Assert.IsTrue(entityAInvalidMessages.Any());
            Assert.IsTrue(entityBInvalidMessages.Any());
        }
    }
}
