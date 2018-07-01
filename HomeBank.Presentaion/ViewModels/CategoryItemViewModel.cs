using HomeBank.Domain.Enums;
using HomeBank.Presentaion.Enums;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class CategoryItemViewModel : ViewModel
    {
        public override string ViewModelName => nameof(CategoryItemViewModel);

        public static IEnumerable<CategoryType> CategoryTypes => Utils.CategoryTypes.Values;

        public event EventHandler<CategoryOperationEventArgs> CategoryItemOperationExecuted;
        public void OnCategoryItemOperationExecuted(CategoryOperationEventArgs args)
        {
            CategoryItemOperationExecuted?.Invoke(this, args);
        }

        public event EventHandler BackExecuted;
        public void OnBackExecuted()
        {
            BackExecuted?.Invoke(this, new EventArgs());
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryType Type { get; set; }

        public OperationType OperationType { get; set; }

        public string Title => $"{Name} - {Description}";

        public CategoryItemViewModel()
        {
        }

        public CategoryItemViewModel(OperationType operationType)
        {
            OperationType = operationType;
        }

        private ICommand _categoryOperationCommand;
        public ICommand CategoryOperationCommand
        {
            get
            {
                return _categoryOperationCommand ?? (_categoryOperationCommand = new ActionCommand(vm =>
                {
                    if (OperationType == OperationType.Add)
                    {
                        Id = Guid.NewGuid();
                    }

                    OnCategoryItemOperationExecuted(new CategoryOperationEventArgs(this));
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

        public Domain.DomainModel.Category ToDomain()
        {
            return new Domain.DomainModel.Category(Id, Name, Description, Type);
        }
    }
}
