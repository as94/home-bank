using System;
using System.Collections.Generic;
using HomeBank.Domain.DomainModels;

namespace HomeBank.Domain.Infrastructure.Comparers
{
    public static class CategoryComparers
    {
        public static IComparer<Category> DefaultCategoryComparer => Comparer<Category>.Create((x, y) =>
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