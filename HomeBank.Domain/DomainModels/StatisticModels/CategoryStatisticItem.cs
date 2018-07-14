using System;

namespace HomeBank.Domain.DomainModels.StatisticModels
{
    public sealed class CategoryStatisticItem : IEquatable<CategoryStatisticItem>
    {
        public CategoryStatisticItem(Category category, decimal cost)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            Category = category;
            Cost = cost;
        }

        public Category Category { get; }
        public decimal Cost { get; }

        #region Equals Logic

        public override bool Equals(object obj)
        {
            if (!(obj is CategoryStatisticItem compareTo)) return false;

            return Equals(compareTo);
        }

        public bool Equals(CategoryStatisticItem other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Category == other.Category &&
                   Cost == other.Cost;
        }

        public override int GetHashCode()
        {
            return Category.GetHashCode() ^ Cost.GetHashCode();
        }

        public static bool operator ==(CategoryStatisticItem x, CategoryStatisticItem y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Equals(y);
        }

        public static bool operator !=(CategoryStatisticItem x, CategoryStatisticItem y)
        {
            return !(x == y);
        }

        #endregion
    }
}
