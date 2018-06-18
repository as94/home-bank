using HomeBank.Domain.Infrastructure;
using System;

namespace HomeBank.Presentation.ViewModels
{
    public class MainViewModel
    {
        public ICategoryRepository CategoryRepository { get; }
        public ITransactionRepository TransactionRepository { get; }

        public MainViewModel(ICategoryRepository categoryRepository, ITransactionRepository transactionRepository)
        {
            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }

            if (transactionRepository == null)
            {
                throw new ArgumentNullException(nameof(transactionRepository));
            }

            CategoryRepository = categoryRepository;
            TransactionRepository = transactionRepository;
        }
    }
}
