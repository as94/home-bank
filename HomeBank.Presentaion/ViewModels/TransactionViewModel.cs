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

        public static IEnumerable<CategoryTypeFilter> CategoryTypes => Utils.CategoryTypes.Filters;

        public event EventHandler<TransactionOperationEventArgs> TransactionOperationExecuted;
        public void OnTransactionOperationExecuted(TransactionOperationEventArgs args)
        {
            TransactionOperationExecuted?.Invoke(this, args);

            Type = CategoryTypeFilter.All;
        }

        public event EventHandler FilterChanged;
        public void OnFilterChanged()
        {
            FilterChanged?.Invoke(this, EventArgs.Empty);
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged();
                OnFilterChanged();
            }
        }

        private CategoryTypeFilter _type;
        public CategoryTypeFilter Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                OnPropertyChanged();
                OnFilterChanged();
            }
        }

        //private CategoryItemViewModel _category;
        //public CategoryItemViewModel Category
        //{
        //    get => _category;
        //    set
        //    {
        //        if (_category == value) return;
        //        _category = value;
        //        OnPropertyChanged();
        //        OnFilterChanged();
        //    }
        //}

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

            Categories = new ObservableCollection<CategoryItemViewModel>();
            Transactions = new ObservableCollection<TransactionItemViewModel>();

            UpdateCategories(categories);
            UpdateTransactions(transactions);
        }

        public void UpdateCategories(IEnumerable<Category> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            Categories.Clear();

            foreach (var category in categories)
            {
                var view = new CategoryItemViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Type = category.Type
                };

                Categories.Add(view);
            }
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
