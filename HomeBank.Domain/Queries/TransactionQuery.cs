using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Enums;

namespace HomeBank.Domain.Queries
{
    public sealed class TransactionQuery
    {
        public TransactionQuery(DateQuery dateQuery = null, CategoryType? type = null, Category category = null)
        {
            DateQuery = dateQuery;
            Type = type;
            Category = category;
        }

        public DateQuery DateQuery { get; }
        public CategoryType? Type { get; }
        public Category Category { get; }
    }
}
