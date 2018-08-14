using System;
using System.Collections.Generic;
using HomeBank.Domain.DomainModels.StatisticModels;

namespace HomeBank.Domain.Infrastructure.Comparers
{
    public static class CategoryStatisticItemComparers
    {
        public static IComparer<CategoryStatisticItem> DefaultCategoryStatisticItemComparer => Comparer<CategoryStatisticItem>.Create((x, y) =>
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(x, null))
            {
                return -1;
            }
            
            if (ReferenceEquals(y, null))
            {
                return 1;
            }
            
            var amounts = y.Cost.CompareTo(x.Cost);

            if (amounts == 0)
            {
                return CategoryComparers.DefaultCategoryComparer.Compare(x.Category, y.Category);
            }

            return amounts;
        });
    }
}