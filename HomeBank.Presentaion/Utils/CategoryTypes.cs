using System;
using System.Collections.Generic;
using System.Linq;
using HomeBank.Domain.Enums;
using HomeBank.Presentation.Converters;
using HomeBank.Presentation.Enums;

namespace HomeBank.Presentation.Utils
{
    internal static class CategoryTypes
    {
        public static IEnumerable<CategoryType> Values => Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>();
        public static IEnumerable<CategoryTypeFilter> Filters => Values
            .Select(t => t.Convert())
            .Union(new[] { CategoryTypeFilter.All });
    }
}
