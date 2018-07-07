using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Queries;
using HomeBank.Presentaion.Converters;
using HomeBank.Presentaion.Enums;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class TransactionViewModel : ViewModel
    {
        private ITransactionRepository _transactionRepository;
        private ICategoryRepository _categoryRepository;

        public override string ViewModelName => nameof(TransactionViewModel);

        public static IEnumerable<CategoryTypeFilter> CategoryTypes => Utils.CategoryTypes.Filters;

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged();

                EventBus.Notify(EventType.TransactionFilterChanged);
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

                EventBus.Notify(EventType.TransactionFilterChanged);
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

        public TransactionViewModel(
            IEventBus eventBus,
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            IEnumerable<Category> categories,
            IEnumerable<Transaction> transactions)
            : base(eventBus)
        {
            if (transactionRepository == null)
            {
                throw new ArgumentNullException(nameof(transactionRepository));
            }

            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }

            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;

            Categories = new ObservableCollection<CategoryItemViewModel>();
            Transactions = new ObservableCollection<TransactionItemViewModel>();

            UpdateCategories(categories);
            UpdateTransactions(transactions);

            EventBus.EventOccured += EventBus_EventOccured;
        }

        private async void EventBus_EventOccured(EventType type, EventArgs args = null)
        {
            switch (type)
            {
                case EventType.TransactionOperationExecuted:
                    await OnTransactionOperationExecuted(args);
                    UpdateTransactions(await _transactionRepository.FindAsync(new TransactionQuery(Date, Type.Convert())));
                    break;

                case EventType.TransactionItemOperationExecuted:
                    await OnTransactionItemOperationExecuted(args);
                    UpdateCategories(await _categoryRepository.FindAsync());
                    UpdateTransactions(await _transactionRepository.FindAsync(new TransactionQuery(Date, Type.Convert())));
                    break;

                case EventType.TransactionFilterChanged:
                case EventType.TransactionBackExecuted:
                    UpdateTransactions(await _transactionRepository.FindAsync(new TransactionQuery(Date, Type.Convert())));
                    break;

                case EventType.CategoryOperationExecuted:
                case EventType.CategoryItemOperationExecuted:
                    UpdateCategories(await _categoryRepository.FindAsync());
                    UpdateTransactions(await _transactionRepository.FindAsync(new TransactionQuery(Date, Type.Convert())));
                    break;
            }
        }

        private async Task OnTransactionOperationExecuted(EventArgs args)
        {
            var transactionOperationArgs = args as TransactionOperationEventArgs;
            if (transactionOperationArgs != null && transactionOperationArgs.Transaction.OperationType == OperationType.Remove)
            {
                await _transactionRepository.RemoveAsync(transactionOperationArgs.Transaction.Id);
            }
        }

        private async Task OnTransactionItemOperationExecuted(EventArgs args)
        {
            var transactionOperationArgs = args as TransactionOperationEventArgs;
            if (transactionOperationArgs == null)
            {
                return;
            }

            var transaction = transactionOperationArgs.Transaction.ToDomain();
            switch (transactionOperationArgs.Transaction.OperationType)
            {
                case OperationType.Add:
                    await _transactionRepository.CreateAsync(transaction);
                    break;

                case OperationType.Edit:
                    await _transactionRepository.ChangeAsync(transaction);
                    break;
            }
        }

        private void UpdateCategories(IEnumerable<Category> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            Categories.Clear();

            foreach (var category in categories)
            {
                var view = new CategoryItemViewModel(EventBus)
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Type = category.Type
                };

                Categories.Add(view);
            }
        }

        private void UpdateTransactions(IEnumerable<Transaction> transactions)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            Transactions.Clear();

            foreach (var transaction in transactions)
            {
                var view = new TransactionItemViewModel(EventBus, _transactionRepository, Categories)
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
                    var args = new TransactionOperationEventArgs(new TransactionItemViewModel(
                        EventBus,
                        _transactionRepository,
                        OperationType.Add,
                        Categories));

                    EventBus.Notify(EventType.TransactionOperationExecuted, args);
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
                    if (SelectedTransaction == null)
                    {
                        return;
                    }

                    SelectedTransaction.OperationType = OperationType.Edit;
                    var args = new TransactionOperationEventArgs(SelectedTransaction);
                    EventBus.Notify(EventType.TransactionOperationExecuted, args);
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
                    if (SelectedTransaction == null)
                    {
                        return;
                    }

                    SelectedTransaction.OperationType = OperationType.Remove;
                    var args = new TransactionOperationEventArgs(SelectedTransaction);
                    EventBus.Notify(EventType.TransactionOperationExecuted, args);
                }));
            }
        }
    }
}
