using System;
using System.Collections.Generic;
using HomeBank.Domain.DomainModels;

namespace HomeBank.Domain.Infrastructure.Comparers
{
    public static class TransactionComparers
    {
        public static IComparer<Transaction> DefaultTransactionComparer => Comparer<Transaction>.Create((x, y) =>
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
            
            var amounts = y.Amount.CompareTo(x.Amount);

            if (amounts == 0)
            {
                var categories = CategoryComparers.DefaultCategoryComparer.Compare(x.Category, y.Category);
                if (categories == 0)
                {
                    return x.Date.CompareTo(y.Date);
                }
            }

            return amounts;
        });
    }
}