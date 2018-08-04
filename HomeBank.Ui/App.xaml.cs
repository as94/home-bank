using HomeBank.Ui.Views;
using System;
using System.Windows;
using HomeBank.Domain.Infrastructure;
using HomeBank.Data.Sqlite.Storages;
using HomeBank.Data.Sqlite.Infrastructure;
using System.Configuration;
using HomeBank.Domain.Infrastructure.Statistic;
using HomeBank.Ui.Implementations;
using log4net;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using log4net.Config;
using HomeBank.Data.Sqlite.UnitOfWork;
using HomeBank.Presentation.Enums;
using HomeBank.Presentation.Infrastructure;
using HomeBank.Presentation.ViewModels;

namespace HomeBank.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool _isOwner;
        private Mutex _appMutex;
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        private string _dbFile;
        private ISessionFactoryProvider _sessionFactoryProvider;
        private ISessionProvider _sessionProvider;

        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ICategoryRepository _categoryRepository;
        private ITransactionRepository _transactionRepository;

        private IStatisticService _statisticService;

        private readonly IEventBus _eventBus = new EventBus();
        private readonly IDialogServiceFactory _yesNoDialogServiceFactory = new YesNoDialogServiceFactory();
        private readonly IDialogServiceFactory _errorDialogServiceFactory = new ErrorDialogServiceFactory();

        private MainViewModel _mainViewModel;
        private MainView _mainView;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var appId = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();
            _appMutex = new Mutex(initiallyOwned: true, name: appId, createdNew: out _isOwner);

            if (!_isOwner)
            {
                MessageBox.Show("The application is already running.");
                Current.Shutdown();
                return;
            }

            XmlConfigurator.Configure();

            base.OnStartup(e);

            _dbFile = ConfigurationManager.ConnectionStrings["HomeBankConnection"].ConnectionString;

            _sessionFactoryProvider = new SessionFactoryProvider(_dbFile);
            _sessionProvider = new SessionProvider(_sessionFactoryProvider);

            _unitOfWorkFactory = new UnitOfWorkFactory(_sessionProvider);
            _categoryRepository = new SqliteCategoryRepository(_sessionProvider);
            _transactionRepository = new SqliteTransactionRepository(_sessionProvider);

            _statisticService = new StatisticService(_transactionRepository);

            var categoryItemViewModel = new CategoryItemViewModel(_eventBus);
            var categoryViewModel = await CategoryViewModel.CreateAsync(
                _eventBus,
                _yesNoDialogServiceFactory,
                _errorDialogServiceFactory,
                _unitOfWorkFactory,
                _categoryRepository);

            var transactionItemViewModel = new TransactionItemViewModel(_eventBus, _transactionRepository, categoryViewModel.Categories);
            var transactionViewModel = await TransactionViewModel.CreateAsync(
                _eventBus,
                _yesNoDialogServiceFactory,
                _unitOfWorkFactory,
                _categoryRepository,
                _transactionRepository);

            var statisticViewModel = await StatisticViewModel.CreateAsync(_eventBus, _statisticService);
            statisticViewModel.Type = CategoryTypeFilter.All;
            statisticViewModel.StartDate = DateTime.Now;
            statisticViewModel.EndDate = DateTime.Now;

            var childrenViewModels = new ViewModel[]
            {
                transactionViewModel,
                categoryViewModel,
                statisticViewModel,
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
                transactionViewModel.StartDate = DateTime.Now;
                transactionViewModel.EndDate = DateTime.Now;

                _mainView = new MainView(_mainViewModel);

                _mainView.Show();
            }

            HandleExceptions();
        }

        private void HandleExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    OnException(ex);
                }
            };

            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                var ex = e.Exception;
                if (ex != null)
                {
                    OnException(ex);
                }

                e.Handled = true;
            };
        }

        private void OnException(Exception ex)
        {
            Log.Fatal(ex.Message, ex);

            var text = "Error occured.\nFile \"err.txt\" contains details.";
            if (_errorDialogServiceFactory.Create(text).ShowDialog)
            {
                Application.Current.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_appMutex != null)
            {
                if (_isOwner)
                {
                    _appMutex.ReleaseMutex();
                }

                _appMutex.Dispose();
            }

            base.OnExit(e);
        }
    }
}
