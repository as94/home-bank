using System.Windows;
using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Ui.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AreYouSureDialogView.xaml
    /// </summary>
    public partial class AreYouSureDialogView : Window, IDialogService
    {
        public AreYouSureDialogView()
        {
            InitializeComponent();
        }

        private bool _showDialog;
        bool IDialogService.ShowDialog => _showDialog;

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            _showDialog = true;
        }
    }
}
