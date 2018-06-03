using HomeBank.Presentaion.ViewModels;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Enums;
using System.Windows;
using System.Windows.Controls;

namespace HomeBank.Ui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                        var itemViewModel = new CategoryItemViewModel();
                        var itemView = new CategoryItemView(itemViewModel);

                        var viewModel = new CategoryViewModel();
                        var view = new CategoryView(viewModel);

                        viewModel.CategoryOperationExecuted += (s, args) =>
                        {
                            GridPrincipal.Children.Clear();
                            GridPrincipal.Children.Add(itemView);
                        };

                        itemViewModel.CategoryItemOperationExecuted += (s, args) =>
                        {
                            GridPrincipal.Children.Clear();
                            GridPrincipal.Children.Add(view);
                        };

                        GridPrincipal.Children.Add(view);
                        break;
                    }
                case MainMenuItems.Statistic:
                    GridPrincipal.Children.Add(new StatisticView());
                    break;
            }
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
