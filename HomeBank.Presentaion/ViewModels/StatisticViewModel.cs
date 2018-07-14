using HomeBank.Domain.DomainModels.StatisticModels;
using HomeBank.Domain.Infrastructure.Statistic;
using HomeBank.Presentaion.Converters;
using HomeBank.Presentaion.Enums;
using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HomeBank.Presentaion.ViewModels
{
    public class StatisticViewModel : ViewModel
    {
        private IStatisticService _statisticService;

        public override string ViewModelName => nameof(StatisticViewModel);

        public static IEnumerable<CategoryTypeFilter> CategoryTypes => Utils.CategoryTypes.Filters;

        private CategoryTypeFilter _type = CategoryTypeFilter.All;
        public CategoryTypeFilter Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                OnPropertyChanged();

                EventBus.Notify(EventType.CategoryStatisticFilterChanged);
            }
        }

        public ObservableCollection<StatisticItemViewModel> CategoryStatisticItems { get; set; }

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

        public static async Task<StatisticViewModel> CreateAsync(IEventBus eventBus, IStatisticService statisticService)
        {
            var categoryStatistic = await statisticService.GetCategoryStatisticAsync();

            return new StatisticViewModel(eventBus, statisticService, categoryStatistic);
        }

        private StatisticViewModel(IEventBus eventBus, IStatisticService statisticService, CategoryStatistic categoryStatistic) : base(eventBus)
        {
            if (statisticService == null)
            {
                throw new ArgumentNullException(nameof(statisticService));
            }

            if (categoryStatistic == null)
            {
                throw new ArgumentNullException(nameof(categoryStatistic));
            }

            _statisticService = statisticService;

            CategoryStatisticItems = new ObservableCollection<StatisticItemViewModel>();

            UpdateCategoryStatisticItems(categoryStatistic.StatisticItems);
            UpdateTotal(categoryStatistic.Total);

            EventBus.EventOccured += EventBus_EventOccured;
        }

        private async void EventBus_EventOccured(EventType type, EventArgs args = null)
        {
            switch (type)
            {
                case EventType.CategoryStatisticFilterChanged:
                case EventType.TransactionItemOperationExecuted:
                case EventType.CategoryItemOperationExecuted:
                    var query = new Domain.Queries.CategoryStatisticQuery(type: Type.Convert());
                    var categoryStatistic = await _statisticService.GetCategoryStatisticAsync(query);
                    UpdateCategoryStatisticItems(categoryStatistic.StatisticItems);
                    UpdateTotal(categoryStatistic.Total);
                    break;
            }
        }

        private void UpdateCategoryStatisticItems(IEnumerable<CategoryStatisticItem> statisticItems)
        {
            if (statisticItems == null)
            {
                throw new ArgumentNullException(nameof(statisticItems));
            }

            CategoryStatisticItems.Clear();

            foreach (var statisticItem in statisticItems)
            {
                var category = statisticItem.Category;
                var cost = statisticItem.Cost;

                var view = new StatisticItemViewModel(EventBus)
                {
                    CategoryItemViewModel = new CategoryItemViewModel(EventBus)
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                        Type = category.Type
                    },
                    Cost = cost
                };

                CategoryStatisticItems.Add(view);
            }
        }

        private void UpdateTotal(decimal total)
        {
            Total = $"Total: {total}";
        }
    }
}
