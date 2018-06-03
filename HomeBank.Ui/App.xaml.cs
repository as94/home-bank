using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Views;
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

            if (_mainView == null)
            {
                _mainViewModel = new MainViewModel();
                _mainView = new MainView(_mainViewModel);

                _mainView.Show();
            }
        }
    }
}
