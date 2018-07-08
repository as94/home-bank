using System;

namespace HomeBank.Data.Sqlite.Storages.Converters
{
    internal static class TransactionConverter
    {
        public static Models.Transaction Convert(Domain.DomainModels.Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            return new Models.Transaction
            {
                Id = transaction.Id.ToString(),
                Date = transaction.Date,
                Amount = transaction.Amount,
                Category = transaction.Category == null ? null : CategoryConverter.Convert(transaction.Category)
            };
        }

        public static Domain.DomainModels.Transaction Convert(Models.Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            return new Domain.DomainModels.Transaction(
                Guid.Parse(transaction.Id),
                transaction.Date,
                transaction.Amount,
                transaction.Category == null ? null : CategoryConverter.Convert(transaction.Category));
        }
    }
}
