using FluentNHibernate.Mapping;

namespace HomeBank.Data.Sqlite.Maps
{
    public class TransactionMap : ClassMap<Models.Transaction>
    {
        public TransactionMap()
        {
            Id(x => x.Id);
            Map(x => x.Date);
            Map(x => x.Amount);

            References(x => x.Category);
        }
    }
}
