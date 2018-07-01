using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.ViewModels;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Enums;
using System;
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
            if (mainViewModel == null)
            {
                throw new ArgumentNullException(nameof(mainViewModel));
            }

            _mainViewModel = mainViewModel;

            InitializeComponent();

            DataContext = mainViewModel;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var menuItemIdx = ListViewMenu.SelectedIndex;
            MoveCursorMenu(menuItemIdx);

            if (menuItemIdx != -1)
            {
                _mainViewModel.MenuItemShowCommand.Execute(menuItemIdx);
            }
        }

        private void MoveCursorMenu(int index)
        {
            TransitioningContentSlide.OnApplyTemplate();

            var topOffset = GetTopOffset(index);

            GridCursor.Margin = new Thickness(0, topOffset, 0, 0);
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
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.UnselectAll();
        }
    }
}
