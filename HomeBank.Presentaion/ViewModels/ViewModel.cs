using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HomeBank.Presentation.Infrastructure;

namespace HomeBank.Presentation.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract string ViewModelName { get; }

        public IEventBus EventBus { get; }

        protected ViewModel(IEventBus eventBus)
        {
            if (eventBus == null)
            {
                throw new ArgumentNullException(nameof(eventBus));
            }

            EventBus = eventBus;
        }
    }
}
