using HomeBank.Data.Sqlite.Storages;
using HomeBank.Domain.Infrastructure;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.Transactions
{
    internal abstract class SqliteTransactionRepositoryTest : StorageTest
    {
        protected ICategoryRepository CategoryRepository;
        protected ITransactionRepository TransactionRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            CategoryRepository = new SqliteCategoryRepository(SessionProvider);
            TransactionRepository = new SqliteTransactionRepository(SessionProvider);
        }

        protected async Task CommitCreateAsync(Domain.DomainModels.Transaction transaction)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await TransactionRepository.CreateAsync(transaction);
                await unitOfWork.CommitAsync();
            }
        }

        protected async Task CommitChangeAsync(Domain.DomainModels.Transaction transaction)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await TransactionRepository.ChangeAsync(transaction);
                await unitOfWork.CommitAsync();
            }
        }

        protected async Task CommitRemoveAsync(Domain.DomainModels.Transaction transaction)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await TransactionRepository.RemoveAsync(transaction);
                await unitOfWork.CommitAsync();
            }
        }

        protected async Task CommitRemoveByIdAsync(Guid id)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await TransactionRepository.RemoveAsync(id);
                await unitOfWork.CommitAsync();
            }
        }

        protected async Task CommitCreateAsync(Domain.DomainModels.Category category)
        {
            using (var unitOfWork = UnitOfWorkFactory.Create())
            {
                await CategoryRepository.CreateAsync(category);
                await unitOfWork.CommitAsync();
            }
        }
    }
}
