using System;

namespace HomeBank.Domain.Test.DummyData
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
                type);
        }
    }
}
