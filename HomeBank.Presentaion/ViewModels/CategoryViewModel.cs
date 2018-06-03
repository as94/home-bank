using HomeBank.Presentaion.Infrastructure;
using System;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class CategoryViewModel
    {
        public event EventHandler CategoryOperationExecuted;
        public void OnCategoryOperationExecuted()
        {
            CategoryOperationExecuted.Invoke(this, new EventArgs());
        }

        public CategoryViewModel()
        {
        }

        private ICommand _addCategoryCommand;
        public ICommand AddCategoryCommand
        {
            get
            {
                return _addCategoryCommand ?? (_addCategoryCommand = new ActionCommand(vm =>
                {
                    OnCategoryOperationExecuted();
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
                    OnCategoryOperationExecuted();
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
                    OnCategoryOperationExecuted();
                }));
            }
        }
    }
}
