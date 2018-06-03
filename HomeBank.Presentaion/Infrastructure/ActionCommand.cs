using System;
using System.Windows.Input;

namespace HomeBank.Presentaion.Infrastructure
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ActionCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

            if (canExecute != null)
            {
                _canExecute = canExecute;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute(parameter) : true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
