namespace HomeBank.Data.Sqlite.Models
{
    public class Category
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Type { get; set; }
    }
}
