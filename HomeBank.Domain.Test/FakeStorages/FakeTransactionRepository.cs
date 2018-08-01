using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBank.Domain.Test.FakeStorages
{
    public sealed class FakeTransactionRepository : ITransactionRepository
    {
        private readonly List<Transaction> _transaction;

        public FakeTransactionRepository()
        {
            _transaction = new List<Transaction>();
        }

        public FakeTransactionRepository(List<Transaction> transactions)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            _transaction = transactions;
        }

        public async Task<Transaction> GetAsync(Guid id)
        {
            return await Task.Run(() => _transaction.FirstOrDefault(t => t.Id == id));
        }
 
        public async Task<IEnumerable<Transaction>> FindAsync(TransactionQuery query = null)
        {
            return await Task.Run(() =>
            {
                IEnumerable<Transaction> result = _transaction;

                if (query != null)
                {
                    result = GetQueryBuilder(result, query.DateRangeQuery);

                    if (query.Type != null)
                    {
                        result = result.Where(c => c.Category?.Type == query.Type.Value);
                    }

                    if (query.Category != null)
                    {
                        result = result.Where(c => c.Category == query.Category);
                    }
                }

                return result;
            });
        }

        public async Task CreateAsync(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Run(() => _transaction.Add(entity));
        }

        public async Task ChangeAsync(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Run(() =>
            {
                var old = _transaction.FirstOrDefault(t => t.Id == entity.Id);

                if (old == null)
                {
                    throw new ArgumentException(nameof(entity));
                }

                old.ChangeDate(entity.Date);
                old.ChangeAmount(entity.Amount);
                old.ChangeCategory(entity.Category);
            });
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await GetAsync(id);

            await Task.Run(() => _transaction.Remove(entity));
        }

        public async Task RemoveAsync(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Run(() => _transaction.Remove(entity));
        }

        private static IEnumerable<Transaction> GetQueryBuilder(IEnumerable<Transaction> result, DateRangeQuery dateRangeQuery)
        {
            if (dateRangeQuery != null)
            {
                if (dateRangeQuery.StartDate?.Date != null)
                {
                    result = result.Where(t => t.Date >= dateRangeQuery.StartDate.Date.Value);
                }

                if (dateRangeQuery.EndDate?.Date != null)
                {
                    result = result.Where(t => t.Date <= dateRangeQuery.EndDate.Date.Value);
                }
            }

            return result;
        }
    }
}
