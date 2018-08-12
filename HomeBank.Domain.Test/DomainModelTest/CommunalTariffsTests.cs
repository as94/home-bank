using System;
using HomeBank.Domain.DomainModels.CommunalModels;
using NUnit.Framework;

namespace HomeBank.Domain.Test.DomainModelTest
{
    [TestFixture]
    public class CommunalTarifficsTests
    {
        [Test]
        public void ThrowException_WheneEctricalSupplyLessZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommunalTariffs(-1, 0, 0));
        }
        
        [Test]
        public void ThrowException_WheneCouldWaterSupplyLessZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommunalTariffs(0, -1, 0));
        }
        
        [Test]
        public void ThrowException_WheneHotWaterSupplyLessZero_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CommunalTariffs(0, 0, -1));
        }
    }
}