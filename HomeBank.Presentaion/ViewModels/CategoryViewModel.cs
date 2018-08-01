using HomeBank.Domain.DomainExceptions;
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
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class CategoryViewModel : ViewModel
    {
        private readonly IDialogServiceFactory _yesNoDialogServiceFactory;
        private readonly IDialogServiceFactory _errorDialogServiceFactory;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICategoryRepository _categoryRepository;

        public override string ViewModelName => nameof(CategoryViewModel);

        public static IEnumerable<CategoryTypeFilter> CategoryTypes => Utils.CategoryTypes.Filters;

        private CategoryTypeFilter _type = CategoryTypeFilter.All;
        public CategoryTypeFilter Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                OnPropertyChanged();

                EventBus.Notify(EventType.CategoryFilterChanged);
            }
        }

        public ObservableCollection<CategoryItemViewModel> Categories { get; set; }
        public CategoryItemViewModel SelectedCategory { get; set; }
        
        public static async Task<CategoryViewModel> CreateAsync(
            IEventBus eventBus,
            IDialogServiceFactory yesNoDialogServiceFactory,
            IDialogServiceFactory errorDialogServiceFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICategoryRepository categoryRepository)
        {
            if (yesNoDialogServiceFactory == null)
            {
                throw new ArgumentNullException(nameof(yesNoDialogServiceFactory));
            }

            if (errorDialogServiceFactory == null)
            {
                throw new ArgumentNullException(nameof(errorDialogServiceFactory));
            }

            if (unitOfWorkFactory == null)
            {
                throw new ArgumentNullException(nameof(unitOfWorkFactory));
            }

            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }

            var categories = await categoryRepository.FindAsync();

            return new CategoryViewModel(
                eventBus,
                yesNoDialogServiceFactory,
                errorDialogServiceFactory,
                unitOfWorkFactory,
                categoryRepository,
                categories);
        }

        private CategoryViewModel(
            IEventBus eventBus,
            IDialogServiceFactory yesNoDialogServiceFactory,
            IDialogServiceFactory errorDialogServiceFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICategoryRepository categoryRepository,
            IEnumerable<Category> categories)
            : base(eventBus)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            _yesNoDialogServiceFactory = yesNoDialogServiceFactory;
            _errorDialogServiceFactory = errorDialogServiceFactory;

            _unitOfWorkFactory = unitOfWorkFactory;
            _categoryRepository = categoryRepository;

            Categories = new ObservableCollection<CategoryItemViewModel>();

            UpdateCategories(categories);
            
            EventBus.EventOccured += EventBus_EventOccured;
        }

        private async void EventBus_EventOccured(EventType type, EventArgs args = null)
        {
            switch (type)
            {
                case EventType.CategoryOperationExecuted:
                    await OnCategoryOperationExecutedAsync(args);
                    UpdateCategories(await _categoryRepository.FindAsync(new CategoryQuery(Type.Convert())));
                    break;

                case EventType.CategoryItemOperationExecuted:
                    await OnCategoryItemOperationExecuted(args);
                    UpdateCategories(await _categoryRepository.FindAsync(new CategoryQuery(Type.Convert())));
                    break;

                case EventType.CategoryFilterChanged:
                case EventType.CategoryBackExecuted:
                    UpdateCategories(await _categoryRepository.FindAsync(new CategoryQuery(Type.Convert())));
                    break;
            }
        }

        private async Task OnCategoryOperationExecutedAsync(EventArgs args)
        {
            if (args is CategoryOperationEventArgs categoryOperationArgs && categoryOperationArgs.Category.OperationType == OperationType.Remove)
            {
                using (var unitOfWork = _unitOfWorkFactory.Create())
                {
                    try
                    {
                        await _categoryRepository.RemoveAsync(categoryOperationArgs.Category.Id);
                        await unitOfWork.CommitAsync();
                    }
                    catch (CategoryRelatedTransactionsException ex)
                    {
                        var text = ex.Message;
                        _errorDialogServiceFactory.Create(text);
                    }
                }
            }
        }

        private async Task OnCategoryItemOperationExecuted(EventArgs args)
        {
            if (args is CategoryOperationEventArgs categoryOperationArgs)
            {
                var operationType = categoryOperationArgs.Category.OperationType;
                var category = categoryOperationArgs.Category.ToDomain();
                switch (operationType)
                {
                    case OperationType.Add:
                        using (var unitOfWork = _unitOfWorkFactory.Create())
                        {
                            await _categoryRepository.CreateAsync(category);
                            await unitOfWork.CommitAsync();
                        }
                        break;

                    case OperationType.Edit:
                        using (var unitOfWork = _unitOfWorkFactory.Create())
                        {
                            await _categoryRepository.ChangeAsync(category);
                            await unitOfWork.CommitAsync();
                        }
                        break;
                }
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

            SelectedCategory = Categories.Count > 0 ? Categories[0] : null;
        }

        private ICommand _addCategoryCommand;
        public ICommand AddCategoryCommand
        {
            get
            {
                return _addCategoryCommand ?? (_addCategoryCommand = new ActionCommand(vm =>
                {
                    var categoryItemViewModel = new CategoryItemViewModel(EventBus, OperationType.Add);
                    var args = new CategoryOperationEventArgs(categoryItemViewModel);
                    EventBus.Notify(EventType.CategoryOperationExecuted, args);
                }));
            }
        }

        private ICommand _editCategoryCommand;
        public ICommand EditCategoryCommand
        {
            get
            {
                return _editCategoryCommand ?? (_editCategoryCommand = new ActionCommand(vm =>
                {
                    if (SelectedCategory == null)
                    {
                        return;
                    }

                    SelectedCategory.OperationType = OperationType.Edit;
                    var args = new CategoryOperationEventArgs(SelectedCategory);
                    EventBus.Notify(EventType.CategoryOperationExecuted, args);
                }));
            }
        }

        private ICommand _removeCategoryCommand;
        public ICommand RemoveCategoryCommand
        {
            get
            {
                return _removeCategoryCommand ?? (_removeCategoryCommand = new ActionCommand(vm =>
                {
                    if (SelectedCategory == null)
                    {
                        return;
                    }
                    
                    if (_yesNoDialogServiceFactory.Create().ShowDialog)
                    {
                        SelectedCategory.OperationType = OperationType.Remove;
                        var args = new CategoryOperationEventArgs(SelectedCategory);
                        EventBus.Notify(EventType.CategoryOperationExecuted, args);
                    }
                }));
            }
        }
    }
}
