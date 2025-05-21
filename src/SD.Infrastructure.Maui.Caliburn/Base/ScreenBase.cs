using Acr.UserDialogs;
using Caliburn.Micro;
using CommunityToolkit.Maui.Views;
using SD.Infrastructure.Maui.Caliburn.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SD.Infrastructure.Maui.Caliburn.Base
{
    /// <summary>
    /// Screen基类
    /// </summary>
    public class ScreenBase : Screen
    {
        #region # 挂起繁忙状态 —— void Busy(string title...
        /// <summary>
        /// 挂起繁忙状态
        /// </summary>
        protected void Busy(string title = "加载中...", MaskType? maskType = null)
        {
            MessageBox.Busy(title, maskType);
        }
        #endregion

        #region # 释放繁忙状态 —— void Idle()
        /// <summary>
        /// 释放繁忙状态
        /// </summary>
        protected void Idle()
        {
            MessageBox.Idle();
        }
        #endregion

        #region # 提示消息 —— void Toast(string title...
        /// <summary>
        /// 提示消息
        /// </summary>
        protected void Toast(string title, TimeSpan? dismissTimer = null)
        {
            MessageBox.Toast(title, dismissTimer);
        }
        #endregion

        #region # 提示消息 —— void ToastVibrantly(string title...
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <remarks>带振动</remarks>
        protected void ToastVibrantly(string title, TimeSpan? dismissTimer = null, int vibratedDuration = 500)
        {
            MessageBox.ToastVibrantly(title, dismissTimer, vibratedDuration);
        }
        #endregion

        #region # 警告消息 —— async Task Alert(string message...
        /// <summary>
        /// 警告消息
        /// </summary>
        protected async Task Alert(string message, string title = null, CancellationToken? cancelToken = default)
        {
            await MessageBox.Alert(message, title, cancelToken);
        }
        #endregion

        #region # 警告消息 —— async Task AlertVibrantly(string message...
        /// <summary>
        /// 警告消息
        /// </summary>
        /// <remarks>带振动</remarks>
        protected async Task AlertVibrantly(string message, string title = null, int vibratedDuration = 500, CancellationToken? cancelToken = default)
        {
            await MessageBox.AlertVibrantly(message, title, vibratedDuration, cancelToken);
        }
        #endregion

        #region # 确认消息 —— async Task<bool> Confirm(string message...
        /// <summary>
        /// 确认消息
        /// </summary>
        protected async Task<bool> Confirm(string message, string title = null, CancellationToken? cancelToken = default)
        {
            return await MessageBox.Confirm(message, title, cancelToken);
        }
        #endregion

        #region # 确认消息 —— async Task<bool> ConfirmVibrantly(string message...
        /// <summary>
        /// 确认消息
        /// </summary>
        protected async Task<bool> ConfirmVibrantly(string message, string title = null, int vibratedDuration = 500, CancellationToken? cancelToken = default)
        {
            return await MessageBox.ConfirmVibrantly(message, title, vibratedDuration, cancelToken);
        }
        #endregion

        #region # 关闭视图模型 —— override async Task TryCloseAsync(bool? dialogResult = null)
        /// <summary>
        /// 关闭视图模型
        /// </summary>
        public override async Task TryCloseAsync(bool? dialogResult = null)
        {
            object view = this.GetView();
            if (view is Popup popup)
            {
                await popup.CloseAsync(dialogResult);
            }

            await base.TryCloseAsync(dialogResult);
        }
        #endregion
    }
}
