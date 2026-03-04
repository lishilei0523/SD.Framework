using System;
using System.Windows.Input;

namespace SD.Infrastructure.Avalonia.Commands
{
    /// <summary>
    /// 转接命令
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// 是否可执行改变事件
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 执行函数
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// 是否可执行函数
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// 创建转接命令构造器
        /// </summary>
        /// <param name="execute">执行函数</param>
        /// <param name="canExecute">是否可执行函数</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
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

            return this._canExecute(parameter);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="parameter">参数</param>
        public void Execute(object parameter)
        {
            this._execute(parameter);
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
