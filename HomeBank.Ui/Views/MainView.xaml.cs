using HomeBank.Presentation.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HomeBank.Ui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private bool _isOpenMenu = false;

        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;

            _isOpenMenu = true;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;

            _isOpenMenu = false;
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selected = (ListViewItem)ListViewMenu.SelectedItem;
            if (selected != null)
            {
                var name = selected.Name;

                switch (name)
                {

                    case "ButtonHome":
                        ViewHome.Visibility = Visibility.Visible;
                        ViewTransaction.Visibility = Visibility.Collapsed;
                        ViewCategory.Visibility = Visibility.Collapsed;
                        break;

                    case "ButtonTransaction":
                        ViewHome.Visibility = Visibility.Collapsed;
                        ViewTransaction.Visibility = Visibility.Visible;
                        ViewCategory.Visibility = Visibility.Collapsed;
                        break;

                    case "ButtonCategory":
                        ViewHome.Visibility = Visibility.Collapsed;
                        ViewTransaction.Visibility = Visibility.Collapsed;
                        ViewCategory.Visibility = Visibility.Visible;
                        break;

                    default:
                        throw new InvalidOperationException("Unknown menu item");
                }

                if (_isOpenMenu)
                {
                    ButtonCloseMenu.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            }
        }
    }
}
