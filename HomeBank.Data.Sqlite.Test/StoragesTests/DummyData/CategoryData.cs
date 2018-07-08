using System;

namespace HomeBank.Data.Sqlite.Test.StoragesTests.DummyData
{
    internal static class CategoryData
    {
        public static Domain.DomainModels.Category CreateCategory(
            Guid id,
            string name = null,
            string description = null,
            Domain.Enums.CategoryType type = Domain.Enums.CategoryType.None)
        {
            return new Domain.DomainModels.Category(
                id,
                name ?? "Product",
                description ?? "Apple",
                type != Domain.Enums.CategoryType.None 
                    ? type
                    : Domain.Enums.CategoryType.Expenditure);
        }
    }
}
