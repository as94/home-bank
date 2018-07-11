using HomeBank.Data.Sqlite.Test.StoragesTests.DummyData;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.Categories
{
    [TestFixture]
    internal class CategoryRepositoryReadTests : CategoryRepositoryTest
    {
        [Test]
        public async Task FindByTypeTest()
        {
            var category1 = CategoryData.CreateCategory(Guid.NewGuid(), type: Domain.Enums.CategoryType.Income);
            var category2 = CategoryData.CreateCategory(Guid.NewGuid(), type: Domain.Enums.CategoryType.Expenditure);
            var category3 = CategoryData.CreateCategory(Guid.NewGuid(), type: Domain.Enums.CategoryType.Expenditure);
            var category4 = CategoryData.CreateCategory(Guid.NewGuid(), type: Domain.Enums.CategoryType.None);

            await CommitCreateAsync(category1);
            await CommitCreateAsync(category2);
            await CommitCreateAsync(category3);
            await CommitCreateAsync(category4);

            var foundCategories = await CategoryRepository.FindAsync(new Domain.Queries.CategoryQuery(Domain.Enums.CategoryType.Income));
            Assert.That(foundCategories, Is.EquivalentTo(new[] { category1 }));
        }
    }
}
