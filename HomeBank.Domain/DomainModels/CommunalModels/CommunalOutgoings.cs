using System;

namespace HomeBank.Domain.DomainModels.CommunalModels
{
    public sealed class CommunalOutgoings
    {
        public CommunalOutgoings(double electricalOutgoings, double couldWaterOutgoings, double hotWaterOutgoings)
        {
            if (electricalOutgoings < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(electricalOutgoings));
            }
            
            if (couldWaterOutgoings < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(couldWaterOutgoings));
            }
            
            if (hotWaterOutgoings < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hotWaterOutgoings));
            }
            
            ElectricalOutgoings = electricalOutgoings;
            CouldWaterOutgoings = couldWaterOutgoings;
            HotWaterOutgoings = hotWaterOutgoings;
        }

        public double ElectricalOutgoings { get; }
        public double CouldWaterOutgoings { get; }
        public double HotWaterOutgoings { get; }
    }
}