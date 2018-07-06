using HomeBank.Data.Sqlite.UnitOfWork;
using HomeBank.Domain.Infrastructure;
using NUnit.Framework;

namespace HomeBank.Data.Sqlite.Test.StoragesTests
{
    internal abstract class StorageTest : DatabaseTest
    {
        protected IUnitOfWorkFactory UnitOfWorkFactory;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            UnitOfWorkFactory = new UnitOfWorkFactory(SessionProvider);
        }
    }
}
