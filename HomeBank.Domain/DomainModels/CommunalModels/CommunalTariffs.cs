using System;

namespace HomeBank.Domain.DomainModels.CommunalModels
{
    public sealed class CommunalTariffs
    {
        public CommunalTariffs(
            double electricalSupplyInRublesPerKilowatt,
            double couldWaterSupplyInRublesPerCubicMeters,
            double hotWaterSupplyInRublesPerCubicMeters)
        {
            if (electricalSupplyInRublesPerKilowatt < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(electricalSupplyInRublesPerKilowatt));
            }
            
            if (couldWaterSupplyInRublesPerCubicMeters < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(couldWaterSupplyInRublesPerCubicMeters));
            }
            
            if (hotWaterSupplyInRublesPerCubicMeters < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hotWaterSupplyInRublesPerCubicMeters));
            }
            
            ElectricalSupplyInRublesPerKilowatt = electricalSupplyInRublesPerKilowatt;
            CouldWaterSupplyInRublesPerCubicMeters = couldWaterSupplyInRublesPerCubicMeters;
            HotWaterSupplyInRublesPerCubicMeters = hotWaterSupplyInRublesPerCubicMeters;
        }

        public double ElectricalSupplyInRublesPerKilowatt { get; }
        public double CouldWaterSupplyInRublesPerCubicMeters { get; }
        public double HotWaterSupplyInRublesPerCubicMeters { get; }
    }
}