using System;
using System.Collections.Generic;
using HomeBank.Domain.DomainModels.StatisticModels;

namespace HomeBank.Domain.Infrastructure.Comparers
{
    public static class CategoryStatisticItemComparers
    {
        public static IComparer<CategoryStatisticItem> DefaultCategoryStatisticItemComparer => Comparer<CategoryStatisticItem>.Create((x, y) =>
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }
                
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
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