namespace HomeBank.Presentation.Infrastructure
{
    public interface IDialogServiceFactory
    {
        IDialogService Create(string text = null);
    }
}
