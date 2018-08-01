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

                Date = new DateTime(Year, Month, Day);
            }
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }

        public DateTime? Date { get; }
    }
}
