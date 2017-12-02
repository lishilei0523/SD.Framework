using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.Toolkits
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
        private readonly Action<object> _action;

        /// <summary>
        /// 是否可执行委托
        /// </summary>
        private readonly Predicate<object> _canExecute;

        #region 01.单参数构造器
        /// <summary>
        /// 单参数构造器
        /// </summary>
        /// <param name="command">命令委托</param>
        public RelayCommand(Action<object> command)
            : this(command, null)
        {

        }
        #endregion

        #region 02.双参数构造器
        /// <summary>
        /// 双参数构造器
        /// </summary>
        /// <param name="action">命令委托</param>
        /// <param name="canExecute">是否可执行委托</param>
        public RelayCommand(Action<object> action, Predicate<object> canExecute)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this._action = action;
            this._canExecute = canExecute;
        }
        #endregion

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
            this._action.Invoke(parameter);
        }
        #endregion

        #endregion
    }
}
