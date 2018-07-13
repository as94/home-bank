using HomeBank.Data.Sqlite.Storages;
using HomeBank.Domain.Infrastructure;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.Categories
{
    internal abstract class SqliteCategoryRepositoryTest : StorageTest
    {
        protected ICategoryRepository CategoryRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            CategoryRepository = new SqliteCategoryRepository(SessionProvider);
        }

        protected async Task CommitCreateAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await CategoryRepository.CreateAsync(category);
                await unitOfWork.CommitAsync();
            }
        }

        protected async Task CommitChangeAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await CategoryRepository.ChangeAsync(category);
                await unitOfWork.CommitAsync();
            }
        }

        protected async Task CommitRemoveAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await CategoryRepository.RemoveAsync(category);
                await unitOfWork.CommitAsync();
            }
        }

        protected async Task CommitRemoveByIdAsync(Guid id)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await CategoryRepository.RemoveAsync(id);
                await unitOfWork.CommitAsync();
            }
        }
    }
}
