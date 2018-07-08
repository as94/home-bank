using HomeBank.Data.Sqlite.Storages;
using HomeBank.Data.Sqlite.Test.StoragesTests.DummyData;
using HomeBank.Domain.Infrastructure;
using NHibernate;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests
{
    [TestFixture]
    internal class CategoryRepositoryTests : StorageTest
    {
        private ICategoryRepository _categoryRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _categoryRepository = new CategoryRepository(SessionProvider);
        }

        [Test]
        public async Task CreateTest()
        {
            var id = Guid.NewGuid();
            var category = CategoryData.CreateCategory(id);
            
            await CommitCreateAsync(category);

            var expected = category;
            var actual = await _categoryRepository.GetAsync(id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task ChangeTest()
        {
            var id = Guid.NewGuid();
            var category = CategoryData.CreateCategory(id);
            var description = "Orange";

            await CommitCreateAsync(category);

            category.ChangeDescription(description);

            await CommitChangeAsync(category);

            var expected = category;
            var actual = await _categoryRepository.GetAsync(id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ThrowStaleObjectStateExceptionChangeIfNotExistsTest()
        {
            var id = Guid.NewGuid();
            var category = CategoryData.CreateCategory(id);
            var description = "Orange";

            Assert.ThrowsAsync<StaleObjectStateException>(async () =>
            {
                category.ChangeDescription(description);
                await CommitChangeAsync(category);
            });
        }

        [Test]
        public async Task RemoveTest()
        {
            var id = Guid.NewGuid();
            var category = CategoryData.CreateCategory(id);

            await CommitCreateAsync(category);

            var existing = await _categoryRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveAsync(category);

            var notExisting = await _categoryRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);
        }

        [Test]
        public async Task RemoveByIdTest()
        {
            var id = Guid.NewGuid();
            var category = CategoryData.CreateCategory(id);

            await CommitCreateAsync(category);

            var existing = await _categoryRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveByIdAsync(id);

            var notExisting = await _categoryRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);
        }

        // TODO: add find test

        private async Task CommitCreateAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _categoryRepository.CreateAsync(category);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task CommitChangeAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _categoryRepository.ChangeAsync(category);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task CommitRemoveAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _categoryRepository.RemoveAsync(category);
                await unitOfWork.CommitAsync();
            }
        }

        private async Task CommitRemoveByIdAsync(Guid id)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await _categoryRepository.RemoveAsync(id);
                await unitOfWork.CommitAsync();
            }
        }
    }
}
