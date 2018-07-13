using System;
using System.Threading.Tasks;
using HomeBank.Domain.DomainModels.StatisticModels;
using HomeBank.Domain.Queries;

namespace HomeBank.Domain.Infrastructure.Statistic
{
    public sealed class StatisticService : IStatisticService
    {
        private readonly ITransactionRepository _transactionRepository;

        public StatisticService(ITransactionRepository transactionRepository)
        {
            if (transactionRepository == null)
            {
                throw new ArgumentNullException(nameof(transactionRepository));
            }

            _transactionRepository = transactionRepository;
        }

        public async Task<CategoryStatistic> GetCategoryStatisticAsync(CategoryStatisticQuery query = null)
        {
            //_transactionRepository.FindAsync(new TransactionQuery())
            throw new NotImplementedException();
        }
    }
}
