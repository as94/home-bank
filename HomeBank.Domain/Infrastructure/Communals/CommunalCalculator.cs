using System;
using HomeBank.Domain.DomainModels.CommunalModels;

namespace HomeBank.Domain.Infrastructure.Communals
{
    public sealed class CommunalCalculator : ICommunalCalculator
    {
        private readonly ICommunalSettings _communalSettings;

        public CommunalCalculator(ICommunalSettings communalSettings)
        {
            _communalSettings = communalSettings ?? throw new ArgumentNullException(nameof(communalSettings));
        }
        
        public CommunalPayments Calculate(CommunalOutgoings outgoings)
        {
            var tariffs = _communalSettings.CommunalTariffs;
            
            var electricalPayment = tariffs.ElectricalSupplyInRublesPerKilowatt * outgoings.ElectricalOutgoings;
            var couldWaterPayment = tariffs.CouldWaterSupplyInRublesPerCubicMeters * outgoings.CouldWaterOutgoings;
            var hotWaterPayment = tariffs.HotWaterSupplyInRublesPerCubicMeters * outgoings.HotWaterOutgoings;
            
            return new CommunalPayments(electricalPayment, couldWaterPayment, hotWaterPayment);
        }
    }
}