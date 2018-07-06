using NUnit.Framework;
using System.IO;

namespace HomeBank.Data.Sqlite.Test.InfrastructureTests
{
    [TestFixture]
    internal class SessionProviderFactoryTest : DatabaseTest
    {
        [Test]
        public void DatabaseFileExistsTest()
        {
            Assert.That(File.Exists(DbFile), Is.True);
        }

        [Test]
        public void SessionFactoryProviderNotNullTest()
        {
            Assert.That(SessionFactoryProvider.SessionFactory, Is.Not.Null);
        }
    }
}
