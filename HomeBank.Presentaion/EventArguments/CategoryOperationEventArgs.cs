using System;
using HomeBank.Presentation.ViewModels;

namespace HomeBank.Presentation.EventArguments
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
