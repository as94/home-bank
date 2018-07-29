using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.IO;

namespace HomeBank.Data.Sqlite.Infrastructure
{
    public sealed class SessionFactoryProvider : ISessionFactoryProvider
    {
        public ISessionFactory SessionFactory { get; }

        public SessionFactoryProvider(string dbFile, bool overwriteExisting = false)
        {
            if (string.IsNullOrEmpty(dbFile))
            {
                throw new ArgumentNullException(nameof(dbFile));
            }

            SessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(dbFile))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
                .ExposeConfiguration(c => BuildScheme(c, dbFile, overwriteExisting))
                .BuildSessionFactory();
        }

        private void BuildScheme(Configuration config, string dbFile, bool overwriteExisting)
        {
            if (overwriteExisting)
            {
                DeleteDatabaseFileIfExists(dbFile);
                CreateDatabase(config);
            }
            else
            {
                CreateDatabaseIfNotExists(config, dbFile);
            }
        }

        private void DeleteDatabaseFileIfExists(string dbFile)
        {
            if (File.Exists(dbFile))
            {
                File.Delete(dbFile);
            }
        }

        private void CreateDatabaseIfNotExists(Configuration config, string dbFile)
        {
            if (!File.Exists(dbFile))
            {
                CreateDatabase(config);
            }
        }

        private static void CreateDatabase(Configuration config)
        {
            var schemaExport = new SchemaExport(config);
            schemaExport.Create(useStdOut: false, execute: true);
        }
    }
}
