using HomeBank.Data.Memory.Store;
using HomeBank.Domain.DomainModels;
using HomeBank.Presentaion.ViewModels;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using HomeBank.Presentaion.Infrastructure;
using HomeBank.Presentaion.Enums;

namespace HomeBank.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IEventBus _eventBus = new EventBus();

        private MainViewModel _mainViewModel;
        private MainView _mainView;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var categories = new List<Category>
            {
                new Category(Guid.NewGuid(), "Work", "Programmer", Domain.Enums.CategoryType.Income),
                new Category(Guid.NewGuid(), "Business", "Store", Domain.Enums.CategoryType.Income),

                new Category(Guid.NewGuid(), "Products", "Apple", Domain.Enums.CategoryType.Expenditure),
                new Category(Guid.NewGuid(), "Products", "Orange", Domain.Enums.CategoryType.Expenditure),
            };

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2018, 2, 3), 10, categories[0]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 2, 4), 30, categories[1]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 3, 20), 50, categories[2]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 6, 9), 1000, categories[3]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 6, 10), 100, categories[3]),
            };

            var categoryRepository = new CategoryRepository(categories);
            var transactionRepository = new TransactionRepository(transactions);

            var categoryItemViewModel = new CategoryItemViewModel(_eventBus);
            var categoryViewModel = new CategoryViewModel(_eventBus, categoryRepository, categories);

            var transactionItemViewModel = new TransactionItemViewModel(_eventBus, transactionRepository, categoryViewModel.Categories);
            var transactionViewModel = new TransactionViewModel(_eventBus, transactionRepository, categoryRepository, categories, transactions);

            var childrenViewModels = new ViewModel[]
            {
                new HomeViewModel(_eventBus),
                transactionViewModel,
                categoryViewModel,
                new StatisticViewModel(_eventBus),
                new AccountViewModel(_eventBus),
                new SettingsViewModel(_eventBus),
                categoryItemViewModel,
                transactionItemViewModel
            };

            if (_mainView == null)
            {
                _mainViewModel = new MainViewModel(
                    _eventBus,
                    childrenViewModels,
                    categoryRepository,
                    transactionRepository);

                transactionViewModel.Type = CategoryTypeFilter.All;
                transactionViewModel.Date = DateTime.Now;

                _mainView = new MainView(_mainViewModel);

                _mainView.Show();
            }
        }
    }
}
