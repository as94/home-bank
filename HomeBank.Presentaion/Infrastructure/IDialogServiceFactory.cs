namespace HomeBank.Presentaion.Infrastructure
{
    public interface IDialogServiceFactory
    {
        IDialogService Create(string text = null);
    }
}
