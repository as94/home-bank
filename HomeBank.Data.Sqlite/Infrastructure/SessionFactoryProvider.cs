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

        public SessionFactoryProvider(string dbFile, bool overwriteExisting = false, bool isNew = false)
        {
            if (string.IsNullOrEmpty(dbFile))
            {
                throw new ArgumentNullException(nameof(dbFile));
            }

            SessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(dbFile))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
                .ExposeConfiguration(c => BuildScheme(c, dbFile, isNew, overwriteExisting))
                .BuildSessionFactory();
        }

        private void BuildScheme(Configuration config, string dbFile, bool overwriteExisting, bool isNew)
        {
            if (overwriteExisting)
            {
                if (File.Exists(dbFile))
                {
                    File.Delete(dbFile);
                }
            }
            
            if (isNew)
            {
                var schemaExport = new SchemaExport(config);
                schemaExport.Create(useStdOut: false, execute: true);
            }
        }
    }
}
