using HomeBank.Domain.DomainModel;
using NUnit.Framework;
using System;

namespace HomeBank.Domain.Test.DomainModelTest
{
    [TestFixture]
    internal class CategoryTest
    {
        [Test]
        public void CategoryCtorTest()
        {
            var id = Guid.NewGuid();
            var name = "Product";
            var description = "Orange";
            var type = Enums.CategoryType.Expenditure;

            var category = new Category(id, name, description, type);

            Assert.That(category.Id, Is.EqualTo(id));
            Assert.That(category.Name, Is.EqualTo(name));
            Assert.That(category.Description, Is.EqualTo(description));
            Assert.That(category.Type, Is.EqualTo(type));
        }
    }
}
