using NUnit.Framework;

namespace HomeBank.Data.Sqlite.Test.InfrastructureTests
{
    [TestFixture]
    internal class SessionProviderTest : DatabaseTest
    {
        [Test]
        public void SessionProviderNotNullTest()
        {
            using (var session = SessionProvider.Session)
            {
                Assert.That(session, Is.Not.Null);
            }
        }
    }
}
