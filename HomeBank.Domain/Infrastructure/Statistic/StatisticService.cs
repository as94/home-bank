﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBank.Domain.DomainModels.StatisticModels;
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

        public async Task<CategoryStatistic> GetCategoryStatisticAsync(CategoryStatisticQuery query = null)
        {
            var transactionQuery = new TransactionQuery(dateQuery: query?.DateQuery, type: query?.CategoryType);
            var transactions = await _transactionRepository.FindAsync(transactionQuery);

            var statisticItems = transactions
                .Where(t => t.Category != null)
                .GroupBy(t => t.Category)
                .Select(g => new CategoryStatisticItem(
                    category: g.Key,
                    cost: g.Select(c => c.Amount).Sum()));

            decimal total = GetTotal(query, statisticItems);

            return new CategoryStatistic(statisticItems, total);
        }

        private static decimal GetTotal(CategoryStatisticQuery query, IEnumerable<CategoryStatisticItem> statisticItems)
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
            return query == null || query.CategoryType == null || query.CategoryType == Enums.CategoryType.None;
        }
    }
}
