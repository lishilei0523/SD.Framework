using System;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.Toolkits
{
    public class DelegateCommand<T> : ICommand
    {
        private static bool CanExecute(T parameter)
        {
            return true;
        }

        private readonly Action<T> _execute;

        private Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute is null.");
            }

            this._execute = execute;
            this._canExecute = canExecute ?? CanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute(this.TranslateParameter(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void Execute(object parameter)
        {
            this._execute(this.TranslateParameter(parameter));
        }

        private T TranslateParameter(object parameter)
        {
            T value = default(T);
            if (parameter != null && typeof(T).IsEnum)
                value = (T)Enum.Parse(typeof(T), (string)parameter);
            else
                value = (T)parameter;
            return value;
        }
    }

    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action execute,
            Func<bool> canExecute = null)
            : base(obj => execute(),
                 (canExecute == null ? null : new Func<object, bool>(obj => canExecute())))
        {

        }
    }
}
