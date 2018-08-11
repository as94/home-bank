using System;
using System.Collections.Generic;
using HomeBank.Domain.DomainModels;

namespace HomeBank.Domain.Infrastructure.Comparers
{
    public static class TransactionComparers
    {
        public static IComparer<Transaction> DefaultTransactionComparer => Comparer<Transaction>.Create((x, y) =>
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }
                
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
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