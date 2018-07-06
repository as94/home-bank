using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Storages
{
    public sealed class TransactionRepository : ITransactionRepository
    {
        public Task<IEnumerable<Transaction>> FindAsync(TransactionQuery query = null)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task ChangeAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
