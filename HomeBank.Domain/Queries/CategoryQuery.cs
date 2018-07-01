using HomeBank.Domain.Enums;

namespace HomeBank.Domain.Queries
{
    public sealed class CategoryQuery
    {
        public CategoryQuery(CategoryType? type = null)
        {
            Type = type;
        }

        public CategoryType? Type { get; }
    }
}
