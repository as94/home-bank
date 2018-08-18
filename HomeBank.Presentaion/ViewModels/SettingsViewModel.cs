using System;
using System.Windows.Input;
using System.Windows.Threading;
using HomeBank.Domain.DomainModels.CommunalModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Presentation.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        private const double Tolerance = 0.0001;
        private const int SettingsStateAnimationTimeInSeconds = 4;
        
        private readonly ICommunalSettings _communalSettings;
        private readonly DispatcherTimer _timer;
        
        public override string ViewModelName => nameof(SettingsViewModel);

        private string _settingsState;
        public string SettingsState
        {
            get => _settingsState;
            set
            {
                if (_settingsState == value) return;
                _settingsState = value;
                
                OnPropertyChanged();
            }
        }

        private double _electricalSupplyInRublesPerKilowatt;

        public double ElectricalSupplyInRublesPerKilowatt
        {
            get => _electricalSupplyInRublesPerKilowatt;
            set
            {
                if (Math.Abs(_electricalSupplyInRublesPerKilowatt - value) < Tolerance) return;
                _electricalSupplyInRublesPerKilowatt = value;
                
                if (_electricalSupplyInRublesPerKilowatt < 0)
                {
                    throw new ArgumentException("Electrical Supply In Rubles Per Kilowatt shouldn't be less than zero");
                }
                
                OnPropertyChanged();
            }
        }

        private double _couldWaterSupplyInRublesPerCubicMeters;

        public double CouldWaterSupplyInRublesPerCubicMeters
        {
            get => _couldWaterSupplyInRublesPerCubicMeters;
            set
            {
                if (Math.Abs(_couldWaterSupplyInRublesPerCubicMeters - value) < Tolerance) return;
                _couldWaterSupplyInRublesPerCubicMeters = value;
                if (_couldWaterSupplyInRublesPerCubicMeters < 0)
                {
                    throw new ArgumentException("Could Water Supply In Rubles Per Cubic Meters shouldn't be less than zero");
                }
                
                OnPropertyChanged();
            }
        }

        private double _hotWaterSupplyInRublesPerCubicMeters;

        public double HotWaterSupplyInRublesPerCubicMeters
        {
            get => _hotWaterSupplyInRublesPerCubicMeters;
            set
            {
                if (Math.Abs(_hotWaterSupplyInRublesPerCubicMeters - value) < Tolerance) return;
                _hotWaterSupplyInRublesPerCubicMeters = value;
                if (_hotWaterSupplyInRublesPerCubicMeters < 0)
                {
                    throw new ArgumentException("Hot Water Supply In Rubles Per Cubic Meters shouldn't be less than zero");
                }
                
                OnPropertyChanged();
            }
        }

        public SettingsViewModel(IEventBus eventBus, ICommunalSettings communalSettings) : base(eventBus)
        {
            _communalSettings = communalSettings ?? throw new ArgumentNullException(nameof(communalSettings));

            ElectricalSupplyInRublesPerKilowatt =
                _communalSettings.CommunalTariffs.ElectricalSupplyInRublesPerKilowatt;
            
            CouldWaterSupplyInRublesPerCubicMeters =
                _communalSettings.CommunalTariffs.CouldWaterSupplyInRublesPerCubicMeters;
            
            HotWaterSupplyInRublesPerCubicMeters =
                _communalSettings.CommunalTariffs.HotWaterSupplyInRublesPerCubicMeters;

            SettingsState = string.Empty;
            
            _timer = new DispatcherTimer();
            _timer.Tick += (s, e) => SettingsState = string.Empty;
            _timer.Interval = TimeSpan.FromSeconds(SettingsStateAnimationTimeInSeconds);
        }

        private ICommand _saveSettingsCommand;

        public ICommand SaveSettingsCommand
        {
            get
            {
                return _saveSettingsCommand ?? (_saveSettingsCommand = new ActionCommand(vm =>
                           {
                               var tarifs = new CommunalTariffs(
                                   ElectricalSupplyInRublesPerKilowatt,
                                   CouldWaterSupplyInRublesPerCubicMeters,
                                   HotWaterSupplyInRublesPerCubicMeters);
                               
                               _communalSettings.Save(tarifs);

                               SettingsState = "Saved";
                               _timer.Start();
                           },
                           vm => _electricalSupplyInRublesPerKilowatt >= 0 &&
                                 _couldWaterSupplyInRublesPerCubicMeters >= 0 &&
                                 _hotWaterSupplyInRublesPerCubicMeters >= 0));
            }
        }
    }
}
