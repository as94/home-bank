using HomeBank.Data.Sqlite.Infrastructure;
using HomeBank.Data.Sqlite.Storages.Converters;
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
    public sealed class SqliteCategoryRepository : ICategoryRepository
    {
        private readonly IStatelessSession _session;

        public SqliteCategoryRepository(ISessionProvider sessionProvider)
        {
            if (sessionProvider == null)
            {
                throw new ArgumentNullException(nameof(sessionProvider));
            }

            _session = sessionProvider.Session;
        }

        public async Task<IEnumerable<Domain.DomainModels.Category>> FindAsync(CategoryQuery query = null)
        {
            var queryBuilder = _session.Query<Models.Category>();

            if (query != null)
            {
                if (query.Type != null)
                {
                    queryBuilder = queryBuilder.Where(c => c.Type == (int)query.Type.Value);
                }
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

            return queryResult.Select(CategoryConverter.Convert);
        }

        public async Task<Domain.DomainModels.Category> GetAsync(Guid id)
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

        public async Task CreateAsync(Domain.DomainModels.Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = CategoryConverter.Convert(entity);

            await _session.InsertAsync(category);
        }

        public async Task ChangeAsync(Domain.DomainModels.Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = CategoryConverter.Convert(entity);

            await _session.UpdateAsync(category);
        }

        public async Task RemoveAsync(Domain.DomainModels.Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = CategoryConverter.Convert(entity);

            await _session.DeleteAsync(category);
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
