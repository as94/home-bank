using System;

namespace HomeBank.Domain.DomainModels
{
    public sealed class Transaction : IIdentify<Guid>, IEquatable<Transaction>
    {
        public Guid Id { get; }

        public DateTime Date { get; private set; }
        public decimal Amount { get; private set; }
        public Category Category { get; private set; }

        public Transaction(
            Guid id,
            DateTime date,
            decimal amount,
            Category category)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            Id = id;

            Date = date;
            Amount = amount;
            Category = category;
        }

        public void ChangeDate(DateTime date)
        {
            Date = date;
        }

        public void ChangeAmount(decimal amount)
        {
            Amount = amount;
        }

        public void ChangeCategory(Category category)
        {
            Category = category;
        }

        #region Equals Logic

        public override bool Equals(object obj)
        {
            if (!(obj is Transaction compareTo)) return false;

            return Equals(compareTo);
        }

        public bool Equals(Transaction other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Id == other.Id &&
                   Date == other.Date &&
                   Amount == other.Amount &&
                   Category == other.Category;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^
                   Date.GetHashCode() ^
                   Amount.GetHashCode() ^
                   Category.GetHashCode();
        }

        public static bool operator ==(Transaction x, Transaction y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Equals(y);
        }

        public static bool operator !=(Transaction x, Transaction y)
        {
            return !(x == y);
        }

        #endregion
    }
}
