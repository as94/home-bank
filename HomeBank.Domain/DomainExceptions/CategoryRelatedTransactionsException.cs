using HomeBank.Domain.DomainModels;
using System;

namespace HomeBank.Domain.DomainExceptions
{
    public class CategoryRelatedTransactionsException : Exception
    {
        public CategoryRelatedTransactionsException(Category category)
            : base($"Category '{category?.Name}' have related transactions.")
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
        }
    }
}
