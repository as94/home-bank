using System;
using NHibernate;

namespace HomeBank.Data.Sqlite.Infrastructure
{
    public sealed class SessionProvider : ISessionProvider
    {
        public IStatelessSession Session { get; }

        public SessionProvider(ISessionFactoryProvider sessionFactoryProvider)
        {
            if (sessionFactoryProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionFactoryProvider));
            }

            Session = sessionFactoryProvider.SessionFactory.OpenStatelessSession();
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
