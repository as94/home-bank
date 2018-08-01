using HomeBank.Data.Sqlite.Infrastructure;
using HomeBank.Data.Sqlite.Storages.Converters;
using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Queries;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Storages
{
    public sealed class SqliteTransactionRepository : ITransactionRepository
    {
        private readonly IStatelessSession _session;

        public SqliteTransactionRepository(ISessionProvider sessionProvider)
        {
            if (sessionProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionProvider));
            }

            _session = sessionProvider.Session;
        }

        public async Task<IEnumerable<Transaction>> FindAsync(TransactionQuery query = null)
        {
            var queryBuilder = _session.Query<Models.Transaction>();

            if (query != null)
            {
                queryBuilder = GetQueryBuilder(queryBuilder, query.DateRangeQuery);

                if (query.Type != null)
                {
                    queryBuilder = queryBuilder.Where(t => t.Category != null && t.Category.Type == (int)query.Type.Value);
                }

                if (query.Category != null)
                {
                    queryBuilder = queryBuilder.Where(t => t.Category != null && t.Category.Id == query.Category.Id.ToString());
                }
            }

            var queryResult = await queryBuilder
                .Select(t => new Models.Transaction
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Category = t.Category
                })
                .ToListAsync();

            return queryResult.Select(TransactionConverter.Convert);
        }

        public async Task<Transaction> GetAsync(Guid id)
        {
            var transaction = await _session.Query<Models.Transaction>()
                .Where(t => t.Id == id.ToString())
                .Select(t => new Models.Transaction
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Category = t.Category
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return null;
            }

            return TransactionConverter.Convert(transaction);
        }

        public async Task CreateAsync(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = TransactionConverter.Convert(entity);

            await _session.InsertAsync(transaction);
        }

        public async Task ChangeAsync(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = TransactionConverter.Convert(entity);

            await _session.UpdateAsync(transaction);
        }

        public async Task RemoveAsync(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var transaction = TransactionConverter.Convert(entity);

            await _session.DeleteAsync(transaction);
        }

        public async Task RemoveAsync(Guid id)
        {
            var transaction = await GetAsync(id);

            if (transaction == null)
            {
                return;
            }

            await RemoveAsync(transaction);
        }

        private static IQueryable<Models.Transaction> GetQueryBuilder(IQueryable<Models.Transaction> queryBuilder, DateRangeQuery dateRangeQuery)
        {
            if (dateRangeQuery != null)
            {
                if (dateRangeQuery.StartDate?.Date != null)
                {
                    queryBuilder = queryBuilder.Where(t => t.Date >= dateRangeQuery.StartDate.Date.Value);
                }

                if (dateRangeQuery.EndDate?.Date != null)
                {
                    queryBuilder = queryBuilder.Where(t => t.Date <= dateRangeQuery.EndDate.Date.Value);
                }
            }

            return queryBuilder;
        }
    }
}
