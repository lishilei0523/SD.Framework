using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.Commands
{
    /// <summary>
    /// 转接命令
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region # 字段与构造器

        /// <summary>
        /// 命令委托
        /// </summary>
        private readonly Action<object> _command;

        /// <summary>
        /// 是否可执行委托
        /// </summary>
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// 创建转接命令构造器
        /// </summary>
        /// <param name="command">命令委托</param>
        public RelayCommand(Action<object> command)
            : this(command, null)
        {

        }

        /// <summary>
        /// 创建转接命令构造器
        /// </summary>
        /// <param name="command">命令委托</param>
        /// <param name="canExecute">是否可执行委托</param>
        public RelayCommand(Action<object> command, Predicate<object> canExecute)
        {
            this._command = command ?? throw new ArgumentNullException(nameof(command));
            this._canExecute = canExecute;
        }

        #endregion

        #region # 事件

        #region 是否可执行变更事件 —— event EventHandler CanExecuteChanged
        /// <summary>
        /// 是否可执行变更事件
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #endregion

        #region # 方法

        #region 是否可执行 —— bool CanExecute(object parameter)
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>是否可执行</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }
        #endregion

        #region 执行 —— void Execute(object parameter)
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="parameter">参数</param>
        public void Execute(object parameter)
        {
            this._command.Invoke(parameter);
        }
        #endregion

        #endregion
    }
}
