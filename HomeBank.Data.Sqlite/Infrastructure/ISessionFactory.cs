using NHibernate;
using System;

namespace HomeBank.Data.Sqlite.Infrastructure
{
    public interface ISessionFactory : IDisposable
    {
        IStatelessSession Create();
    }
}
