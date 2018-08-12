using System;
using HomeBank.Domain.DomainModels.CommunalModels;
using NUnit.Framework;

namespace HomeBank.Domain.Test.DomainModelTest
{
    [TestFixture]
    public class CommunalOutgoingsTests
    {
        [Test]
        public void ThrowException_WhenElectricalOutgoingsLessZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommunalOutgoings(-1, 0, 0));
        }

        [Test]
        public void ThrowException_WhenleCouldWaterLessZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommunalOutgoings(0, -1, 0));
        }
        
        [Test]
        public void ThrowException_WhenleHotWaterLessZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommunalOutgoings(0, 0, -1));
        }
    }
}