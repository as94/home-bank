using NHibernate;
using System;

namespace HomeBank.Data.Sqlite.Infrastructure
{
    public interface ISessionProvider : IDisposable
    {
        IStatelessSession Session { get; }
    }
}
