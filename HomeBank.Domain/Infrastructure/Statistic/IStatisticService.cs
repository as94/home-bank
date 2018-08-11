using System.Collections.Generic;
using HomeBank.Domain.DomainModels.StatisticModels;
using HomeBank.Domain.Queries;
using System.Threading.Tasks;

namespace HomeBank.Domain.Infrastructure.Statistic
{
    public interface IStatisticService
    {
        Task<CategoryStatistic> GetCategoryStatisticAsync(
            CategoryStatisticQuery query = null,
            IComparer<CategoryStatisticItem> categoryStatisticItemComparer = null);
    }
}
