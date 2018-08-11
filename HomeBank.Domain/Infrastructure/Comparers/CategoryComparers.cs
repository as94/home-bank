using System;
using System.Collections.Generic;
using HomeBank.Domain.DomainModels;

namespace HomeBank.Domain.Infrastructure.Comparers
{
    public static class CategoryComparers
    {
        public static IComparer<Category> DefaultCategoryComparer => Comparer<Category>.Create((x, y) =>
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }
                
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }
                
            var types = x.Type.CompareTo(y.Type);
            if (types == 0)
            {
                var names = String.CompareOrdinal(x.Name, y.Name);
                if (names == 0)
                {
                    return String.CompareOrdinal(x.Description, y.Description);
                }
                
                return names;    
            }

            return types;
        });
    }
}