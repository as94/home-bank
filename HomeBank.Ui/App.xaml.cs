using HomeBank.Presentaion.ViewModels;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Views;
using System;
using System.Windows;
using HomeBank.Presentaion.Infrastructure;
using HomeBank.Presentaion.Enums;
using HomeBank.Domain.Infrastructure;
using HomeBank.Data.Sqlite.Storages;
using HomeBank.Data.Sqlite.Infrastructure;
using System.Configuration;

namespace HomeBank.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string _dbFile;
        private ISessionFactoryProvider _sessionFactoryProvider;
        private ISessionProvider _sessionProvider;

        private ICategoryRepository _categoryRepository;
        private ITransactionRepository _transactionRepository;

        private IEventBus _eventBus = new EventBus();

        private MainViewModel _mainViewModel;
        private MainView _mainView;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _dbFile = ConfigurationManager.ConnectionStrings["HomeBankConnection"].ConnectionString;

            _sessionFactoryProvider = new SessionFactoryProvider(_dbFile);
            _sessionProvider = new SessionProvider(_sessionFactoryProvider);

            _categoryRepository = new SqliteCategoryRepository(_sessionProvider);
            _transactionRepository = new SqliteTransactionRepository(_sessionProvider);

            var categoryItemViewModel = new CategoryItemViewModel(_eventBus);
            var categoryViewModel = await CategoryViewModel.CreateAsync(_eventBus, _categoryRepository);

            var transactionItemViewModel = new TransactionItemViewModel(_eventBus, _transactionRepository, categoryViewModel.Categories);
            var transactionViewModel = await TransactionViewModel.CreateAsync(_eventBus, _categoryRepository, _transactionRepository);

            var childrenViewModels = new ViewModel[]
            {
                new HomeViewModel(_eventBus),
                transactionViewModel,
                categoryViewModel,
                new StatisticViewModel(_eventBus),
                new SettingsViewModel(_eventBus),
                categoryItemViewModel,
                transactionItemViewModel
            };

            if (_mainView == null)
            {
                _mainViewModel = new MainViewModel(
                    _eventBus,
                    childrenViewModels,
                    _categoryRepository,
                    _transactionRepository);

                transactionViewModel.Type = CategoryTypeFilter.All;
                transactionViewModel.Date = DateTime.Now;

                _mainView = new MainView(_mainViewModel);

                _mainView.Show();
            }
        }
    }
}
