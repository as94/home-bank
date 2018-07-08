using HomeBank.Data.Sqlite.Storages;
using HomeBank.Domain.Infrastructure;
using NUnit.Framework;

namespace HomeBank.Data.Sqlite.Test.StoragesTests
{
    [TestFixture]
    internal class TransactionTests : StorageTest
    {
        private ITransactionRepository _transactionRepository;

        [SetUp]
        public void SetUp()
        {
            //_transactionRepository = new TransactionRepository(SessionProvider);
        }
    }
}
