using HomeBank.Data.Sqlite.Test.StoragesTests.DummyData;
using NHibernate;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.Categories
{
    [TestFixture]
    internal class CategoryRepositoryWriteTests : CategoryRepositoryTest
    {
        [Test]
        public async Task CreateTest()
        {
            var id = Guid.NewGuid();
            var category = CategoryData.CreateCategory(id);
            
            await CommitCreateAsync(category);

            var expected = category;
            var actual = await CategoryRepository.GetAsync(id);

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
            var actual = await CategoryRepository.GetAsync(id);

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

            var existing = await CategoryRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveAsync(category);

            var notExisting = await CategoryRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);
        }

        [Test]
        public async Task RemoveByIdTest()
        {
            var id = Guid.NewGuid();
            var category = CategoryData.CreateCategory(id);

            await CommitCreateAsync(category);

            var existing = await CategoryRepository.GetAsync(id);
            Assert.That(existing, Is.Not.Null);

            await CommitRemoveByIdAsync(id);

            var notExisting = await CategoryRepository.GetAsync(id);
            Assert.That(notExisting, Is.Null);
        }
    }
}
