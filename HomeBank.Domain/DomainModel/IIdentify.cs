namespace HomeBank.Domain.DomainModel
{
    public interface IIdentify<T>
    {
        T Id { get; }
    }
}
