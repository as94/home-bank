using HomeBank.Domain.DomainModels;
using NUnit.Framework;
using System;

namespace HomeBank.Domain.Test.DomainModelTest
{
    [TestFixture]
    internal class TransactionTest
    {
        [Test]
        public void TransactionCtorTest()
        {
            var id = Guid.NewGuid();
            var date = new DateTime(2018, 2, 3);
            var amount = 10.0M;
            var category = CreateCategory();

            var transaction = new Transaction(id, date, amount, category);

            Assert.That(transaction.Id, Is.EqualTo(id));
            Assert.That(transaction.Date, Is.EqualTo(date));
            Assert.That(transaction.Amount, Is.EqualTo(amount));
            Assert.That(transaction.Category, Is.EqualTo(category));
        }

        private static Category CreateCategory()
        {
            return new Category(
                id: Guid.NewGuid(),
                name: "Product",
                description: "Orange",
                type: Enums.CategoryType.Expenditure);
        }
    }
}
