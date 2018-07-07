namespace HomeBank.Presentaion.Infrastructure
{
    public enum EventType
    {
        Unknown,

        CategoryItemOperationExecuted,
        CategoryOperationExecuted,

        TransactionItemOperationExecuted,
        TransactionOperationExecuted,

        CategoryBackExecuted,
        TransactionBackExecuted,

        CategoryFilterChanged,
        TransactionFilterChanged
    }
}
