using FluentNHibernate.Mapping;
using HomeBank.Data.Sqlite.Models;

namespace HomeBank.Data.Sqlite.Maps
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Type);
        }
    }
}
