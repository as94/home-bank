using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Enums;

namespace HomeBank.Domain.Queries
{
    public sealed class TransactionQuery
    {
        public TransactionQuery(DateRangeQuery dateRangeQuery = null, CategoryType? type = null, Category category = null)
        {
            DateRangeQuery = dateRangeQuery;
            Type = type;
            Category = category;
        }

        public DateRangeQuery DateRangeQuery { get; }
        public CategoryType? Type { get; }
        public Category Category { get; }
    }
}
