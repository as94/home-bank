using System;
using System.Collections.Generic;

namespace HomeBank.Domain.DomainModels.CommunalModels
{
    public sealed class CommunalPayments : IEquatable<CommunalPayments>
    {
        private const double Tolerance = 0.01;
        
        public CommunalPayments(double electricalPayment, double couldWaterPayment, double hotWaterPayment)
        {
            ElectricalPayment = electricalPayment;
            CouldWaterPayment = couldWaterPayment;
            HotWaterPayment = hotWaterPayment;
        }
        
        public double ElectricalPayment { get; }
        public double CouldWaterPayment { get; }
        public double HotWaterPayment { get; }

        public double Total => ElectricalPayment + CouldWaterPayment + HotWaterPayment;
        
        #region Equals Logic

        public override bool Equals(object obj)
        {
            if (!(obj is CommunalPayments compareTo)) return false;

            return Equals(compareTo);
        }

        public bool Equals(CommunalPayments other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Math.Abs(ElectricalPayment - other.ElectricalPayment) < Tolerance &&
                   Math.Abs(CouldWaterPayment - other.CouldWaterPayment) < Tolerance &&
                   Math.Abs(HotWaterPayment - other.HotWaterPayment) < Tolerance &&
                   Math.Abs(Total - other.Total) < Tolerance;
        }

        public override int GetHashCode()
        {
            return ElectricalPayment.GetHashCode() ^
                   CouldWaterPayment.GetHashCode() ^
                   HotWaterPayment.GetHashCode() ^
                   Total.GetHashCode();
        }

        public static bool operator ==(CommunalPayments x, CommunalPayments y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Equals(y);
        }

        public static bool operator !=(CommunalPayments x, CommunalPayments y)
        {
            return !(x == y);
        }

        #endregion
    }
}