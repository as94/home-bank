using HomeBank.Presentaion.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeBank.Presentaion.ViewModels
{
    public class CategoryItemViewModel
    {
        public event EventHandler CategoryItemOperationExecuted;
        public void OnCategoryItemOperationExecuted()
        {
            CategoryItemOperationExecuted.Invoke(this, new EventArgs());
        }

        private ICommand _categoryOperationCommand;
        public ICommand CategoryOperationCommand
        {
            get
            {
                return _categoryOperationCommand ?? (_categoryOperationCommand = new ActionCommand(vm =>
                {
                    OnCategoryItemOperationExecuted();
                }));
            }
        }
    }
}
