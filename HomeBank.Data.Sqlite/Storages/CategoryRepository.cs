using HomeBank.Data.Sqlite.Infrastructure;
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
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly IStatelessSession _session;

        public CategoryRepository(ISessionProvider sessionProvider)
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

            var queryResult = await queryBuilder.ToListAsync();

            return queryResult.Select(Convert);
        }

        public async Task<Domain.DomainModels.Category> GetAsync(Guid id)
        {
            var category = await _session.GetAsync<Models.Category>(id.ToString());

            if (category == null)
            {
                throw new InvalidOperationException($"Category with id = '{id}' not found.");
            }

            return Convert(category);
        }

        public async Task CreateAsync(Domain.DomainModels.Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = Convert(entity);

            await _session.InsertAsync(category);
        }

        public async Task ChangeAsync(Domain.DomainModels.Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = Convert(entity);

            await _session.UpdateAsync(category);
        }

        public async Task RemoveAsync(Domain.DomainModels.Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var category = Convert(entity);

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

        private Models.Category Convert(Domain.DomainModels.Category entity)
        {
            return new Models.Category
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Description = entity.Description,
                Type = (int)entity.Type
            };
        }

        private Domain.DomainModels.Category Convert(Models.Category category)
        {
            return new Domain.DomainModels.Category(
                Guid.Parse(category.Id),
                category.Name,
                category.Description,
                (Domain.Enums.CategoryType)category.Type);
        }
    }
}
