using HomeBank.Presentaion.Infrastructure;
using HomeBank.Ui.Views.Dialogs;
using System;

namespace HomeBank.Ui.Implementations
{
    internal sealed class YesNoDialogServiceFactory : IDialogServiceFactory
    {
        public IDialogService Create(string text)
        {
            var view = new AreYouSureDialogView();
            view.ShowDialog();
            return view;
        }
    }
}
