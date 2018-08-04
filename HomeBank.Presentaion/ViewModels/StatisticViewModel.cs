using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HomeBank.Domain.DomainModels.StatisticModels;
using HomeBank.Domain.Infrastructure.Statistic;
using HomeBank.Domain.Queries;
using HomeBank.Presentation.Converters;
using HomeBank.Presentation.Enums;
using HomeBank.Presentation.Infrastructure;
using OxyPlot;
using OxyPlot.Series;

namespace HomeBank.Presentation.ViewModels
{
    public class StatisticViewModel : ViewModel
    {
        private readonly IStatisticService _statisticService;

        public override string ViewModelName => nameof(StatisticViewModel);

        public static IEnumerable<CategoryTypeFilter> CategoryTypes => Utils.CategoryTypes.Filters;

        private CategoryTypeFilter _type;
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

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                OnPropertyChanged();

                EventBus.Notify(EventType.CategoryStatisticFilterChanged);
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                OnPropertyChanged();

                EventBus.Notify(EventType.CategoryStatisticFilterChanged);
            }
        }

        public ObservableCollection<StatisticItemViewModel> CategoryStatisticItems { get; set; }
        
        public PlotModel GraphicModel { get; set; } 

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

        private StatisticViewModel(IEventBus eventBus, IStatisticService statisticService, CategoryStatistic categoryStatistic)
            : base(eventBus)
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
            
            GraphicModel = new PlotModel
            {
                Title = "Categories"
            };

            UpdateCategoryStatisticItems(categoryStatistic.StatisticItems);
            UpdateCategoryStatisticPieSeries(categoryStatistic.StatisticItems);
            UpdateTotal(categoryStatistic.Total);

            EventBus.EventOccured += EventBus_EventOccured;
        }

        private async void EventBus_EventOccured(EventType type, EventArgs args = null)
        {
            switch (type)
            {
                case EventType.CategoryStatisticFilterChanged:

                case EventType.TransactionOperationExecuted:
                case EventType.TransactionItemOperationExecuted:

                case EventType.CategoryOperationExecuted:
                case EventType.CategoryItemOperationExecuted:

                    var query = new CategoryStatisticQuery(
                        dateRangeQuery: new DateRangeQuery(StartDate, EndDate?.AddDays(1)),
                        type: Type.Convert());

                    var categoryStatistic = await _statisticService.GetCategoryStatisticAsync(query);
                    UpdateCategoryStatisticItems(categoryStatistic.StatisticItems);
                    UpdateCategoryStatisticPieSeries(categoryStatistic.StatisticItems);
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

        private void UpdateCategoryStatisticPieSeries(IEnumerable<CategoryStatisticItem> statisticItems)
        {
            if (statisticItems == null)
            {
                throw new ArgumentNullException(nameof(statisticItems));
            }
            
            GraphicModel.Series.Clear();

            var categoryStatisticPieSerie = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            foreach (var statisticItem in statisticItems)
            {
                var title = $"{statisticItem.Category.Name} - {statisticItem.Category.Description}";
                var cost = Convert.ToDouble(statisticItem.Cost);

                categoryStatisticPieSerie.Slices.Add(
                    new PieSlice(title, cost)
                    {
                        IsExploded = true
                    });
            }

            GraphicModel.Series.Add(categoryStatisticPieSerie);
        }

        private void UpdateTotal(decimal total)
        {
            Total = $"Total: {total}";
        }
    }
}
