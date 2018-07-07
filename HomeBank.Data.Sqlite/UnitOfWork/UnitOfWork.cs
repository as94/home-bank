using HomeBank.Data.Sqlite.Infrastructure;
using HomeBank.Domain.Infrastructure;
using NHibernate;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private ITransaction _transaction;

        public UnitOfWork(ISessionProvider sessionProvider)
        {
            if (sessionProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionProvider));
            }

            _transaction = sessionProvider.Session.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            if (!_transaction.WasCommitted && !_transaction.WasRolledBack)
            {
                _transaction.Rollback();
            }

            _transaction.Dispose();
        }
    }
}
