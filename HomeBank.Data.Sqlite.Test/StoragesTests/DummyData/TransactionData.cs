using System;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.DummyData
{
    internal static class TransactionData
    {
        public static Domain.DomainModels.Transaction CreateTransaction(
            Guid id,
            DateTime? date = null,
            decimal? amount = null,
            Domain.DomainModels.Category category = null)
        {
            return new Domain.DomainModels.Transaction(
                id,
                date ?? DateTime.Now,
                amount ?? 10,
                category ?? CategoryData.CreateCategory(Guid.NewGuid()));
        }
    }
}
