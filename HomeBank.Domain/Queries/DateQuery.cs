using System;

namespace HomeBank.Domain.Queries
{
    public sealed class DateQuery
    {
        public DateQuery(DateTime? date = null)
        {
            if (date != null)
            {
                Year = date.Value.Year;
                Month = date.Value.Month;
                Day = date.Value.Day;
            }
        }

        public DateQuery(
            int? year = null,
            int? month = null,
            int? day = null)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public int? Year { get; }
        public int? Month { get; }
        public int? Day { get; }
    }
}
