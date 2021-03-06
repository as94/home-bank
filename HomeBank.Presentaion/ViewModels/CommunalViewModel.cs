﻿using System;
using System.Windows.Input;
using HomeBank.Domain.DomainModels.CommunalModels;
using HomeBank.Domain.Infrastructure.Communals;
using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Presentation.ViewModels
{
    public class CommunalViewModel : ViewModel
    {
        private const double Tolerance = 0.0001;
        
        private readonly ICommunalCalculator _communalCalculator;

        public const string ElectricalPaymentLabel = "Electrical Payment";
        public const string CouldWaterPaymentLabel = "Could Water Payment";
        public const string HotWaterPaymentLabel = "Hot Water Payment";
        public const string TotalLabel = "Total";
        
        public override string ViewModelName => nameof(SettingsViewModel);

        private double _electricalOutgoings;
        public double ElectricalOutgoings
        {
            get => _electricalOutgoings;
            set
            {
                if (Math.Abs(_electricalOutgoings - value) < Tolerance) return;
                _electricalOutgoings = value;
                
                if (_electricalOutgoings < 0)
                {
                    throw new ArgumentException("The Electrical Outgoings shouldn't be less than zero");
                }
                
                OnPropertyChanged();
            }
        }

        private double _couldWaterOutgoings;
        public double CouldWaterOutgoings
        {
            get => _couldWaterOutgoings;
            set
            {
                if (Math.Abs(_couldWaterOutgoings - value) < Tolerance) return;
                _couldWaterOutgoings = value;
                if (_couldWaterOutgoings < 0)
                {
                    throw new ArgumentException("The Could Water Outgoings shouldn't be less than zero");
                }
                
                OnPropertyChanged();
            }
        }

        private string _hotWaterPayment;
        public double HotWaterOutgoings
        {
            get => _hotWaterOutgoings;
            set
            {
                if (Math.Abs(_hotWaterOutgoings - value) < Tolerance) return;
                _hotWaterOutgoings = value;
                if (_hotWaterOutgoings < 0)
                {
                    throw new ArgumentException("The Hot Water Outgoings shouldn't be less than zero");
                }
                
                OnPropertyChanged();
            }
        }

        private string _electricalPayment;
        public string ElectricalPayment
        {
            get => _electricalPayment;
            set
            {
                if (_electricalPayment == value) return;
                _electricalPayment = value;
                
                OnPropertyChanged();
            }
        }

        private string _couldWaterPayment;
        public string CouldWaterPayment
        {
            get => _couldWaterPayment;
            set
            {
                if (_couldWaterPayment == value) return;
                _couldWaterPayment = value;
                
                OnPropertyChanged();
            }
        }


        private double _hotWaterOutgoings;
        public string HotWaterPayment
        {
            get => _hotWaterPayment;
            set
            {
                if (_hotWaterPayment == value) return;
                _hotWaterPayment = value;
                
                OnPropertyChanged();
            }
        }

        private string _total;
        public string Total
        {
            get => _total;
            set
            {
                if (_total == value) return;
                _total = value;
                OnPropertyChanged();
            }
        }

        public CommunalViewModel(IEventBus eventBus, ICommunalCalculator communalCalculator) : base(eventBus)
        {
            if (communalCalculator == null)
            {
                throw new ArgumentNullException(nameof(communalCalculator));
            }
            
            _communalCalculator = communalCalculator;
            
            ElectricalPayment = $"{ElectricalPaymentLabel}: 0";
            CouldWaterPayment = $"{CouldWaterPaymentLabel}: 0";
            HotWaterPayment = $"{HotWaterPaymentLabel}: 0";
            
            Total = $"{TotalLabel}: 0";
        }

        private ICommand _caclulateOperationCommand;
        public ICommand CaclulateOperationCommand
        {
            get
            {
                return _caclulateOperationCommand ?? (_caclulateOperationCommand = new ActionCommand(vm =>
                           {
                               var outgoings = new CommunalOutgoings(ElectricalOutgoings, CouldWaterOutgoings,
                                   HotWaterOutgoings);
                               var payments = _communalCalculator.Calculate(outgoings);

                               Update(payments);
                           },
                           vm => _electricalOutgoings >= 0 && _couldWaterOutgoings >= 0 && _hotWaterOutgoings >= 0));
            }
        }
        
        private void Update(CommunalPayments payments)
        {
            ElectricalPayment = $"{ElectricalPaymentLabel}: {payments.ElectricalPayment}";
            CouldWaterPayment = $"{CouldWaterPaymentLabel}: {payments.CouldWaterPayment}";
            HotWaterPayment = $"{HotWaterPaymentLabel}: {payments.HotWaterPayment}";
            
            Total = $"{TotalLabel}: {payments.Total}";
        }
    }
}