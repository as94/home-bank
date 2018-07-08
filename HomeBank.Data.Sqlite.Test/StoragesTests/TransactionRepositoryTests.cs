using HomeBank.Data.Sqlite.Storages;
using HomeBank.Domain.Infrastructure;
using NHibernate;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests
{
    [TestFixture]
    internal class TransactionRepositoryTests : StorageTest
    {
        private ICategoryRepository _categoryRepository;
        private ITransactionRepository _transactionRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _categoryRepository = new CategoryRepository(SessionProvider);
            _transactionRepository = new TransactionRepository(SessionProvider);
        }

        [Test]
        public async Task CreateTest()
        {
            var id = Guid.NewGuid();
            var transaction = DummyData.TransactionData.CreateTransaction(id);

            await CommitCreateAsync(transaction.Category);
            await CommitCreateAsync(transaction);

            var expected = transaction;
            var actual = await _transactionRepository.GetAsync(id);

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
            var actual = await _transactionRepository.GetAsync(id);

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

            var existing = await _transactionRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveAsync(transaction);

            var notExisting = await _transactionRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);

            var existingCategory = await _categoryRepository.GetAsync(transaction.Category.Id);
            Assert.That(existingCategory, Is.Not.Null);
        }

        [Test]
        public async Task RemoveByIdTest()
        {
            var id = Guid.NewGuid();
            var transaction = DummyData.TransactionData.CreateTransaction(id);

            await CommitCreateAsync(transaction.Category);
            await CommitCreateAsync(transaction);

            var existing = await _transactionRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveByIdAsync(id);

            var notExisting = await _transactionRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);

            var existingCategory = await _categoryRepository.GetAsync(transaction.Category.Id);
            Assert.That(existingCategory, Is.Not.Null);
        }

        private async Task CommitCreateAsync(Domain.DomainModels.Transaction transaction)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _transactionRepository.CreateAsync(transaction);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task CommitChangeAsync(Domain.DomainModels.Transaction transaction)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _transactionRepository.ChangeAsync(transaction);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task CommitRemoveAsync(Domain.DomainModels.Transaction transaction)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _transactionRepository.RemoveAsync(transaction);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task CommitRemoveByIdAsync(Guid id)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _transactionRepository.RemoveAsync(id);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task CommitCreateAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _categoryRepository.CreateAsync(category);
                await unitOfWork.CommitAsync();
            }
        }

        // TODO: add find test
    }
}
