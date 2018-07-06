using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.IO;

namespace HomeBank.Data.Sqlite.Infrastructure
{
    internal class SessionFactoryProvider : ISessionFactoryProvider
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
                if (File.Exists(dbFile))
                {
                    File.Delete(dbFile);
                }

                var schemaExport = new SchemaExport(config);
                schemaExport.Create(useStdOut: false, execute: true);
            }
        }
    }
}
