using HomeBank.Presentaion.Infrastructure;
using System.Windows;

namespace HomeBank.Ui.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for ErrorDialogView.xaml
    /// </summary>
    public partial class ErrorDialogView : Window, IDialogService
    {
        public ErrorDialogView(string errorMessage)
        {
            InitializeComponent();

            error.Text = errorMessage;
        }

        private bool _showDialog;
        bool IDialogService.ShowDialog => _showDialog;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _showDialog = true;
        }
    }
}
