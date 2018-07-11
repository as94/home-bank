using HomeBank.Data.Sqlite.Test.StoragesTests.DummyData;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.Transactions
{
    [TestFixture]
    internal class TransactionRepositoryReadTests : TransactionRepositoryTest
    {
        [Test]
        public async Task FindByDateTest()
        {
            var date = new DateTime(2018, 1, 1);

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

            var foundTransactions = await TransactionRepository.FindAsync(new Domain.Queries.TransactionQuery(date));
            Assert.That(foundTransactions, Is.EquivalentTo(new[] { transaction1 }));
        }

        [Test]
        public async Task FindByCategoryTest()
        {
            var category1 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Business", description: "Site", type: Domain.Enums.CategoryType.Income);
            var category2 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Apple", type: Domain.Enums.CategoryType.Expenditure);
            var category3 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Orange", type: Domain.Enums.CategoryType.Expenditure);
            var category4 = CategoryData.CreateCategory(Guid.NewGuid(), name: string.Empty, description: string.Empty, type: Domain.Enums.CategoryType.None);

            await CommitCreateAsync(category1);
            await CommitCreateAsync(category2);
            await CommitCreateAsync(category3);
            await CommitCreateAsync(category4);
            
            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category1);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category2);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category3);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category4);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);

            var foundTransactions = await TransactionRepository.FindAsync(new Domain.Queries.TransactionQuery(category: category2));
            Assert.That(foundTransactions, Is.EquivalentTo(new[] { transaction2 }));
        }

        [Test]
        public async Task FindByCategoryTypeTest()
        {
            var category1 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Business", description: "Site", type: Domain.Enums.CategoryType.Income);
            var category2 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Apple", type: Domain.Enums.CategoryType.Expenditure);
            var category3 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Orange", type: Domain.Enums.CategoryType.Expenditure);
            var category4 = CategoryData.CreateCategory(Guid.NewGuid(), name: string.Empty, description: string.Empty, type: Domain.Enums.CategoryType.None);

            await CommitCreateAsync(category1);
            await CommitCreateAsync(category2);
            await CommitCreateAsync(category3);
            await CommitCreateAsync(category4);

            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category1);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category2);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category3);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), category: category4);

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
            var category1 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Business", description: "Site", type: Domain.Enums.CategoryType.Income);
            var category2 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Apple", type: Domain.Enums.CategoryType.Expenditure);
            var category3 = CategoryData.CreateCategory(Guid.NewGuid(), name: "Products", description: "Orange", type: Domain.Enums.CategoryType.Expenditure);
            var category4 = CategoryData.CreateCategory(Guid.NewGuid(), name: string.Empty, description: string.Empty, type: Domain.Enums.CategoryType.None);

            await CommitCreateAsync(category1);
            await CommitCreateAsync(category2);
            await CommitCreateAsync(category3);
            await CommitCreateAsync(category4);

            var date1 = new DateTime(2018, 1, 1);
            var date2 = new DateTime(2018, 1, 2);
            var date3 = new DateTime(2018, 1, 3);
            var date4 = new DateTime(2018, 1, 4);

            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), date1, category: category1);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), date2, category: category2);
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), date3, category: category3);
            var transaction4 = TransactionData.CreateTransaction(Guid.NewGuid(), date4, category: category4);

            await CommitCreateAsync(transaction1);
            await CommitCreateAsync(transaction2);
            await CommitCreateAsync(transaction3);
            await CommitCreateAsync(transaction4);

            var query = new Domain.Queries.TransactionQuery(date: date3, type: Domain.Enums.CategoryType.Expenditure, category: category3);

            var foundTransactions = await TransactionRepository.FindAsync(query);
            Assert.That(foundTransactions, Is.EquivalentTo(new[] { transaction3 }));
        }
    }
}
