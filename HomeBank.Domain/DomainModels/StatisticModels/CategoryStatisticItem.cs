using System;

namespace HomeBank.Domain.DomainModels.StatisticModels
{
    public sealed class CategoryStatisticItem
    {
        public CategoryStatisticItem(Category category, int cost)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            Category = category;
            Cost = cost;
        }

        public Category Category { get; }
        public int Cost { get; }
    }
}
