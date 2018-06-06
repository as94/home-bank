using HomeBank.Presentaion.ViewModels;
using System;

namespace HomeBank.Presentaion.EventArguments
{
    public sealed class CategoryOperationEventArgs : EventArgs
    {
        public CategoryOperationEventArgs(CategoryItemViewModel category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            
            Category = category;
        }
        
        public CategoryItemViewModel Category { get; }
    }
}
