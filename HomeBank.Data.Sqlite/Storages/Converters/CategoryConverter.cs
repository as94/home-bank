using System;

namespace HomeBank.Data.Sqlite.Storages.Converters
{
    internal static class CategoryConverter
    {
        public static Models.Category Convert(Domain.DomainModels.Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return new Models.Category
            {
                Id = category.Id.ToString(),
                Name = category.Name,
                Description = category.Description,
                Type = (int)category.Type
            };
        }

        public static Domain.DomainModels.Category Convert(Models.Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return new Domain.DomainModels.Category(
                Guid.Parse(category.Id),
                category.Name,
                category.Description,
                (Domain.Enums.CategoryType)category.Type);
        }
    }
}
