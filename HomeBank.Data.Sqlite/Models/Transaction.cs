using System;

namespace HomeBank.Data.Sqlite.Models
{
    public class Transaction
    {
        public virtual string Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual decimal Amount { get; set; }

        public virtual Category Category { get; set; }
    }
}
