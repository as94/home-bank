using HomeBank.Presentaion.Infrastructure;

namespace HomeBank.Presentaion.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        public override string ViewModelName => nameof(HomeViewModel);

        public HomeViewModel(IEventBus eventBus) : base(eventBus)
        {
        }
    }
}
