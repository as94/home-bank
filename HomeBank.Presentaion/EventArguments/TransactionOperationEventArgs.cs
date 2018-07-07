using HomeBank.Presentaion.ViewModels;
using System;

namespace HomeBank.Presentaion.EventArguments
{
    public sealed class TransactionOperationEventArgs : EventArgs
    {
        public TransactionOperationEventArgs(TransactionItemViewModel transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            Transaction = transaction;
        }

        public TransactionItemViewModel Transaction { get; set; }
    }
}
