using HomeBank.Data.Sqlite.Infrastructure;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace HomeBank.Data.Sqlite.Test
{
    internal abstract class DatabaseTest
    {
        protected static readonly string DbFile = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "HomeBank.db");

        protected ISessionFactoryProvider SessionFactoryProvider;
        protected ISessionProvider SessionProvider;

        [SetUp]
        public virtual void SetUp()
        {
            SessionFactoryProvider = new SessionFactoryProvider(DbFile, overwriteExisting: true);
            SessionProvider = new SessionProvider(SessionFactoryProvider);
        }

        [TearDown]
        public void TearDown()
        {
            SessionProvider.Dispose();

            File.Delete(DbFile);
        }
    }
}
