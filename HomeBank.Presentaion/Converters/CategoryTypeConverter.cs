﻿using HomeBank.Domain.Enums;
using HomeBank.Presentaion.Enums;

namespace HomeBank.Presentaion.Converters
{
    public static class CategoryTypeConverter
    {
        public static CategoryTypeFilter Convert(this CategoryType type)
        {
            switch (type)
            {
                case CategoryType.None:
                    return CategoryTypeFilter.None;

                case CategoryType.Income:
                    return CategoryTypeFilter.Income;

                case CategoryType.Expenditure:
                    return CategoryTypeFilter.Expenditure;
            }

            return CategoryTypeFilter.All;
        }

        public static CategoryType? Convert(this CategoryTypeFilter filter)
        {
            switch (filter)
            {
                case CategoryTypeFilter.None:
                    return CategoryType.None;

                case CategoryTypeFilter.Income:
                    return CategoryType.Income;

                case CategoryTypeFilter.Expenditure:
                    return CategoryType.Expenditure;
            }

            return null;
        }
    }
}