using HomeBank.Domain.DomainModels.CommunalModels;

namespace HomeBank.Domain.Infrastructure
{
    public interface ICommunalSettings
    {
        CommunalTariffs CommunalTariffs { get; }
        void Save(CommunalTariffs tariffs);
    }
}