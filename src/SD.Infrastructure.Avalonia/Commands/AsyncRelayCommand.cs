using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SD.Infrastructure.Avalonia.Commands
{
    /// <summary>
    /// 异步转接命令
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return !this._isExecuting && (this._canExecute == null || this._canExecute(parameter));
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
                await this._execute(parameter);
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
