using Acr.UserDialogs;
using Caliburn.Micro;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SD.Infrastructure.Xamarin.Caliburn.Base
{
    /// <summary>
    /// 单活动Conductor基类
    /// </summary>
    public class OneActiveConductorBase : Conductor<IScreen>.Collection.OneActive
    {
        #region # 挂起繁忙状态 —— void Busy(string title...
        /// <summary>
        /// 挂起繁忙状态
        /// </summary>
        protected void Busy(string title = "加载中...", MaskType? maskType = null)
        {
            UserDialogs.Instance.ShowLoading(title, maskType);
        }
        #endregion

        #region # 释放繁忙状态 —— void Idle()
        /// <summary>
        /// 释放繁忙状态
        /// </summary>
        protected void Idle()
        {
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region # 提示消息 —— void Toast(string title...
        /// <summary>
        /// 提示消息
        /// </summary>
        protected void Toast(string title, TimeSpan? dismissTimer = null)
        {
            UserDialogs.Instance.Toast(title, dismissTimer);
        }
        #endregion

        #region # 提示消息 —— void ToastVibrantly(string title...
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <remarks>带振动</remarks>
        protected void ToastVibrantly(string title, TimeSpan? dismissTimer = null, int vibratedDuration = 500)
        {
            Vibration.Vibrate(vibratedDuration);
            UserDialogs.Instance.Toast(title, dismissTimer);
        }
        #endregion

        #region # 警告消息 —— async Task Alert(string message...
        /// <summary>
        /// 警告消息
        /// </summary>
        protected async Task Alert(string message, string title = null, CancellationToken? cancelToken = null)
        {
            await UserDialogs.Instance.AlertAsync(message, title, "确定", cancelToken);
        }
        #endregion

        #region # 警告消息 —— async Task AlertVibrantly(string message...
        /// <summary>
        /// 警告消息
        /// </summary>
        /// <remarks>带振动</remarks>
        protected async Task AlertVibrantly(string message, string title = null, int vibratedDuration = 500, CancellationToken? cancelToken = null)
        {
            Vibration.Vibrate(vibratedDuration);
            await UserDialogs.Instance.AlertAsync(message, title, "确定", cancelToken);
        }
        #endregion

        #region # 确认消息 —— async Task<bool> Confirm(string message...
        /// <summary>
        /// 确认消息
        /// </summary>
        protected async Task<bool> Confirm(string message, string title = null, CancellationToken? cancelToken = null)
        {
            return await UserDialogs.Instance.ConfirmAsync(message, title, "确定", "取消", cancelToken);
        }
        #endregion

        #region # 确认消息 —— async Task<bool> ConfirmVibrantly(string message...
        /// <summary>
        /// 确认消息
        /// </summary>
        protected async Task<bool> ConfirmVibrantly(string message, string title = null, int vibratedDuration = 500, CancellationToken? cancelToken = null)
        {
            Vibration.Vibrate(vibratedDuration);
            return await UserDialogs.Instance.ConfirmAsync(message, title, "确定", "取消", cancelToken);
        }
        #endregion
    }
}
