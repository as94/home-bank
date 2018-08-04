using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Presentation.ViewModels
{
    public class StatisticItemViewModel : ViewModel
    {
        public override string ViewModelName => nameof(StatisticItemViewModel);

        public CategoryItemViewModel CategoryItemViewModel { get; set; }
        public decimal Cost { get; set; }

        public StatisticItemViewModel(IEventBus eventBus)
            : base(eventBus)
        {
        }
    }
}
