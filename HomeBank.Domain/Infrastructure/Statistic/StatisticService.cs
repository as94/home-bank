using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBank.Domain.DomainModels.StatisticModels;
using HomeBank.Domain.Infrastructure.Comparers;
using HomeBank.Domain.Queries;

namespace HomeBank.Domain.Infrastructure.Statistic
{
    public sealed class StatisticService : IStatisticService
    {
        private readonly ITransactionRepository _transactionRepository;

        public StatisticService(ITransactionRepository transactionRepository)
        {
            if (transactionRepository == null)
            {
                throw new ArgumentNullException(nameof(transactionRepository));
            }

            _transactionRepository = transactionRepository;
        }

        public async Task<CategoryStatistic> GetCategoryStatisticAsync(
            CategoryStatisticQuery query = null,
            IComparer<CategoryStatisticItem> categoryStatisticItemComparer = null)
        {
            var transactionQuery = new TransactionQuery(
                dateRangeQuery: query?.DateRangeQuery,
                type: query?.CategoryType);

            if (categoryStatisticItemComparer == null)
            {
                categoryStatisticItemComparer = CategoryStatisticItemComparers.DefaultCategoryStatisticItemComparer;
            }

            var transactions = await _transactionRepository.FindAsync(transactionQuery);

            var statisticItems = transactions
                .Where(t => t.Category != null)
                .GroupBy(t => t.Category)
                .Select(g => new CategoryStatisticItem(
                    category: g.Key,
                    cost: g.Select(c => c.Amount).Sum()))
                .OrderBy(i => i, categoryStatisticItemComparer)
                .ToArray();

            decimal total = GetTotal(query, statisticItems);

            return new CategoryStatistic(statisticItems, total);
        }

        private static decimal GetTotal(CategoryStatisticQuery query, CategoryStatisticItem[] statisticItems)
        {
            decimal total = 0;

            if (IsDifferenceTotal(query))
            {
                var totalIncomes = statisticItems
                    .Where(item => item.Category.Type == Enums.CategoryType.Income)
                    .Sum(item => item.Cost);

                var totalExpenditures = statisticItems
                    .Where(item => item.Category.Type == Enums.CategoryType.Expenditure)
                    .Sum(item => item.Cost);

                total = totalIncomes - totalExpenditures;
            }
            else
            {
                total = statisticItems.Sum(item => item.Cost);
            }

            return total;
        }

        private static bool IsDifferenceTotal(CategoryStatisticQuery query)
        {
            return query?.CategoryType == null || query.CategoryType == Enums.CategoryType.None;
        }
    }
}
