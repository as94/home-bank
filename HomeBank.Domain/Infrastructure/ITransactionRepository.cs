using HomeBank.Domain.DomainModel;
using HomeBank.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBank.Domain.Infrastructure
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
        Task<IEnumerable<Transaction>> FindAsync(TransactionQuery query = null);
    }
}
