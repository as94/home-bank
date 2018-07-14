using HomeBank.Domain.Enums;

namespace HomeBank.Domain.Queries
{
    public sealed class CategoryStatisticQuery
    {
        public CategoryStatisticQuery(DateQuery dateQuery = null, CategoryType? type = null)
        {
            DateQuery = dateQuery;
            CategoryType = type;
        }

        public DateQuery DateQuery { get; }

        public CategoryType? CategoryType { get; }
    }
}
