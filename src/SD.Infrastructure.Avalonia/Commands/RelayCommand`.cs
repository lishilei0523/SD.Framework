using System;
using System.Windows.Input;

namespace SD.Infrastructure.Avalonia.Commands
{
    /// <summary>
    /// 转接命令
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// 是否可执行改变事件
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 执行函数
        /// </summary>
        private readonly Action<T> _execute;

        /// <summary>
        /// 是否可执行函数
        /// </summary>
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// 创建转接命令构造器
        /// </summary>
        /// <param name="execute">执行函数</param>
        /// <param name="canExecute">是否可执行函数</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
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
            if (this._canExecute == null)
            {
                return true;
            }
            if (parameter == null && typeof(T).IsValueType)
            {
                return this._canExecute(default(T));
            }
            if (parameter is T arg)
            {
                return this._canExecute(arg);
            }

            return false;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="parameter">参数</param>
        public void Execute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
            {
                this._execute(default(T));
            }
            else if (parameter is T arg)
            {
                this._execute(arg);
            }
            else if (parameter != null)
            {
                this._execute((T)Convert.ChangeType(parameter, typeof(T)));
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
