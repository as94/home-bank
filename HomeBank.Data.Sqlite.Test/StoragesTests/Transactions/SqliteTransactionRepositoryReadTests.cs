using HomeBank.Data.Sqlite.Test.StoragesTests.DummyData;
using HomeBank.Domain.Queries;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.Transactions
{
    [TestFixture]
    internal class TransactionRepositoryReadTests : SqliteTransactionRepositoryTest
    {
        [Test]
        public async Task FindByDateTest()
        {
            var date = new DateTime(2018, 1, 1);
            var dateQuery = new DateQuery(date);

            var category = CategoryData.CreateCategory(Guid.NewGuid());

            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), date, category: category);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), date.AddDays(1), category: category);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), date.AddDays(2), category: category);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), date.AddDays(3), category: category);

            await CommitCreateAsync(category);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);

            var foundTransactions = await TransactionRepository.FindAsync(
                new TransactionQuery(new DateRangeQuery(dateQuery, dateQuery)));

            Assert.That(foundTransactions, Is.EquivalentTo(new[] { transaction1 }));
        }

        [Test]
        public async Task FindByDateRange_WhenEntersRange_Test()
        {
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2018, 1, 4);
            
            var category = CategoryData.CreateCategory(Guid.NewGuid());
            
            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate, category: category);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate.AddDays(1), category: category);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate.AddDays(2), category: category);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate.AddDays(3), category: category);
            
            await CommitCreateAsync(category);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);
            
            var dateRangeQuery = new DateRangeQuery(new DateQuery(startDate), new DateQuery(endDate));
            var foundTransactions = await TransactionRepository.FindAsync(new TransactionQuery(dateRangeQuery));

            Assert.That(foundTransactions, Is.EquivalentTo(new[]
            {
                transaction1,
                transaction2,
                transaction3, 
                transaction4
            }));
        }

        [Test]
        public async Task FindByDateRange_WhenOutOfRange_Test()
        {
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2017, 1, 4);
            
            var category = CategoryData.CreateCategory(Guid.NewGuid());
            
            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate, category: category);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate.AddDays(1), category: category);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate.AddDays(2), category: category);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate.AddDays(3), category: category);
            
            await CommitCreateAsync(category);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);
            
            var dateRangeQuery = new DateRangeQuery(new DateQuery(startDate), new DateQuery(endDate));
            var foundTransactions = await TransactionRepository.FindAsync(new TransactionQuery(dateRangeQuery));

            Assert.That(foundTransactions, Is.Empty);
        }

        [Test]
        public async Task FindByCategoryTest()
        {
            var categories = await this.CreateCategoriesAsync();
            
            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[0]);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[1]);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[2]);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[3]);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);

            var foundTransactions = await TransactionRepository.FindAsync(new Domain.Queries.TransactionQuery(category: categories[1]));
            Assert.That(foundTransactions, Is.EquivalentTo(new[] { transaction2 }));
        }

        [Test]
        public async Task FindByCategoryTypeTest()
        {
            var categories = await this.CreateCategoriesAsync();

            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[0]);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[1]);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[2]);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), category: categories[3]);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);

            var foundTransactions = await TransactionRepository.FindAsync(new Domain.Queries.TransactionQuery(type: Domain.Enums.CategoryType.Expenditure));
            Assert.That(foundTransactions, Is.EquivalentTo(new[] { transaction2, transaction3 }));
        }

        [Test]
        public async Task FindTest()
        {
            var categories = await this.CreateCategoriesAsync();
            
            var date1 = new DateTime(2018, 1, 1);
            var date2 = new DateTime(2018, 1, 2);
            var date3 = new DateTime(2018, 1, 3);
            var date4 = new DateTime(2018, 1, 4);

            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), date1, category: categories[0]);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), date2, category: categories[1]);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), date3, category: categories[2]);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), date4, category: categories[3]);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);

            var query = new Domain.Queries.TransactionQuery(
                dateRangeQuery: new DateRangeQuery(new DateQuery(date3), new DateQuery(date3)),
                type: Domain.Enums.CategoryType.Expenditure,
                category: categories[2]);

            var foundTransactions = await TransactionRepository.FindAsync(query);
            Assert.That(foundTransactions, Is.EquivalentTo(new[] { transaction3 }));
        }

        private async Task<Domain.DomainModels.Category[]> CreateCategoriesAsync()
        {
            var category1 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Business", description: "Site", type: Domain.Enums.CategoryType.Income);
            var category2 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Apple", type: Domain.Enums.CategoryType.Expenditure);
            var category3 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Orange", type: Domain.Enums.CategoryType.Expenditure);
            var category4 = CategoryData.CreateCategory(Guid.NewGuid(), name: string.Empty, description: string.Empty, type: Domain.Enums.CategoryType.None);

            await CommitCreateAsync(category1);
            await CommitCreateAsync(category2);
            await CommitCreateAsync(category3);
            await CommitCreateAsync(category4);

            return new[]
            {
                category1,
                category2,
                category3,
                category4
            };
        }
    }
}
