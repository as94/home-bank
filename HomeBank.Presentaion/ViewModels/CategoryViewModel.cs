using HomeBank.Domain.DomainModel;
using HomeBank.Domain.Enums;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class CategoryViewModel : ViewModel
    {
        public override string ViewModelName => nameof(CategoryViewModel);

        public static IEnumerable<CategoryType> CategoryTypes => CategoryItemViewModel.CategoryTypes;

        public event EventHandler<CategoryOperationEventArgs> CategoryOperationExecuted;
        public void OnCategoryOperationExecuted(CategoryOperationEventArgs args)
        {
            CategoryOperationExecuted?.Invoke(this, args);
        }

        public CategoryType Type { get; set; }

        public ObservableCollection<CategoryItemViewModel> Categories { get; set; }
        public CategoryItemViewModel SelectedCategory { get; set; }

        public CategoryViewModel(IEnumerable<Category> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            Categories = new ObservableCollection<CategoryItemViewModel>();

            UpdateCategories(categories);
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

            SelectedCategory = Categories.Count > 0 ? Categories[0] : null;
        }

        private ICommand _addCategoryCommand;
        public ICommand AddCategoryCommand
        {
            get
            {
                return _addCategoryCommand ?? (_addCategoryCommand = new ActionCommand(vm =>
                {
                    OnCategoryOperationExecuted(new CategoryOperationEventArgs(new CategoryItemViewModel(Enums.OperationType.Add)));
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
                    SelectedCategory.OperationType = Enums.OperationType.Edit;
                    OnCategoryOperationExecuted(new CategoryOperationEventArgs(SelectedCategory));
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
                    if (SelectedCategory != null)
                    {
                        SelectedCategory.OperationType = Enums.OperationType.Remove;
                        OnCategoryOperationExecuted(new CategoryOperationEventArgs(SelectedCategory));
                    }
                }));
            }
        }
    }
}
