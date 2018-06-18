using HomeBank.Domain.Infrastructure;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.ViewModels;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Enums;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HomeBank.Ui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel _mainViewModel;

        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;

            DataContext = mainViewModel;
        }

        private async void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            GridPrincipal.Children.Clear();
            
            switch ((MainMenuItems)index)
            {
                case MainMenuItems.Home:
                    GridPrincipal.Children.Add(new HomeView());
                    break;

                case MainMenuItems.Transaction:
                    {
                        var viewModel = new TransactionViewModel(
                            categories: await _mainViewModel.CategoryRepository.FindAsync(),
                            transactions: await _mainViewModel.TransactionRepository.FindAsync());

                        var view = new TransactionView(viewModel);

                        EventHandler<TransactionOperationEventArgs> operationExecutedHandler = GetTransactionOperationExecutedHandler(viewModel, view);

                        viewModel.TransactionOperationExecuted += async (s, args) =>
                        {
                            if (args.Transaction.OperationType == Presentaion.Enums.OperationType.Remove)
                            {
                                await _mainViewModel.TransactionRepository.RemoveAsync(args.Transaction.Id);
                                viewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());
                                return;
                            }

                            var transaction = args.Transaction;
                            transaction.TransactionItemOperationExecuted += operationExecutedHandler;
                            transaction.BackExecuted += async (backSender, backArgs) =>
                            {
                                viewModel.UpdateTransactions(await _mainViewModel.TransactionRepository.FindAsync());

                                GridPrincipal.Children.Clear();
                                GridPrincipal.Children.Add(view);
                            };

                            var itemView = new TransactionItemView(transaction);

                            GridPrincipal.Children.Clear();
                            GridPrincipal.Children.Add(itemView);
                        };

                        GridPrincipal.Children.Add(view);
                        break;
                    }

                case MainMenuItems.Category:
                    {
                        var viewModel = new CategoryViewModel(await _mainViewModel.CategoryRepository.FindAsync());
                        var view = new CategoryView(viewModel);

                        EventHandler<CategoryOperationEventArgs> operationExecutedHandler = GetCategoryOperationExecutedHandler(viewModel, view);

                        viewModel.CategoryOperationExecuted += async (s, args) =>
                        {
                            if (args.Category.OperationType == Presentaion.Enums.OperationType.Remove)
                            {
                                await _mainViewModel.CategoryRepository.RemoveAsync(args.Category.Id);
                                viewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync());
                                return;
                            }

                            var category = args.Category;
                            category.CategoryItemOperationExecuted += operationExecutedHandler;
                            category.BackExecuted += async (backSender, backArgs) =>
                            {
                                viewModel.UpdateCategories(await _mainViewModel.CategoryRepository.FindAsync());

                                GridPrincipal.Children.Clear();
                                GridPrincipal.Children.Add(view);
                            };

                            var itemView = new CategoryItemView(category);

                            GridPrincipal.Children.Clear();
                            GridPrincipal.Children.Add(itemView);
                        };

                        GridPrincipal.Children.Add(view);
                        break;
                    }

                case MainMenuItems.Statistic:
                    GridPrincipal.Children.Add(new StatisticView());
                    break;
            }
        }

        private EventHandler<CategoryOperationEventArgs> GetCategoryOperationExecutedHandler(CategoryViewModel viewModel, CategoryView view)
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

                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(view);
            };
        }

        private EventHandler<TransactionOperationEventArgs> GetTransactionOperationExecutedHandler(TransactionViewModel viewModel, TransactionView view)
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

                GridPrincipal.Children.Clear();
                GridPrincipal.Children.Add(view);
            };
        }

        private void MoveCursorMenu(int index)
        {
            TransitioningContentSlide.OnApplyTemplate();

            var topOffset = GetTopOffset(index);

            GridCursor.Margin = new Thickness(0, topOffset  , 0, 0);
        }

        private static int GetTopOffset(int index)
        {
            var emptyItemIndex = -1;
            var nonVisibleOffset = -60;

            var fixedItemOffset = 100;
            var itemOffsetFactor = 60;

            var visibleOffset = fixedItemOffset + (itemOffsetFactor * index);

            return index == emptyItemIndex ? nonVisibleOffset : visibleOffset;
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.UnselectAll();

            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new AccountView());
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.UnselectAll();

            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new SettingView());
        }
    }
}
