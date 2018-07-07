using HomeBank.Presentaion.Infrastructure;

namespace HomeBank.Presentaion.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public override string ViewModelName => nameof(SettingsViewModel);

        public SettingsViewModel(IEventBus eventBus) : base(eventBus)
        {
        }
    }
}
