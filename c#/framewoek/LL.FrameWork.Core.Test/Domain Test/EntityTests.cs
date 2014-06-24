//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

namespace Domain.Seedwork.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using LL.FrameWork.Core.Domain;
    using Domain.Seedwork.Tests.Classes;

    [TestClass()]
    public class EntityTests
    {
        [TestMethod()]
        public void SameIdentityProduceEqualsTrueTest()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var entityLeft = new SampleEntity();
            var entityRight = new SampleEntity();

            entityLeft.Id = id;
            entityRight.Id = id;

            //Act
            bool resultOnEquals = entityLeft.Equals(entityRight);
            bool resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.IsTrue(resultOnEquals);
            Assert.IsTrue(resultOnOperator);

        }
        [TestMethod()]
        public void DiferentIdProduceEqualsFalseTest()
        {
            //Arrange

            var entityLeft = new SampleEntity();
            var entityRight = new SampleEntity();

            entityLeft.Id = Guid.NewGuid();
            entityRight.Id = Guid.NewGuid();


            //Act
            bool resultOnEquals = entityLeft.Equals(entityRight);
            bool resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.IsFalse(resultOnEquals);
            Assert.IsFalse(resultOnOperator);

        }
        [TestMethod()]
        public void CompareUsingEqualsOperatorsAndNullOperandsTest()
        {
            //Arrange

            SampleEntity entityLeft = null;
            SampleEntity entityRight = new SampleEntity();

            entityRight.Id = Guid.NewGuid();

            //Act
            if (!(entityLeft == (EntityBase<Guid>)null))//this perform ==(left,right)
                Assert.Fail();

            if (!(entityRight != (EntityBase<Guid>)null))//this perform !=(left,right)
                Assert.Fail();

            entityRight = null;

            //Act
            if (!(entityLeft == entityRight))//this perform ==(left,right)
                Assert.Fail();

            if (entityLeft != entityRight)//this perform !=(left,right)
                Assert.Fail();


        }
        [TestMethod()]
        public void CompareTheSameReferenceReturnTrueTest()
        {
            //Arrange
            var entityLeft = new SampleEntity();
            SampleEntity entityRight = entityLeft;


            //Act
            if (!entityLeft.Equals(entityRight))
                Assert.Fail();

            if (!(entityLeft == entityRight))
                Assert.Fail();

        }
        [TestMethod()]
        public void CompareWhenLeftIsNullAndRightIsNullReturnFalseTest()
        {
            //Arrange

            SampleEntity entityLeft = null;
            var entityRight = new SampleEntity();

            entityRight.Id = Guid.NewGuid();

            //Act
            if (!(entityLeft == (EntityBase<Guid>)null))//this perform ==(left,right)
                Assert.Fail();

            if (!(entityRight != (EntityBase<Guid>)null))//this perform !=(left,right)
                Assert.Fail();
        }

        [TestMethod()]
        public void SetIdentitySetANonTransientEntity()
        {
            //Arrange
            var entity = new SampleEntity();

            //Act
            entity.Id = Guid.NewGuid();

            //Assert
            //Assert.IsFalse(entity.IsTransient());
        }

        [TestMethod()]
        public void ChangeIdentitySetNewIdentity()
        {
            //Arrange
            var entity = new SampleEntity();
            entity.Id = Guid.NewGuid();

            //act
            Guid expected = entity.Id;
            entity.Id = Guid.NewGuid();

            //assert
            Assert.AreNotEqual(expected, entity.Id);
        }
    }
}
