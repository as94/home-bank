using HomeBank.Domain.DomainModels;
using HomeBank.Domain.Enums;
using System;

namespace HomeBank.Domain.Queries
{
    public sealed class TransactionQuery
    {
        public TransactionQuery(DateTime? date = null, CategoryType? type = null, Category category = null)
        {
            Date = date;
            Type = type;
            Category = category;
        }

        public DateTime? Date { get; }
        public CategoryType? Type { get; }
        public Category Category { get; }
    }
}
