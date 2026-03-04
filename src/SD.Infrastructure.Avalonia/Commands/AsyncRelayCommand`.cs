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
        /// <summary>
        /// 是否可执行改变事件
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 是否执行中
        /// </summary>
        private bool _executing;

        /// <summary>
        /// 执行函数
        /// </summary>
        private readonly Func<T, Task> _execute;

        /// <summary>
        /// 是否可执行函数
        /// </summary>
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// 创建异步转接命令构造器
        /// </summary>
        /// <param name="execute">执行函数</param>
        /// <param name="canExecute">是否可执行函数</param>
        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>是否可执行</returns>
        public bool CanExecute(object parameter)
        {
            if (this._executing)
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

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="parameter">参数</param>
        public async void Execute(object parameter)
        {
            if (!this.CanExecute(parameter))
            {
                return;
            }

            try
            {
                this._executing = true;
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
                this._executing = false;
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 触发是否可执行改变事件
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
