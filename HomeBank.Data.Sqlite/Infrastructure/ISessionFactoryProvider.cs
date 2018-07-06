using NHibernate;

namespace HomeBank.Data.Sqlite.Infrastructure
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory SessionFactory { get; }
    }
}
