using Caliburn.Micro;
using Microsoft.Win32;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Caliburn.Models;
using SD.Infrastructure.WPF.Constants;
using SD.Infrastructure.WPF.Interfaces;
using SD.Infrastructure.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Lifetime.Clear;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SD.Infrastructure.WPF.Caliburn.Base
{
    /// <summary>
    /// 单活动Conductor基类
    /// </summary>
    public abstract class OneActiveConductorBase : Conductor<IScreen>.Collection.OneActive, IBusy
    {
        #region # 字段及构造器

        /// <summary>
        /// 通知
        /// </summary>
        private readonly Notifier _notifier;

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected OneActiveConductorBase()
        {
            //初始化通知
            this._notifier = new Notifier(options =>
            {
                options.PositionProvider = new WindowPositionProvider(Application.Current.MainWindow, this.NotifierOptions.Location, this.NotifierOptions.MarginX, this.NotifierOptions.MarginY);
                options.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(this.NotifierOptions.Lifetime), MaximumNotificationCount.FromCount(this.NotifierOptions.MaxCount));
                options.DisplayOptions.TopMost = false;
                options.Dispatcher = Dispatcher.CurrentDispatcher;
            });
            this._notifier.ClearMessages(new ClearAll());

            //默认值
            this.LiveMessages = new ObservableCollection<LiveMessage>();
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

        #region 实时消息列表 —— ObservableCollection<LiveMessage> LiveMessages
        /// <summary>
        /// 实时消息列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<LiveMessage> LiveMessages { get; set; }
        #endregion

        #region 只读属性 - 通知设置 —— virtual NotifierOptions NotifierOptions
        /// <summary>
        /// 只读属性 - 通知设置
        /// </summary>
        public virtual NotifierOptions NotifierOptions
        {
            get => new NotifierOptions();
        }
        #endregion

        #endregion

        #region # 方法

        #region 失去活动 —— override void OnDeactivate(bool close)
        /// <summary>
        /// 失去活动
        /// </summary>
#if NET40 || NET45
        protected override void OnDeactivate(bool close)
#endif
#if NET461_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
#endif
        {
            if (close)
            {
                this._notifier.Dispose();
            }
#if NET40 || NET45
            base.OnDeactivate(close);
#endif
#if NET461_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            return base.OnDeactivateAsync(close, cancellationToken);
#endif
        }
        #endregion

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

        #region 提示消息 —— void ToastInfo(string message...
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="onClick">点击事件</param>
        /// <param name="onClose">关闭事件</param>
        public void ToastInfo(string message, Action<NotificationBase> onClick = null, Action<NotificationBase> onClose = null)
        {
            MessageOptions options = new MessageOptions
            {
                FontSize = this.NotifierOptions.FontSize,
                ShowCloseButton = this.NotifierOptions.ShowCloseButton,
                FreezeOnMouseEnter = this.NotifierOptions.FreezeOnMouseEnter,
                UnfreezeOnMouseLeave = this.NotifierOptions.UnfreezeOnMouseLeave,
                NotificationClickAction = onClick,
                CloseClickAction = onClose
            };

            this._notifier.ShowInformation(message, options);
        }
        #endregion

        #region 提示成功 —— void ToastSuccess(string message...
        /// <summary>
        /// 提示成功
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="onClick">点击事件</param>
        /// <param name="onClose">关闭事件</param>
        public void ToastSuccess(string message, Action<NotificationBase> onClick = null, Action<NotificationBase> onClose = null)
        {
            MessageOptions options = new MessageOptions
            {
                FontSize = this.NotifierOptions.FontSize,
                ShowCloseButton = this.NotifierOptions.ShowCloseButton,
                FreezeOnMouseEnter = this.NotifierOptions.FreezeOnMouseEnter,
                UnfreezeOnMouseLeave = this.NotifierOptions.UnfreezeOnMouseLeave,
                NotificationClickAction = onClick,
                CloseClickAction = onClose
            };

            this._notifier.ShowSuccess(message, options);
        }
        #endregion

        #region 提示警告 —— void ToastWarning(string message...
        /// <summary>
        /// 提示警告
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="onClick">点击事件</param>
        /// <param name="onClose">关闭事件</param>
        public void ToastWarning(string message, Action<NotificationBase> onClick = null, Action<NotificationBase> onClose = null)
        {
            MessageOptions options = new MessageOptions
            {
                FontSize = this.NotifierOptions.FontSize,
                ShowCloseButton = this.NotifierOptions.ShowCloseButton,
                FreezeOnMouseEnter = this.NotifierOptions.FreezeOnMouseEnter,
                UnfreezeOnMouseLeave = this.NotifierOptions.UnfreezeOnMouseLeave,
                NotificationClickAction = onClick,
                CloseClickAction = onClose
            };

            this._notifier.ShowWarning(message, options);
        }
        #endregion

        #region 提示错误 —— void ToastError(string message...
        /// <summary>
        /// 提示错误
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="onClick">点击事件</param>
        /// <param name="onClose">关闭事件</param>
        public void ToastError(string message, Action<NotificationBase> onClick = null, Action<NotificationBase> onClose = null)
        {
            MessageOptions options = new MessageOptions
            {
                FontSize = this.NotifierOptions.FontSize,
                ShowCloseButton = this.NotifierOptions.ShowCloseButton,
                FreezeOnMouseEnter = this.NotifierOptions.FreezeOnMouseEnter,
                UnfreezeOnMouseLeave = this.NotifierOptions.UnfreezeOnMouseLeave,
                NotificationClickAction = onClick,
                CloseClickAction = onClose
            };

            this._notifier.ShowError(message, options);
        }
        #endregion

        #region 发送实时消息 —— void SendMessage(string content...
        /// <summary>
        /// 发送实时消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="messageType">消息类型</param>
        public void SendMessage(string content, LiveMessageType messageType)
        {
            this.OnUIThread(() =>
            {
                LiveMessage liveMessage = new LiveMessage(content, messageType);
                this.LiveMessages.Insert(0, liveMessage);
            });
        }
        #endregion

        #region 清空实时消息 —— void ClearMessages()
        /// <summary>
        /// 清空实时消息
        /// </summary>
        public void ClearMessages()
        {
            this.OnUIThread(() =>
            {
                this.LiveMessages.Clear();
            });
        }
        #endregion

        #region 导出实时消息 —— void ExportMessages()
        /// <summary>
        /// 导出实时消息
        /// </summary>
        public void ExportMessages()
        {
            IList<string> messages = new List<string>();
            foreach (LiveMessage liveMessage in this.LiveMessages.ToArray())
            {
                messages.Add($"{liveMessage.Content}\t{liveMessage.MessageType.ToString()}\t{liveMessage.AddedTime:yyyy-MM-dd HH:mm:ss.fff}");
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.txt)|*.txt",
                FileName = $"实时消息_{DateTime.Now:yyyyMMddHHmmss}",
                AddExtension = true,
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                File.WriteAllLines(filePath, messages);
            }
        }
        #endregion

        #region 在UI线程执行 —— void OnUIThread(Action action)
        /// <summary>
        /// 在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public void OnUIThread(System.Action action)
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
#if NET461 || NET462 || NETCOREAPP3_1
        /// <summary>
        /// 异步等待在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public Task OnUIThreadAsync(System.Func<Task> action)
        {
            return action.OnUIThreadAsync();
        }
#endif
        #endregion

        #endregion
    }
}
