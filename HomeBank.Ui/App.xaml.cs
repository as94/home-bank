using HomeBank.Data.Memory.Store;
using HomeBank.Domain.DomainModel;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.ViewModels;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HomeBank.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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

            // TODO: transaction categories not updated when updated category

            var childrenViewModels = new ViewModel[]
            {
                new HomeViewModel(),
                GetTransactionViewModel(categories, transactions),
                GetCategoryViewModel(categories),
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

                _mainView.Show();
            }
        }

        private TransactionViewModel GetTransactionViewModel(IEnumerable<Category> categories, IEnumerable<Transaction> transactions)
        {
            var viewModel = new TransactionViewModel(categories, transactions);

            EventHandler<TransactionOperationEventArgs> operationExecutedHandler = GetTransactionOperationExecutedHandler(viewModel);

            viewModel.TransactionOperationExecuted += async (s, args) =>
            {
                if (args.Transaction.OperationType == Presentaion.Enums.OperationType.Remove)
                {
                    await _mainViewModel.TransactionRepository.RemoveAsync(args.Transaction.Id);
                    viewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());
                    return;
                }

                var transactionItemViewModel = args.Transaction;
                transactionItemViewModel.TransactionItemOperationExecuted += operationExecutedHandler;
                transactionItemViewModel.BackExecuted += async (backSender, backArgs) =>
                {
                    viewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());

                    _mainViewModel.SelectedChildren = viewModel;
                };

                _mainViewModel.SelectedChildren = transactionItemViewModel;
            };

            return viewModel;
        }

        private CategoryViewModel GetCategoryViewModel(IEnumerable<Category> categories)
        {
            var viewModel = new CategoryViewModel(categories);

            EventHandler<CategoryOperationEventArgs> operationExecutedHandler = GetCategoryOperationExecutedHandler(viewModel);

            viewModel.CategoryOperationExecuted += async (s, args) =>
            {
                if (args.Category.OperationType == Presentaion.Enums.OperationType.Remove)
                {
                    await _mainViewModel.CategoryRepository.RemoveAsync(args.Category.Id);
                    viewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync());
                    return;
                }

                var categoryItemViewModel = args.Category;
                categoryItemViewModel.CategoryItemOperationExecuted += operationExecutedHandler;
                categoryItemViewModel.BackExecuted += async (backSender, backArgs) =>
                {
                    viewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync());

                    _mainViewModel.SelectedChildren = viewModel;
                };

                _mainViewModel.SelectedChildren = categoryItemViewModel;
            };

            return viewModel;
        }

        private EventHandler<TransactionOperationEventArgs> GetTransactionOperationExecutedHandler(TransactionViewModel viewModel)
        {
            return async (s, args) =>
            {
                switch (args.Transaction.OperationType)
                {
                    case Presentaion.Enums.OperationType.Add:
                        await _mainViewModel.TransactionRepository.CreateAsync(args.Transaction.ToDomain());
                        viewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());
                        break;

                    case Presentaion.Enums.OperationType.Edit:
                        await _mainViewModel.TransactionRepository.ChangeAsync(args.Transaction.ToDomain());
                        viewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());
                        break;

                    default:
                        break;
                }

                _mainViewModel.SelectedChildren = viewModel;
            };
        }

        private EventHandler<CategoryOperationEventArgs> GetCategoryOperationExecutedHandler(CategoryViewModel viewModel)
        {
            return async (s, args) =>
            {
                switch (args.Category.OperationType)
                {
                    case Presentaion.Enums.OperationType.Add:
                        await _mainViewModel.CategoryRepository.CreateAsync(args.Category.ToDomain());
                        viewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync());
                        break;

                    case Presentaion.Enums.OperationType.Edit:
                        await _mainViewModel.CategoryRepository.ChangeAsync(args.Category.ToDomain());
                        viewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync());
                        break;

                    default:
                        break;
                }

                _mainViewModel.SelectedChildren = viewModel;
            };
        }
    }
}
