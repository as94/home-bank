using HomeBank.Domain.Enums;
using HomeBank.Presentaion.Converters;
using HomeBank.Presentaion.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBank.Presentaion.Utils
{
    internal static class CategoryTypes
    {
        public static IEnumerable<CategoryType> Values => Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>();
        public static IEnumerable<CategoryTypeFilter> Filters => Values
            .Select(t => t.Convert())
            .Union(new[] { CategoryTypeFilter.All });
    }
}
