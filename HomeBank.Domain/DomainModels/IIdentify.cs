namespace HomeBank.Domain.DomainModels
{
    public interface IIdentify<T>
    {
        T Id { get; }
    }
}
