using System;

namespace HomeBank.Domain.Queries
{
    public sealed class DateRangeQuery
    {
        public DateRangeQuery(DateQuery startDate = null, DateQuery endDate = null)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateRangeQuery(DateTime? startDate = null, DateTime? endDate = null)
        {
            StartDate = new DateQuery(startDate);
            EndDate = new DateQuery(endDate);
        }

        public DateQuery StartDate { get; }
        public DateQuery EndDate { get; }
    }
}
