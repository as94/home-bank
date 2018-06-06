using HomeBank.Domain.DomainModel;
using System.Threading.Tasks;

namespace HomeBank.Domain.Infrastructure
{
    public interface IRepository<EntityType, IdType> where EntityType : IIdentify<IdType>
    {
        Task<EntityType> GetAsync(IdType id);

        Task CreateAsync(EntityType entity);
        Task ChangeAsync(EntityType entity);
        Task RemoveAsync(EntityType entity);

        Task RemoveAsync(IdType id);
    }
}
