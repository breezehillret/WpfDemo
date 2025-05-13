using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace MVVMExample
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // ICommand CanExecuteChanged event, updated to match your existing setup
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Implementation of CanExecute method
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // Implementation of Execute method
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
