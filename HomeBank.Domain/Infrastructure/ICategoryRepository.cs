using HomeBank.Domain.DomainModel;
using HomeBank.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBank.Domain.Infrastructure
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<IEnumerable<Category>> FindAsync(CategoryQuery query = null);
    }
}
