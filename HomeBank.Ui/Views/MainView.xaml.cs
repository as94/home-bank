using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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

        private void DragStripGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void WindowMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
                    GridPrincipal.Children.Add(new TransactionView());
                    break;

                case MainMenuItems.Category:
                    GridPrincipal.Children.Add(new CategoryView());
                    break;

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
