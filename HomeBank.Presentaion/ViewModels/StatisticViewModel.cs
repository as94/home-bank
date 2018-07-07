using HomeBank.Presentaion.Infrastructure;

namespace HomeBank.Presentaion.ViewModels
{
    public class StatisticViewModel : ViewModel
    {
        public override string ViewModelName => nameof(StatisticViewModel);

        public StatisticViewModel(IEventBus eventBus) : base(eventBus)
        {
        }
    }
}
