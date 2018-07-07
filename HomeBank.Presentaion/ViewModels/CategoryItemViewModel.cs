using HomeBank.Domain.Enums;
using HomeBank.Domain.Infrastructure;
using HomeBank.Presentaion.Enums;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class CategoryItemViewModel : ViewModel
    {
        public override string ViewModelName => nameof(CategoryItemViewModel);

        public static IEnumerable<CategoryType> CategoryTypes => Utils.CategoryTypes.Values;

        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryType Type { get; set; }

        public OperationType OperationType { get; set; }

        public string Title => $"{Name} - {Description}";

        public CategoryItemViewModel(IEventBus eventBus)
            : base(eventBus)
        {
        }

        public CategoryItemViewModel(IEventBus eventBus, OperationType operationType)
            : this(eventBus)
        {
            OperationType = operationType;

            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
        }

        private ICommand _categoryOperationCommand;
        public ICommand CategoryOperationCommand
        {
            get
            {
                return _categoryOperationCommand ?? (_categoryOperationCommand = new ActionCommand(vm =>
                {
                    EventBus.Notify(EventType.CategoryItemOperationExecuted, new CategoryOperationEventArgs(this));
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
                    EventBus.Notify(EventType.CategoryBackExecuted);
                }));
            }
        }

        public Domain.DomainModels.Category ToDomain()
        {
            return new Domain.DomainModels.Category(Id, Name, Description, Type);
        }
    }
}
