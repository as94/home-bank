using System;
using System.Collections.Generic;
using System.Windows.Input;
using HomeBank.Domain.Infrastructure;
using HomeBank.Presentation.Enums;
using HomeBank.Presentation.EventArguments;
using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Presentation.ViewModels
{
    public class TransactionItemViewModel : ViewModel
    {
        private ITransactionRepository _transactionRepository;

        public override string ViewModelName => nameof(TransactionItemViewModel);

        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public CategoryItemViewModel CategoryItemViewModel { get; set; }

        public OperationType OperationType { get; set; }

        public IEnumerable<CategoryItemViewModel> Categories { get; }

        public TransactionItemViewModel(
            IEventBus eventBus,
            ITransactionRepository transactionRepository,
            IEnumerable<CategoryItemViewModel> categories)
            : base(eventBus)
        {
            if (transactionRepository == null)
            {
                throw new ArgumentNullException(nameof(transactionRepository));
            }

            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            _transactionRepository = transactionRepository;

            Categories = categories;
        }

        public TransactionItemViewModel(
            IEventBus eventBus,
            ITransactionRepository transactionRepository,
            OperationType operationType,
            IEnumerable<CategoryItemViewModel> categories) 
            : this(eventBus, transactionRepository, categories)
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

                    EventBus.Notify(EventType.TransactionItemOperationExecuted, new TransactionOperationEventArgs(this));
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
                    EventBus.Notify(EventType.TransactionBackExecuted);
                }));
            }
        }

        public Domain.DomainModels.Transaction ToDomain()
        {
            return new Domain.DomainModels.Transaction(Id, Date, Amount, CategoryItemViewModel?.ToDomain());
        }
    }
}
