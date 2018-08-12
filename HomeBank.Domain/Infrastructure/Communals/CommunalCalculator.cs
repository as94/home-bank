using System;
using HomeBank.Domain.DomainModels.CommunalModels;

namespace HomeBank.Domain.Infrastructure.Communals
{
    public sealed class CommunalCalculator : ICommunalCalculator
    {
        public CommunalCalculator(CommunalTariffs tariffs)
        {
            ChangeTariffs(tariffs);
        }
        
        public CommunalTariffs Tariffs { get; private set; }

        public void ChangeTariffs(CommunalTariffs tariffs)
        {
            if (tariffs == null)
            {
                throw new ArgumentNullException(nameof(tariffs));
            }
            
            Tariffs = tariffs;   
        }
        
        public CommunalPayments Calculate(CommunalOutgoings outgoings)
        {
            var electricalPayment = Tariffs.ElectricalSupplyInRublesPerKilowatt * outgoings.ElectricalOutgoings;
            var couldWaterPayment = Tariffs.CouldWaterSupplyInRublesPerCubicMeters * outgoings.CouldWaterOutgoings;
            var hotWaterPayment = Tariffs.HotWaterSupplyInRublesPerCubicMeters * outgoings.HotWaterOutgoings;
            
            return new CommunalPayments(electricalPayment, couldWaterPayment, hotWaterPayment);
        }
    }
}