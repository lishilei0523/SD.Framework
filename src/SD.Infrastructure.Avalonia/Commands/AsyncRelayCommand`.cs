using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SD.Infrastructure.Avalonia.Commands
{
    /// <summary>
    /// 异步转接命令
    /// </summary>
    public class AsyncRelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this._isExecuting)
            {
                return false;
            }

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

        public async void Execute(object parameter)
        {
            if (!this.CanExecute(parameter))
            {
                return;
            }

            try
            {
                this._isExecuting = true;
                this.RaiseCanExecuteChanged();

                if (parameter == null && typeof(T).IsValueType)
                {
                    await this._execute(default(T));
                }
                else if (parameter is T t)
                {
                    await this._execute(t);
                }
                else if (parameter != null)
                {
                    await this._execute((T)Convert.ChangeType(parameter, typeof(T)));
                }
            }
            finally
            {
                this._isExecuting = false;
                this.RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
