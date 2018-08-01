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
        private readonly IDialogServiceFactory _yesNoDialogServiceFactory;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;

        public override string ViewModelName => nameof(TransactionViewModel);

        public static IEnumerable<CategoryTypeFilter> CategoryTypes => Utils.CategoryTypes.Filters;

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                OnPropertyChanged();

                EventBus.Notify(EventType.TransactionFilterChanged);
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate == value) return;
                _endDate = value;
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

        public static async Task<TransactionViewModel> CreateAsync(
            IEventBus eventBus,
            IDialogServiceFactory yesNoDialogServiceFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICategoryRepository categoryRepository,
            ITransactionRepository transactionRepository)
        {
            if (yesNoDialogServiceFactory == null)
            {
                throw new ArgumentNullException(nameof(yesNoDialogServiceFactory));
            }

            if (unitOfWorkFactory == null)
            {
                throw new ArgumentNullException(nameof(unitOfWorkFactory));
            }

            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }

            if (transactionRepository == null)
            {
                throw new ArgumentNullException(nameof(transactionRepository));
            }

            var categories = await categoryRepository.FindAsync();
            var transactions = await transactionRepository.FindAsync();

            return new TransactionViewModel(
                eventBus,
                yesNoDialogServiceFactory,
                unitOfWorkFactory,
                categoryRepository,
                transactionRepository,
                categories,
                transactions);
        }

        private TransactionViewModel(
            IEventBus eventBus,
            IDialogServiceFactory yesNoDialogServiceFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICategoryRepository categoryRepository,
            ITransactionRepository transactionRepository,
            IEnumerable<Category> categories,
            IEnumerable<Transaction> transactions)
            : base(eventBus)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            _yesNoDialogServiceFactory = yesNoDialogServiceFactory;

            _unitOfWorkFactory = unitOfWorkFactory;
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
                    UpdateTransactions(await _transactionRepository.FindAsync(
                        new TransactionQuery(
                            dateRangeQuery: new DateRangeQuery(StartDate, EndDate),
                            type: Type.Convert())));
                    break;

                case EventType.TransactionItemOperationExecuted:
                    await OnTransactionItemOperationExecuted(args);
                    UpdateCategories(await _categoryRepository.FindAsync());
                    UpdateTransactions(await _transactionRepository.FindAsync(
                        new TransactionQuery(
                            dateRangeQuery: new DateRangeQuery(StartDate, EndDate),
                            type: Type.Convert())));
                    break;

                case EventType.TransactionFilterChanged:
                case EventType.TransactionBackExecuted:
                    UpdateTransactions(await _transactionRepository.FindAsync(
                        new TransactionQuery(
                            dateRangeQuery: new DateRangeQuery(StartDate, EndDate),
                            type: Type.Convert())));
                    break;

                case EventType.CategoryOperationExecuted:
                case EventType.CategoryItemOperationExecuted:
                    UpdateCategories(await _categoryRepository.FindAsync());
                    UpdateTransactions(await _transactionRepository.FindAsync(
                        new TransactionQuery(
                            dateRangeQuery: new DateRangeQuery(StartDate, EndDate),
                            type: Type.Convert())));
                    break;
            }
        }

        private async Task OnTransactionOperationExecuted(EventArgs args)
        {
            if (args is TransactionOperationEventArgs transactionOperationArgs && transactionOperationArgs.Transaction.OperationType == OperationType.Remove)
            {
                using (var unitOfWork = _unitOfWorkFactory.Create())
                {
                    await _transactionRepository.RemoveAsync(transactionOperationArgs.Transaction.Id);
                    await unitOfWork.CommitAsync();
                }
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
                    using (var unitOfWork = _unitOfWorkFactory.Create())
                    {
                        await _transactionRepository.CreateAsync(transaction);
                        await unitOfWork.CommitAsync();
                    }
                    break;

                case OperationType.Edit:
                    using (var unitOfWork = _unitOfWorkFactory.Create())
                    {
                        await _transactionRepository.ChangeAsync(transaction);
                        await unitOfWork.CommitAsync();
                    }
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
                    CategoryItemViewModel = Categories.FirstOrDefault(c => c.Id == transaction.Category?.Id)
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

                    if (_yesNoDialogServiceFactory.Create().ShowDialog)
                    {
                        SelectedTransaction.OperationType = OperationType.Remove;
                        var args = new TransactionOperationEventArgs(SelectedTransaction);
                        EventBus.Notify(EventType.TransactionOperationExecuted, args);
                    }
                }));
            }
        }
    }
}
