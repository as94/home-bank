namespace HomeBank.Presentation.Infrastructure
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
        TransactionFilterChanged,

        CategoryStatisticFilterChanged
    }
}
