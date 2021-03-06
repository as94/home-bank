﻿using HomeBank.Domain.DomainModels.CommunalModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Infrastructure.Communals;
using NSubstitute;
using NUnit.Framework;

namespace HomeBank.Domain.Test.ServiceTests
{
    [TestFixture]
    public class CommunalCalculatorTests
    {
        [Test]
        public void CalculateTest()
        {
            var communalSettings = Substitute.For<ICommunalSettings>();
            communalSettings.CommunalTariffs.Returns(new CommunalTariffs(2.49, 17.16, 100.10));
            
            var calculator = new CommunalCalculator(communalSettings);
            
            var outgoings = new CommunalOutgoings(107, 2, 2);

            var payments = calculator.Calculate(outgoings);
            
            var expected = new CommunalPayments(266.43, 34.32, 200.2);
            var actual = payments;
            
            Assert.AreEqual(expected, actual);
        }
    }
}