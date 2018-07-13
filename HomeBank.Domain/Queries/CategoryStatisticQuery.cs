namespace HomeBank.Domain.Queries
{
    public sealed class CategoryStatisticQuery
    {
        public CategoryStatisticQuery(DateQuery dateQuery = null)
        {
            DateQuery = dateQuery;
        }

        public DateQuery DateQuery { get; }
    }
}
