using System;
using System.Collections.Generic;

namespace HomeBank.Domain.DomainModels.StatisticModels
{
    public sealed class CategoryStatistic
    {
        public CategoryStatistic(IEnumerable<CategoryStatisticItem> statisticItems, int total)
        {
            if (statisticItems == null)
            {
                throw new ArgumentNullException(nameof(statisticItems));
            }

            StatisticItems = statisticItems;
            Total = total;
        }

        public IEnumerable<CategoryStatisticItem> StatisticItems { get; }
        public int Total { get; }
    }
}
