using System;
using System.Windows.Input;

namespace SD.Infrastructure.Avalonia.Commands
{
    /// <summary>
    /// 转接命令
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }
            if (parameter == null && typeof(T).IsValueType)
            {
                return this._canExecute(default(T));
            }
            if (parameter is T t)
            {
                return this._canExecute(t);
            }

            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
            {
                this._execute(default(T));
            }
            else if (parameter is T t)
            {
                this._execute(t);
            }
            else if (parameter != null)
            {
                this._execute((T)Convert.ChangeType(parameter, typeof(T)));
            }
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
