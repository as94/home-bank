using System;
using NHibernate;

namespace HomeBank.Data.Sqlite.Infrastructure
{
    public sealed class SessionFactory : ISessionFactory
    {
        private IStatelessSession _session;
        private readonly ISessionFactoryProvider _sessionFactoryProvider;

        public SessionFactory(ISessionFactoryProvider sessionFactoryProvider)
        {
            if (sessionFactoryProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionFactoryProvider));
            }

            _sessionFactoryProvider = sessionFactoryProvider;
        }

        public void Dispose()
        {
            if (_session != null)
            {
                _session.Dispose();
            }
        }

        public IStatelessSession Create()
        {
            _session = _sessionFactoryProvider.SessionFactory.OpenStatelessSession();

            return _session;
        }
    }
}
