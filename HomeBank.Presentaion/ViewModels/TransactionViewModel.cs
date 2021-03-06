﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Infrastructure;
using HomeBank.Domain.Queries;
using HomeBank.Presentation.Converters;
using HomeBank.Presentation.Enums;
using HomeBank.Presentation.EventArguments;
using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Presentation.ViewModels
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

        private CategoryItemViewModel _selectedCategory;
        public CategoryItemViewModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value) return;
                _selectedCategory = value;
                OnPropertyChanged();
        
                EventBus.Notify(EventType.TransactionFilterChanged);
            }
        }

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
            var query = new TransactionQuery(
                dateRangeQuery: new DateRangeQuery(StartDate, EndDate?.AddDays(1)),
                type: Type.Convert(),
                category: SelectedCategory?.ToDomain());
            
            switch (type)
            {
                case EventType.TransactionOperationExecuted:
                    await OnTransactionOperationExecuted(args);
                    UpdateTransactions(await _transactionRepository.FindAsync(query));
                    break;

                case EventType.TransactionItemOperationExecuted:
                    await OnTransactionItemOperationExecuted(args);
                    UpdateCategories(await _categoryRepository.FindAsync());
                    UpdateTransactions(await _transactionRepository.FindAsync(query));
                    break;

                case EventType.TransactionFilterChanged:
                case EventType.TransactionBackExecuted:
                    UpdateTransactions(await _transactionRepository.FindAsync(query));
                    break;

                case EventType.CategoryOperationExecuted:
                case EventType.CategoryItemOperationExecuted:
                    UpdateCategories(await _categoryRepository.FindAsync());
                    UpdateTransactions(await _transactionRepository.FindAsync(query));
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
        
        private ICommand _сlearStartDateCommand;
        public ICommand ClearStartDateCommand
        {
            get
            {
                return _сlearStartDateCommand ?? (_сlearStartDateCommand = new ActionCommand(vm => { StartDate = null; }));
            }
        }
        
        private ICommand _сlearEndDateCommand;
        public ICommand ClearEndDateCommand
        {
            get
            {
                return _сlearEndDateCommand ?? (_сlearEndDateCommand = new ActionCommand(vm => { EndDate = null; }));
            }
        }
    }
}
