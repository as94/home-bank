using System.Collections.Generic;
using System.Threading.Tasks;
using HomeBank.Domain.DomainModels.StatisticModels;
using HomeBank.Domain.Queries;

namespace HomeBank.Domain.Infrastructure.Statistics
{
    public interface IStatisticService
    {
        Task<CategoryStatistic> GetCategoryStatisticAsync(
            CategoryStatisticQuery query = null,
            IComparer<CategoryStatisticItem> categoryStatisticItemComparer = null);
    }
}
