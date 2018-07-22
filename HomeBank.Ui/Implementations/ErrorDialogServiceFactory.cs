using HomeBank.Presentaion.Infrastructure;
using HomeBank.Ui.Views.Dialogs;
using System.Windows;

namespace HomeBank.Ui.Implementations
{
    internal sealed class ErrorDialogServiceFactory : IDialogServiceFactory
    {
        public IDialogService Create(string text = null)
        {
            var view = new ErrorDialogView(text);
            view.ShowDialog();
            return view;
        }
    }
}
