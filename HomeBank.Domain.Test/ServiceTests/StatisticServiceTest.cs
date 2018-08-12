using HomeBank.Domain.DomainModels.StatisticModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Test.DummyData;
using HomeBank.Domain.Test.FakeStorages;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using HomeBank.Domain.Infrastructure.Statistics;
using HomeBank.Domain.Queries;

namespace HomeBank.Domain.Test.ServiceTests
{
    [TestFixture]
    internal class StatisticServiceTest
    {
        private ITransactionRepository _transactionRepository;

        private IStatisticService _statisticService;

        [SetUp]
        public void SetUp()
        {
            _transactionRepository = new FakeTransactionRepository();

            _statisticService = new StatisticService(_transactionRepository);
        }

        [Test]
        public async Task GetCategoryStatisticByDateTest()
        {
            var category1 = CategoryData.CreateCategory(Guid.NewGuid(), "Products", "Orange", Enums.CategoryType.Expenditure);
            var category2 = CategoryData.CreateCategory(Guid.NewGuid(), "Products", "Cheese", Enums.CategoryType.Expenditure);
            var date = new DateTime(2018, 1, 1);

            var transaction0 = TransactionData.CreateTransaction(Guid.NewGuid(), date, 200, category1);
            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), date, 100, category2);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), new DateTime(2017, 1, 1));
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), new DateTime(2019, 1, 1));

            await _transactionRepository.CreateAsync(transaction0);
            await _transactionRepository.CreateAsync(transaction1);
            await _transactionRepository.CreateAsync(transaction2);
            await _transactionRepository.CreateAsync(transaction3);

            var query = new CategoryStatisticQuery(
                new DateRangeQuery(new DateQuery(date), new DateQuery(date)),
                Enums.CategoryType.Expenditure);

            var statistic = await _statisticService.GetCategoryStatisticAsync(query);

            var expected = new CategoryStatistic(new[]
            {
                new CategoryStatisticItem(category1, 200),
                new CategoryStatisticItem(category2, 100)
            },
            300);

            var actual = statistic;

            Assert.That(expected, Is.EqualTo(actual));
        }
        
        [Test]
        public async Task GetCategoryStatisticByDateRangeTest()
        {
            var category1 = CategoryData.CreateCategory(Guid.NewGuid(), "Products", "Orange", Enums.CategoryType.Expenditure);
            var category2 = CategoryData.CreateCategory(Guid.NewGuid(), "Products", "Cheese", Enums.CategoryType.Expenditure);
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2018, 1, 2);

            var transaction0 = TransactionData.CreateTransaction(Guid.NewGuid(), startDate, 200, category1);
            var transaction1 = TransactionData.CreateTransaction(Guid.NewGuid(), endDate, 100, category2);
            var transaction2 = TransactionData.CreateTransaction(Guid.NewGuid(), new DateTime(2017, 1, 1));
            var transaction3 = TransactionData.CreateTransaction(Guid.NewGuid(), new DateTime(2019, 1, 1));

            await _transactionRepository.CreateAsync(transaction0);
            await _transactionRepository.CreateAsync(transaction1);
            await _transactionRepository.CreateAsync(transaction2);
            await _transactionRepository.CreateAsync(transaction3);

            var query = new CategoryStatisticQuery(
                new DateRangeQuery(new DateQuery(startDate), new DateQuery(endDate)),
                Enums.CategoryType.Expenditure);
            
            var statistic = await _statisticService.GetCategoryStatisticAsync(query);

            var expected = new CategoryStatistic(new[]
                {
                    new CategoryStatisticItem(category1, 200),
                    new CategoryStatisticItem(category2, 100)
                },
                300);

            var actual = statistic;

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}
