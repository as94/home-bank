using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class TransactionItemViewModel
    {
        public event EventHandler TransactionItemOperationExecuted;
        public void OnTransactionItemOperationExecuted()
        {
            TransactionItemOperationExecuted.Invoke(this, new EventArgs());
        }

        private ICommand _transactionOperationCommand;
        public ICommand TransactionOperationCommand
        {
            get
            {
                return _transactionOperationCommand ?? (_transactionOperationCommand = new ActionCommand(vm =>
                {
                    OnTransactionItemOperationExecuted();
                }));
            }
        }
    }
}
