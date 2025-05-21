using Caliburn.Micro;
using SD.Infrastructure.Avalonia.Caliburn.Aspects;
using SD.Infrastructure.Avalonia.Interfaces;
using System;
using System.Threading.Tasks;

namespace SD.Infrastructure.Avalonia.Caliburn.Base
{
    /// <summary>
    /// Screen基类
    /// </summary>
    public abstract class ScreenBase : Screen, IBusy
    {
        #region # 字段及构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected ScreenBase()
        {

        }

        #endregion

        #region # 属性

        #region 是否繁忙 —— bool IsBusy
        /// <summary>
        /// 是否繁忙
        /// </summary>
        [DependencyProperty]
        public bool IsBusy { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 挂起繁忙状态 —— void Busy()
        /// <summary>
        /// 挂起繁忙状态
        /// </summary>
        public void Busy()
        {
            this.IsBusy = true;
        }
        #endregion

        #region 释放繁忙状态 —— void Idle()
        /// <summary>
        /// 释放繁忙状态
        /// </summary>
        public void Idle()
        {
            this.IsBusy = false;
        }
        #endregion

        #region 在UI线程执行 —— void OnUIThread(System.Action action)
        /// <summary>
        /// 在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public new void OnUIThread(System.Action action)
        {
            action.OnUIThread();
        }
        #endregion

        #region 异步在UI线程执行 —— void BeginOnUIThread(Action action)
        /// <summary>
        /// 异步在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public void BeginOnUIThread(System.Action action)
        {
            action.BeginOnUIThread();
        }
        #endregion

        #region 异步等待在UI线程执行 —— Task OnUIThreadAsync(Func<Task> action)
#if NET461 || NET462 || NETCOREAPP3_1_OR_GREATER
        /// <summary>
        /// 异步等待在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public Task OnUIThreadAsync(Func<Task> action)
        {
            return action.OnUIThreadAsync();
        }
#endif
        #endregion

        #endregion
    }
}
