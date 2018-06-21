using HomeBank.Domain.DomainModel;
using HomeBank.Presentaion.Enums;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class TransactionViewModel : ViewModel
    {
        public override string ViewModelName => nameof(TransactionViewModel);

        public event EventHandler<TransactionOperationEventArgs> TransactionOperationExecuted;
        public void OnTransactionOperationExecuted(TransactionOperationEventArgs args)
        {
            TransactionOperationExecuted?.Invoke(this, args);
        }

        public ObservableCollection<TransactionItemViewModel> Transactions { get; set; }
        public TransactionItemViewModel SelectedTransaction { get; set; }

        public ObservableCollection<CategoryItemViewModel> Categories { get; set; }

        public TransactionViewModel(IEnumerable<Category> categories, IEnumerable<Transaction> transactions)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            Categories = new ObservableCollection<CategoryItemViewModel>(categories.Select(category =>
                new CategoryItemViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Type = category.Type
                }));

            Transactions = new ObservableCollection<TransactionItemViewModel>();

            UpdateTransactions(transactions);
        }

        public void UpdateTransactions(IEnumerable<Transaction> transactions)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            Transactions.Clear();

            foreach (var transaction in transactions)
            {
                var view = new TransactionItemViewModel(Categories)
                {
                    Id = transaction.Id,
                    Date = transaction.Date,
                    Amount = transaction.Amount,
                    CategoryItemViewModel = Categories.FirstOrDefault(c => c.Id == transaction.Category.Id)
                };

                Transactions.Add(view);
            }

            SelectedTransaction = Transactions.Count > 0 ? Transactions[0] : null;
        }

        private ICommand _addTransactionCommand;
        public ICommand AddTransactionCommand
        {
            get
            {
                return _addTransactionCommand ?? (_addTransactionCommand = new ActionCommand(vm =>
                {
                    OnTransactionOperationExecuted(new TransactionOperationEventArgs(new TransactionItemViewModel(OperationType.Add, Categories)));
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
                    SelectedTransaction.OperationType = OperationType.Edit;
                    OnTransactionOperationExecuted(new TransactionOperationEventArgs(SelectedTransaction));
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
                    SelectedTransaction.OperationType = OperationType.Remove;
                    OnTransactionOperationExecuted(new TransactionOperationEventArgs(SelectedTransaction));
                }));
            }
        }
    }
}
