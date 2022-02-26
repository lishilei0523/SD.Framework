using Acr.UserDialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SD.Infrastructure.Xamarin.Caliburn.Extensions
{
    /// <summary>
    /// 消息框扩展
    /// </summary>
    public static class MessageBox
    {
        #region # 挂起繁忙状态 —— static void Busy(string title...
        /// <summary>
        /// 挂起繁忙状态
        /// </summary>
        public static void Busy(string title = "加载中...", MaskType? maskType = null)
        {
            UserDialogs.Instance.ShowLoading(title, maskType);
        }
        #endregion

        #region # 释放繁忙状态 —— static void Idle()
        /// <summary>
        /// 释放繁忙状态
        /// </summary>
        public static void Idle()
        {
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region # 提示消息 —— static void Toast(string title...
        /// <summary>
        /// 提示消息
        /// </summary>
        public static void Toast(string title, TimeSpan? dismissTimer = null)
        {
            UserDialogs.Instance.Toast(title, dismissTimer);
        }
        #endregion

        #region # 提示消息 —— static void ToastVibrantly(string title...
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <remarks>带振动</remarks>
        public static void ToastVibrantly(string title, TimeSpan? dismissTimer = null, int vibratedDuration = 500)
        {
            Vibration.Vibrate(vibratedDuration);
            UserDialogs.Instance.Toast(title, dismissTimer);
        }
        #endregion

        #region # 警告消息 —— static async Task Alert(string message...
        /// <summary>
        /// 警告消息
        /// </summary>
        public static async Task Alert(string message, string title = null, CancellationToken? cancelToken = null)
        {
            await UserDialogs.Instance.AlertAsync(message, title, "确定", cancelToken);
        }
        #endregion

        #region # 警告消息 —— static async Task AlertVibrantly(string message...
        /// <summary>
        /// 警告消息
        /// </summary>
        /// <remarks>带振动</remarks>
        public static async Task AlertVibrantly(string message, string title = null, int vibratedDuration = 500, CancellationToken? cancelToken = null)
        {
            Vibration.Vibrate(vibratedDuration);
            await UserDialogs.Instance.AlertAsync(message, title, "确定", cancelToken);
        }
        #endregion

        #region # 确认消息 —— static async Task<bool> Confirm(string message...
        /// <summary>
        /// 确认消息
        /// </summary>
        public static async Task<bool> Confirm(string message, string title = null, CancellationToken? cancelToken = null)
        {
            return await UserDialogs.Instance.ConfirmAsync(message, title, "确定", "取消", cancelToken);
        }
        #endregion

        #region # 确认消息 —— static async Task<bool> ConfirmVibrantly(string message...
        /// <summary>
        /// 确认消息
        /// </summary>
        public static async Task<bool> ConfirmVibrantly(string message, string title = null, int vibratedDuration = 500, CancellationToken? cancelToken = null)
        {
            Vibration.Vibrate(vibratedDuration);
            return await UserDialogs.Instance.ConfirmAsync(message, title, "确定", "取消", cancelToken);
        }
        #endregion
    }
}
