using HomeBank.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBank.Domain.Infrastructure
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<IEnumerable<Category>> FindAsync();
    }
}
