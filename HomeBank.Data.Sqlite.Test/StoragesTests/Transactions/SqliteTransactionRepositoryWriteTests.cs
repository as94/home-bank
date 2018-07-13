using HomeBank.Data.Sqlite.Storages;
using HomeBank.Domain.Infrastructure;
using NHibernate;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.Transactions
{
    [TestFixture]
    internal class SqliteTransactionRepositoryWriteTests : SqliteTransactionRepositoryTest
    {
        [Test]
        public async Task CreateTest()
        {
            var id = Guid.NewGuid();
            var transaction = DummyData.TransactionData.CreateTransaction(id);

            await CommitCreateAsync(transaction.Category);
            await CommitCreateAsync(transaction);

            var expected = transaction;
            var actual = await TransactionRepository.GetAsync(id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ChangeTest()
        {
            var id = Guid.NewGuid();
            var transaction = DummyData.TransactionData.CreateTransaction(id);
            var amount = 20;

            await CommitCreateAsync(transaction.Category);
            await CommitCreateAsync(transaction);

            var newCategory = DummyData.CategoryData.CreateCategory(Guid.NewGuid(), "Product", "Orange", Domain.Enums.CategoryType.Expenditure);
            await CommitCreateAsync(newCategory);

            transaction.ChangeAmount(amount);
            transaction.ChangeCategory(newCategory);

            await CommitChangeAsync(transaction);

            var expected = transaction;
            var actual = await TransactionRepository.GetAsync(id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ThrowStaleObjectStateExceptionChangeIfNotExistsTest()
        {
            var id = Guid.NewGuid();
            var transaction = DummyData.TransactionData.CreateTransaction(id);
            var amount = 20;

            Assert.ThrowsAsync<StaleObjectStateException>(async () =>
            {
                transaction.ChangeAmount(amount);
                await CommitChangeAsync(transaction);
            });
        }

        [Test]
        public async Task RemoveTest()
        {
            var id = Guid.NewGuid();
            var transaction = DummyData.TransactionData.CreateTransaction(id);

            await CommitCreateAsync(transaction.Category);
            await CommitCreateAsync(transaction);

            var existing = await TransactionRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveAsync(transaction);

            var notExisting = await TransactionRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);

            var existingCategory = await CategoryRepository.GetAsync(transaction.Category.Id);
            Assert.That(existingCategory, Is.Not.Null);
        }

        [Test]
        public async Task RemoveByIdTest()
        {
            var id = Guid.NewGuid();
            var transaction = DummyData.TransactionData.CreateTransaction(id);

            await CommitCreateAsync(transaction.Category);
            await CommitCreateAsync(transaction);

            var existing = await TransactionRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveByIdAsync(id);

            var notExisting = await TransactionRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);

            var existingCategory = await CategoryRepository.GetAsync(transaction.Category.Id);
            Assert.That(existingCategory, Is.Not.Null);
        }
    }
}
