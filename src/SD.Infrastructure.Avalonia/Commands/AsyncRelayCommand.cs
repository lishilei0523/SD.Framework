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
        private readonly Func<object, Task> _execute;

        /// <summary>
        /// 是否可执行函数
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// 创建异步转接命令构造器
        /// </summary>
        /// <param name="execute">执行函数</param>
        /// <param name="canExecute">是否可执行函数</param>
        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            #region # 验证

            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute), "执行函数不可为空！");
            }

            #endregion

            this._execute = execute;
            this._canExecute = canExecute;
        }

        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>是否可执行</returns>
        public bool CanExecute(object parameter)
        {
            return !this._executing && (this._canExecute == null || this._canExecute(parameter));
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
                await this._execute(parameter);
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
