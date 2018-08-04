using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Presentation.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public override string ViewModelName => nameof(SettingsViewModel);

        public SettingsViewModel(IEventBus eventBus) : base(eventBus)
        {
        }
    }
}
