﻿using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBank.Domain.Infrastructure.Comparers;

namespace HomeBank.Domain.Test.FakeStorages
{
    public sealed class FakeCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categories;

        public FakeCategoryRepository()
        {
            _categories = new List<Category>();
        }

        public FakeCategoryRepository(List<Category> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            _categories = categories;
        }

        public async Task<Category> GetAsync(Guid id)
        {
            return await Task.Run(() => _categories.FirstOrDefault(c => c.Id == id));
        }

        public async Task<IEnumerable<Category>> FindAsync(
            CategoryQuery query = null,
            IComparer<Category> categoryComparer = null)
        {
            return await Task.Run(() =>
            {
                if (query?.Type != null)
                {
                    return _categories.Where(c => c.Type == query.Type.Value);
                }

                if (categoryComparer == null)
                {
                    categoryComparer = CategoryComparers.DefaultCategoryComparer;
                }

                return _categories.OrderBy(c => c, categoryComparer);
            });
        }

        public async Task CreateAsync(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Run(() => _categories.Add(entity));
        }

        public async Task ChangeAsync(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Run(() =>
            {
                var old = _categories.FirstOrDefault(c => c.Id == entity.Id);

                if (old == null)
                {
                    throw new ArgumentException(nameof(entity));
                }

                old.ChangeName(entity.Name);
                old.ChangeDescription(entity.Description);
                old.ChangeType(entity.Type);
            });
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await GetAsync(id);

            await Task.Run(() => _categories.Remove(entity));
        }

        public async Task RemoveAsync(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Run(() => _categories.Remove(entity));
        }
    }
}
