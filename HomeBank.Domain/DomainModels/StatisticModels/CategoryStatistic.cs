using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBank.Domain.DomainModels.StatisticModels
{
    public sealed class CategoryStatistic : IEquatable<CategoryStatistic>
    {
        public CategoryStatistic(IEnumerable<CategoryStatisticItem> statisticItems, decimal total)
        {
            if (statisticItems == null)
            {
                throw new ArgumentNullException(nameof(statisticItems));
            }

            StatisticItems = statisticItems;
            Total = total;
        }

        public IEnumerable<CategoryStatisticItem> StatisticItems { get; }
        public decimal Total { get; }


        #region Equals Logic

        public override bool Equals(object obj)
        {
            if (!(obj is CategoryStatistic compareTo)) return false;

            return Equals(compareTo);
        }

        public bool Equals(CategoryStatistic other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Enumerable.SequenceEqual(StatisticItems, other.StatisticItems) &&
                   Total == other.Total;
        }

        public override int GetHashCode()
        {
            return StatisticItems.GetHashCode() ^
                   Total.GetHashCode();
        }

        public static bool operator ==(CategoryStatistic x, CategoryStatistic y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Equals(y);
        }

        public static bool operator !=(CategoryStatistic x, CategoryStatistic y)
        {
            return !(x == y);
        }

        #endregion
    }
}
