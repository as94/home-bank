using HomeBank.Domain.Enums;

namespace HomeBank.Domain.Queries
{
    public sealed class CategoryStatisticQuery
    {
        public CategoryStatisticQuery(DateRangeQuery dateRangeQuery = null, CategoryType? type = null)
        {
            DateRangeQuery = dateRangeQuery;
            CategoryType = type;
        }

        public DateRangeQuery DateRangeQuery { get; }

        public CategoryType? CategoryType { get; }
    }
}
