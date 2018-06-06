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
                        var itemViewModel = new TransactionItemViewModel();
                        var itemView = new TransactionItemView(itemViewModel);

                        var viewModel = new TransactionViewModel();
                        var view = new TransactionView(viewModel);

                        viewModel.TransactionOperationExecuted += (s, args) =>
                        {
                            GridPrincipal.Children.Clear();
                            GridPrincipal.Children.Add(itemView);
                        };

                        itemViewModel.TransactionItemOperationExecuted += (s, args) =>
                        {
                            GridPrincipal.Children.Clear();
                            GridPrincipal.Children.Add(view);
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
                            category.BackExecuted += (backSender, backArgs) =>
                            {
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

        private void MoveCursorMenu(int index)
        {
            TransitioningContentSlide.OnApplyTemplate();

            var topOffset = index == -1 
                ? -60
                : 100 + (60 * index);

            GridCursor.Margin = new Thickness(0, topOffset  , 0, 0);
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
