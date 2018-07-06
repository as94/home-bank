using HomeBank.Data.Sqlite.Infrastructure;
using HomeBank.Domain.Infrastructure;
using System;

namespace HomeBank.Data.Sqlite.UnitOfWork
{
    public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISessionProvider _sessionProvider;

        public UnitOfWorkFactory(ISessionProvider sessionProvider)
        {
            if (sessionProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionProvider));
            }

            _sessionProvider = sessionProvider;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_sessionProvider);
        }
    }
}
