using HomeBank.Data.Memory.Store;
using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Queries;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.ViewModels;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using HomeBank.Presentaion.Converters;

namespace HomeBank.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TransactionViewModel _transactionViewModel;
        private CategoryViewModel _categoryViewModel;

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

            _transactionViewModel = new TransactionViewModel(categories, transactions);
            InitializeTransactionViewModel();

            _categoryViewModel = new CategoryViewModel(categories);
            InitializeCategoryViewModel();

            var childrenViewModels = new ViewModel[]
            {
                new HomeViewModel(),
                _transactionViewModel,
                _categoryViewModel,
                new StatisticViewModel(),
                new AccountViewModel(),
                new SettingsViewModel()
            };

            if (_mainView == null)
            {
                _mainViewModel = new MainViewModel(
                    childrenViewModels,
                    categoryRepository,
                    transactionRepository);

                _mainView = new MainView(_mainViewModel);

                _transactionViewModel.Type = Presentaion.Enums.CategoryTypeFilter.All;
                _transactionViewModel.Date = DateTime.Now;

                _mainView.Show();
            }
        }

        private void InitializeTransactionViewModel()
        {
            EventHandler<TransactionOperationEventArgs> operationExecutedHandler = GetTransactionOperationExecutedHandler();

            _transactionViewModel.TransactionOperationExecuted += async (s, args) =>
            {
                if (args.Transaction.OperationType == Presentaion.Enums.OperationType.Remove)
                {
                    await _mainViewModel.TransactionRepository.RemoveAsync(args.Transaction.Id);
                    _transactionViewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());
                    return;
                }

                var transactionItemViewModel = args.Transaction;
                transactionItemViewModel.TransactionItemOperationExecuted += operationExecutedHandler;
                transactionItemViewModel.BackExecuted += async (backSender, backArgs) =>
                {
                    _transactionViewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());

                    _mainViewModel.SelectedChildren = _transactionViewModel;
                };

                _mainViewModel.SelectedChildren = transactionItemViewModel;
            };

            _transactionViewModel.FilterChanged += async (s, args) =>
            {
                var date = _transactionViewModel.Date;
                var type = _transactionViewModel.Type.Convert();
                //var category = _transactionViewModel.Category?.ToDomain();

                //var categoryQuery = new CategoryQuery(type);
                var transactionQuery = new TransactionQuery(date, type);

                //var categories = await _mainViewModel.CategoryRepository.FindAsync(categoryQuery);
                var transactions = await _mainViewModel.TransactionRepository.FindAsync(transactionQuery);

                //_transactionViewModel.UpdateCategories(categories);
                _transactionViewModel.UpdateTransactions(transactions);
            };
        }

        private void InitializeCategoryViewModel()
        {
            EventHandler<CategoryOperationEventArgs> operationExecutedHandler = GetCategoryOperationExecutedHandler();

            _categoryViewModel.CategoryOperationExecuted += async (s, args) =>
            {
                if (args.Category.OperationType == Presentaion.Enums.OperationType.Remove)
                {
                    await _mainViewModel.CategoryRepository.RemoveAsync(args.Category.Id);
                    await UpdateCategoriesAsync();

                    return;
                }

                var categoryItemViewModel = args.Category;
                categoryItemViewModel.CategoryItemOperationExecuted += operationExecutedHandler;
                categoryItemViewModel.BackExecuted += async (backSender, backArgs) =>
                {
                    _categoryViewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync());

                    _mainViewModel.SelectedChildren = _categoryViewModel;
                };

                _mainViewModel.SelectedChildren = categoryItemViewModel;
            };

            _categoryViewModel.FilterChanged += async (s, args) =>
            {
                var query = new CategoryQuery(_categoryViewModel.Type.Convert());
                _categoryViewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync(query));
            };
        }

        private EventHandler<TransactionOperationEventArgs> GetTransactionOperationExecutedHandler()
        {
            return async (s, args) =>
            {
                switch (args.Transaction.OperationType)
                {
                    case Presentaion.Enums.OperationType.Add:
                        await _mainViewModel.TransactionRepository.CreateAsync(args.Transaction.ToDomain());
                        await UpdateTransactionsAsync();
                        break;

                    case Presentaion.Enums.OperationType.Edit:
                        await _mainViewModel.TransactionRepository.ChangeAsync(args.Transaction.ToDomain());
                        await UpdateTransactionsAsync();
                        break;

                    default:
                        break;
                }

                _mainViewModel.SelectedChildren = _transactionViewModel;
            };
        }

        private EventHandler<CategoryOperationEventArgs> GetCategoryOperationExecutedHandler()
        {
            return async (s, args) =>
            {
                switch (args.Category.OperationType)
                {
                    case Presentaion.Enums.OperationType.Add:
                        await _mainViewModel.CategoryRepository.CreateAsync(args.Category.ToDomain());
                        await UpdateCategoriesAsync();
                        break;

                    case Presentaion.Enums.OperationType.Edit:
                        await _mainViewModel.CategoryRepository.ChangeAsync(args.Category.ToDomain());
                        await UpdateCategoriesAsync();
                        break;

                    default:
                        break;
                }

                _mainViewModel.SelectedChildren = _categoryViewModel;
            };
        }

        private async Task UpdateCategoriesAsync()
        {
            var categories = await _mainViewModel.CategoryRepository.FindAsync();
            _categoryViewModel.UpdateCategories(categories);
            _transactionViewModel.UpdateCategories(categories);

            await UpdateTransactionsAsync();
        }

        private async Task UpdateTransactionsAsync()
        {
            var transactions = await _mainViewModel.TransactionRepository.FindAsync();
            _transactionViewModel.UpdateTransactions(transactions);
        }
    }
}
