using System;
using System.Threading.Tasks;

namespace HomeBank.Domain.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
    }
}
