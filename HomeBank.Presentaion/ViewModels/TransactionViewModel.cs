using HomeBank.Presentaion.Infrastructure;
using System;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class TransactionViewModel
    {
        public event EventHandler TransactionOperationExecuted;
        public void OnTransactionOperationExecuted()
        {
            TransactionOperationExecuted.Invoke(this, new EventArgs());
        }

        public TransactionViewModel()
        {
        }

        private ICommand _addTransactionCommand;
        public ICommand AddTransactionCommand
        {
            get
            {
                return _addTransactionCommand ?? (_addTransactionCommand = new ActionCommand(vm =>
                {
                    OnTransactionOperationExecuted();
                }));
            }
        }

        private ICommand _editTransactionCommand;
        public ICommand EditTransactionCommand
        {
            get
            {
                return _editTransactionCommand ?? (_editTransactionCommand = new ActionCommand(vm =>
                {
                    OnTransactionOperationExecuted();
                }));
            }
        }

        private ICommand _removeTransactionCommand;
        public ICommand RemoveTransactionCommand
        {
            get
            {
                return _removeTransactionCommand ?? (_removeTransactionCommand = new ActionCommand(vm =>
                {
                    OnTransactionOperationExecuted();
                }));
            }
        }
    }
}
