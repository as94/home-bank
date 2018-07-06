using HomeBank.Data.Sqlite.Storages;
using HomeBank.Domain.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests
{
    [TestFixture]
    internal class CategoryTests : StorageTest
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
            var category = new Domain.DomainModels.Category(id, "Product", "Apple", Domain.Enums.CategoryType.Expenditure);

            using (var session = SessionProvider.Session)
            {
                using (var unitOfWork = UnitOfWorkFactory.Create())
                {
                    await _categoryRepository.CreateAsync(category);
                    await unitOfWork.CommitAsync();
                }

                var expected = category;
                var actual = await _categoryRepository.GetAsync(id);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
