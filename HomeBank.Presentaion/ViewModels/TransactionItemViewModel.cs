using HomeBank.Domain.DomainModel;
using HomeBank.Presentaion.Enums;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class TransactionItemViewModel
    {
        public event EventHandler<TransactionOperationEventArgs> TransactionItemOperationExecuted;
        public void OnTransactionItemOperationExecuted(TransactionOperationEventArgs args)
        {
            TransactionItemOperationExecuted?.Invoke(this, args);
        }

        public event EventHandler BackExecuted;
        public void OnBackExecuted()
        {
            BackExecuted?.Invoke(this, new EventArgs());
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public CategoryItemViewModel CategoryItemViewModel { get; set; }

        public OperationType OperationType { get; set; }

        public IEnumerable<CategoryItemViewModel> Categories { get; }

        public TransactionItemViewModel(IEnumerable<CategoryItemViewModel> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            Categories = categories;
        }

        public TransactionItemViewModel(OperationType operationType)
        {
            OperationType = operationType;
        }

        private ICommand _transactionOperationCommand;
        public ICommand TransactionOperationCommand
        {
            get
            {
                return _transactionOperationCommand ?? (_transactionOperationCommand = new ActionCommand(vm =>
                {
                    if (OperationType == OperationType.Add)
                    {
                        Id = Guid.NewGuid();
                    }

                    OnTransactionItemOperationExecuted(new TransactionOperationEventArgs(this));
                }));
            }
        }

        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new ActionCommand(vm =>
                {
                    OnBackExecuted();
                }));
            }
        }

        public Domain.DomainModel.Transaction ToDomain()
        {
            return new Domain.DomainModel.Transaction(Id, Date, Amount, CategoryItemViewModel.ToDomain());
        }
    }
}
