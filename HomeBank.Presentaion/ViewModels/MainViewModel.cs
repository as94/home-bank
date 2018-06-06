using HomeBank.Domain.Infrastructure;
using System;

namespace HomeBank.Presentation.ViewModels
{
    public class MainViewModel
    {
        public ICategoryRepository CategoryRepository { get; }

        public MainViewModel(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }

            CategoryRepository = categoryRepository;
        }
    }
}
