using HomeBank.Ui.Views.Dialogs;
using HomeBank.Presentation.Infrastructure;

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
