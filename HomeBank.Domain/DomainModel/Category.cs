using HomeBank.Domain.Enums;
using System;

namespace HomeBank.Domain.DomainModel
{
    public sealed class Category : IIdentify<Guid>, IEquatable<Category>
    {
        public Guid Id { get; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public CategoryType Type { get; private set; }

        public Category(
            Guid id,
            string name,
            string description,
            CategoryType type)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            if (type == CategoryType.None)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Id = id;

            Name = name;
            Description = description;
            Type = type;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        public void ChangeDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            Description = description;
        }

        public void ChangeType(CategoryType type)
        {
            if (type == CategoryType.None)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type;
        }

        #region Equals Logic

        public override bool Equals(object obj)
        {
            if (!(obj is Category compareTo)) return false;

            return Equals(compareTo);
        }

        public bool Equals(Category other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Id == other.Id &&
                   Name == other.Name &&
                   Description == other.Description &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^
                   Name.GetHashCode() ^
                   Description.GetHashCode() ^
                   Type.GetHashCode();
        }

        public static bool operator ==(Category x, Category y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Equals(y);
        }

        public static bool operator !=(Category x, Category y)
        {
            return !(x == y);
        }

        #endregion
    }
}
