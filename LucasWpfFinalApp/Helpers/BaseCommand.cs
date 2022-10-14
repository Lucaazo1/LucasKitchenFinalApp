using LucasWpfFinalApp.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LucasWpfFinalApp.Helpers
{
    internal abstract class BaseCommand : ICommand
    {
        //public abstract void Execute(object? parameter);
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        //ny kod
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public BaseCommand(Action<object> execute, Func<object, bool> canExecute = null!)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //gamla kod
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //public virtual bool CanExecute(object? parameter) => true;
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
    }
}
