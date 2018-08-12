using HomeBank.Domain.DomainModels.CommunalModels;

namespace HomeBank.Domain.Infrastructure.Communals
{
    public interface ICommunalCalculator
    {
        CommunalPayments Calculate(CommunalOutgoings outgoings);
    }
}