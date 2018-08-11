using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using HomeBank.Data.Sqlite.Infrastructure;
using HomeBank.Data.Sqlite.Storages.Converters;
using HomeBank.Domain.DomainExceptions;
using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Infrastructure.Comparers;
using HomeBank.Domain.Queries;
using NHibernate;
using NHibernate.Exceptions;
using NHibernate.Linq;

namespace HomeBank.Data.Sqlite.Storages
{
    public sealed class SqliteCategoryRepository : ICategoryRepository
    {
        private const int SqliteConstraintErrorCode = 19;

        private readonly IStatelessSession _session;

        public SqliteCategoryRepository(ISessionProvider sessionProvider)
        {
            if (sessionProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionProvider));
            }

            _session = sessionProvider.Session;
        }

        public async Task<IEnumerable<Category>> FindAsync(
            CategoryQuery query = null,
            IComparer<Category> categoryComparer = null)
        {
            var queryBuilder = _session.Query<Models.Category>();

            if (query != null)
            {
                if (query.Type != null)
                {
                    queryBuilder = queryBuilder.Where(c => c.Type == (int)query.Type.Value);
                }
            }

            if (categoryComparer == null)
            {
                categoryComparer = CategoryComparers.DefaultCategoryComparer;
            }

            var queryResult = await queryBuilder
                .Select(c => new Models.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync();

            return queryResult
                .Select(CategoryConverter.Convert)
                .OrderBy(c => c, categoryComparer);
        }

        public async Task<Category> GetAsync(Guid id)
        {
            var category = await _session.Query<Models.Category>()
                .Where(c => c.Id == id.ToString())
                .Select(c => new Models.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return null;
            }

            return CategoryConverter.Convert(category);
        }

        public async Task CreateAsync(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = CategoryConverter.Convert(entity);

            await _session.InsertAsync(category);
        }

        public async Task ChangeAsync(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = CategoryConverter.Convert(entity);

            await _session.UpdateAsync(category);
        }

        public async Task RemoveAsync(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = CategoryConverter.Convert(entity);

            try
            {
                await _session.DeleteAsync(category);
            }
            catch (GenericADOException ex) when ((ex.InnerException as SQLiteException)?.ErrorCode == SqliteConstraintErrorCode)
            {
                throw new CategoryRelatedTransactionsException(entity);
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            var category = await GetAsync(id);

            if (category == null)
            {
                return;
            }

            await RemoveAsync(category);
        }
    }
}
